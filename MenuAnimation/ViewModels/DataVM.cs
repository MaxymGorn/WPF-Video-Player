using MenuAnimation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MenuAnimado1.ViewModels
{
    class DataVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Table Table;
        public DataVM(Table Table) => this.Table = Table;  

        public DataVM(){}

        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public int Id
        {
            get => Table.Id;
            set
            {
                Table.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get => Table.Name;
            set
            {
                Table.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Source
        {
            get => Table.Source;
            set
            {
                Table.Source = value;
                OnPropertyChanged("Source");
            }
        }

        public string Desciption
        {
            get => Table.Desciption;
            set
            {
                Table.Source = value;
                OnPropertyChanged("Desciption");
            }
        }
    }
}
