using MenuAnimado1.Commands;
using MenuAnimation.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace MenuAnimado1.ViewModels
{


    class AppViewModel : INotifyPropertyChanged
    {
        #region констуктор  

        public AppViewModel()
        {
           DataVMs  = new ObservableCollection<DataVM>();

            LoadData();
        
        }
        #endregion

        #region функции

        void LoadData()
        {
            DataVMs.Clear();
            dynamic taskses = database.Tables.ToList();
            foreach (var el in taskses)
            {
                DataVMs.Add(new DataVM(el));
            }
        }

        internal void FileDBExist(int v = 1)
        {
            try
            {
                var tabl = database.Tables;
                foreach (var el in tabl)
                {
                    if (!File.Exists(el.Source))
                    {
                        database.Tables.Remove(el);
                    }
                }
                if (v == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                database.SaveChanges();
            }
        }

        private void findImagesInDirectory(string path)
        {
            List<string> pathf = new List<string>();
            List<string> filenames = new List<string>();
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                if (s.EndsWith(".mp4") || s.EndsWith(".avi") || s.EndsWith(".mpg") || s.EndsWith(".mts") || s.EndsWith(".m2ts") || s.EndsWith(".mkv") || s.EndsWith(".3gp") || s.EndsWith(".flv") || s.EndsWith(".wmv"))
                {
                    pathf.Add(s);
                    filenames.Add(s.Remove(0, path.Length + 1));
                }
            }
            List<Table> tables = new List<Table>();
            for (int i = 0; i < filenames.Count; i++)
            {
                tables.Add(new Table() { Id = 0, Name = filenames[i], Desciption = "" });
                tables[i].Source = @"..\..\Images s Video/" + tables[i].Name;
            }
            for (int i = 0; i < tables.Count; i++)
            {
                if (!File.Exists(@"..\..\Images s Video/" + tables[i].Name))
                {
                    File.Copy(pathf[i], @"..\..\Images s Video/" + tables[i].Name);
                    tables[i].Name=tables[i].Name.Substring(0, tables[i].Name.Length - 4);                   
                    DataVMs.Add(new DataVM(tables[i]));
                    FileDBExist();
                }

            }



        }


        #endregion

        #region Переменние и свойства

        public ObservableCollection<DataVM> DataVMs  { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

     
        private  Database1Entities database = new Database1Entities();

        private DataVM selectedData;
        private RelayCommand fileAdd;
        private RelayCommand saveF;
        private RelayCommand deletefile;
        private RelayCommand fileaddcolletion;

        public DataVM SelectedData
        {
            get => this.selectedData;
            set
            {
                this.selectedData = value;
                OnPropertyChanged("SelectedData");
            }

        }
   
        #endregion

        #region команды

        public RelayCommand FileAddcolletion
        {
            get
            {

                return fileaddcolletion ?? (fileaddcolletion = new RelayCommand(obj =>
                {
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        findImagesInDirectory(folderBrowserDialog.SelectedPath);
                    }
                }));
            } 
        }
        

        public RelayCommand SaveF
        {
            get
            {

                return saveF ?? (saveF = new RelayCommand(obj =>
                {
                    var select1 = DataVMs.Where(c => c.Id == 0).ToList();
                    foreach (var el in select1)
                    {
                        database.Tables.Add(new Table() { Name = el.Name, Id = el.Id, Source = el.Source, Desciption = el.Desciption }); ;
                    }
                    FileDBExist();
                    database.SaveChanges();
                    LoadData();
                }));
            } }
        public RelayCommand FileAdd
        {
            get
            {

                return fileAdd ?? (fileAdd = new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Media files (*.mp4;*.avi;*.mpg;*.mts;*.m2ts;*.mkv;*.flv;*.3gp;*.wmv)|*.mp4;*.avi;*.mpg;*.mts;*.m2ts;*.mkv;*.flv;*.3gp;*.wmv";
                    if (openFileDialog.ShowDialog() == true)
                    {
                            Table table1 = new Table();
                            table1.Desciption = "";
                            table1.Name = openFileDialog.SafeFileName;
                            table1.Id = 0;
                            table1.Source = @"..\..\Images s Video/" + openFileDialog.SafeFileName;
                        try
                        {
                            if (File.Exists(table1.Source))
                            {
                                throw new Exception("Ошибка");
                            }
                            File.Copy(openFileDialog.FileName, table1.Source);
                            table1.Name = table1.Name.Substring(0, table1.Name.Length - 4);
                            DataVMs.Add(new DataVM(table1));
                            FileDBExist();
                        }
                        catch(Exception er)
                        {
                            MessageBox.Show("Файл уже существует!", er.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }));
            }

          }
        
        public RelayCommand FileDelete
        {
            get
            {
                return deletefile ?? (deletefile = new RelayCommand(obj =>
                {
                    try
                    {
                        var categoty = database.Tables.Where(c => c.Name == SelectedData.Name).FirstOrDefault();
                        if (categoty == null)
                        {
                            throw new Exception();
                        }
                        database.Tables.Remove(categoty);
                        database.SaveChanges();
                        LoadData();
                        File.Delete(categoty.Source);
                    }
                    catch (Exception)
                    {

                    }
                }));
            }
        }

        #endregion
    }
}
