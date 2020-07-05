using MenuAnimado1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Net.Http;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace MenuAnimado1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _blue;
        AppViewModel vm = new AppViewModel();
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private bool randomPlay = false;
        public MainWindow()
        {
            InitializeComponent();
           
            this.DataContext = vm;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            _blue = false;
            vm.FileDBExist(0);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((myMediaElement.Source != null) && (myMediaElement.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = myMediaElement.Position.TotalSeconds;
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }



        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (myMediaElement != null) && (myMediaElement.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaElement.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaElement.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaElement.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            myMediaElement.Position = TimeSpan.FromSeconds(sliProgress.Value);
            if (lblProgressStatus.Text == myMediaElement.NaturalDuration.ToString())
            {
                switch (randomPlay)
                {
                    case false:
                        {
                            if (list_Data.SelectedIndex != list_Data.Items.Count - 1)
                            {
                                list_Data.SelectedIndex++;
                            }
                            else
                            {
                                list_Data.SelectedIndex = 0;
                            }
                            break;
                        }
                    case true:
                        {
                            Random random = new Random();
                            list_Data.SelectedIndex = random.Next(0, list_Data.Items.Count);
                            break;
                        }
                }
        
                
            }
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            myMediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
           
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            if (_blue)
                Application.Current.Resources.MergedDictionaries[0]=new ResourceDictionary() { Source = new Uri("Blue.xaml", UriKind.Relative) };
            else
                Application.Current.Resources.MergedDictionaries[0]=new ResourceDictionary() { Source = new Uri("Black.xaml", UriKind.Relative) };
            _blue = !_blue;
        }



        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            myMediaElement.Play();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            myMediaElement.Stop();
        }

        private void Ellipse_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            myMediaElement.Pause();
        }



   

        private void list_Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Random random = new Random();
            myMediaElement.Play();
            followersTB.Text = (random.Next(0,7000000)).ToString();
            followersTB2.Text = (random.Next(0, 7000000)).ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            switch(randomPlay)
            {
                case false :
                    {
                        if (list_Data.SelectedIndex == -1 || list_Data.SelectedIndex == list_Data.Items.Count - 1)
                        {
                            list_Data.SelectedItem = list_Data.Items[0];
                        }
                        else

                        if (list_Data.SelectedItems.Count > 0)
                        {
                            list_Data.SelectedItem = list_Data.Items[list_Data.SelectedIndex + 1];
                        }
                        break;
                    }
                case true :
                    {
                        Random random = new Random();
                        list_Data.SelectedIndex = random.Next(0, list_Data.Items.Count);
                        break;
                    }
            }
   
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            switch (randomPlay)
            {
                case false:
                    {
                        if (list_Data.SelectedIndex == -1 || list_Data.SelectedIndex == 0)//
                        {
                            list_Data.SelectedItem = list_Data.Items[list_Data.Items.Count - 1];
                        }
                        else

                        if (list_Data.SelectedItems.Count > 0)
                        {
                            list_Data.SelectedItem = list_Data.Items[list_Data.SelectedIndex - 1];
                        }
                        break;
                    }
                case true:
                    {
                        Random random = new Random();
                        list_Data.SelectedIndex = random.Next(0, list_Data.Items.Count);
                        break;
                    }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
    
            if(randomPlay==false)
            {
                RandomPlayVideo.ToolTip = "Отлючить рандомный выбор";
                randomPlay = true;
            }
            else
            {
                RandomPlayVideo.ToolTip = "Включить рандомный выбор";
                randomPlay = false;
            }
        
        }
    }


}
