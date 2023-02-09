using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WPF_MusicPlayer_ekz
{// починить таскалку и кнопки после переключения двойным щелчком
    public partial class MainWindow : Window
    {//   перем. для работы с диалоговым окном
        OpenFileDialog ofd = new OpenFileDialog();
       
        //лист с плейлистами
        List<string> PlaylistsListPaths = new List<string>();
        List<string> ViewPlaylists=new List<string>();
        //лист с полным путём к файлам
        public List<string> CurrentPathsPlaylist = new List<string>();
        //лист с названиями треков для отображения без пути
        List<string> CurrentViewList = new List<string>();
        int selectedPlaylistId = 0;
        //номер текущего трека  в списке проигрывания
        int CurrentPlayTrackNumber = 0;
        int SelectedTrack = 0;
        string StatusBarText;
        //путь к папке с плейлистами
        string PathToPlaylists = "Playlists/";
       //число символов которое нужно убрать с конца имени плейлиста для отображения без расширения
        int TailLenght = 5;

        Storyboard st;
        MediaTimeline MedTimeLine;

        
        public MainWindow()
        {
            InitializeComponent();
            LoadPlaylists();
            //определение медиаэлемента
            st = (Storyboard)this.Resources["MediaStoryboardResource"];
            MedTimeLine = (MediaTimeline)st.Children[0];
        }

        public void LoadPlaylists()
        {
            //загрузка плейлистов
            if(ViewPlaylists.Count!=0)
            { ViewPlaylists.Clear();}
            PlaylistsListPaths = Directory.GetFiles(PathToPlaylists, "*.mpdf", SearchOption.AllDirectories).ToList();
            foreach (var item in PlaylistsListPaths)
            {
                FileInfo fileInf = new FileInfo(item);
                string playlistCleanName = fileInf.Name;
                playlistCleanName = playlistCleanName.Substring(0, playlistCleanName.Length - TailLenght);
                ViewPlaylists.Add(playlistCleanName);
            }
            cmb_Playlists.DataContext = null;
            cmb_Playlists.DataContext = ViewPlaylists;//
        }

        // метод позволяет перематывать проигрываемый отрывок
        private bool suppressSeek;
        private void sliderPosition_ValueChanged(object sender, RoutedEventArgs e)
        {         
            if (!suppressSeek)
                st.Seek(TimeSpan.FromSeconds(sliderPositionTime.Value), TimeSeekOrigin.BeginTime);
        }

        // настройка слайдера перемотки
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {         
            sliderPositionTime.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
        }        

        // метод запускается для обновления текущей проигрываемой позиции      
        private void storyboard_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            Clock storyboardClock = (Clock)sender;

            if (storyboardClock.CurrentProgress == null)
            {
                lblTime.Text = "";
            }
            else
            {
                // настроить вывод текущего времени проигрывания и позиции слайдера
                lblTime.Text = storyboardClock.CurrentTime.ToString();
                suppressSeek = true;
                sliderPositionTime.Value = storyboardClock.CurrentTime.Value.TotalSeconds;
                suppressSeek = false;
            }
        }     

        // drag'n'Drop для листа треков
         private void MusicList_DragDrop(object sender, DragEventArgs e)
         { 
            // Если перетаскивается список файлов
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Получить и напечатать список файлов
                string[] str = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in str)
                {
                    FileAttributes fAtr = System.IO.File.GetAttributes(item);//получить атрибуты по имени
                    FileInfo file = new FileInfo(item);

                    if ((fAtr & FileAttributes.Directory) == FileAttributes.Directory)//если это папка
                    {
                        CopyFromDir(item);//рекурс. функция копирования
                    }
                    else//если файл поддерживаемого расширения то добавить его в листы
                    {
                        if (item.EndsWith(".mp3") || item.EndsWith(".asf") || item.EndsWith(".wma") || item.EndsWith(".wav") || item.EndsWith(".au") )  
                        {
                            CurrentPathsPlaylist.Add(item);
                            CurrentViewList.Add(file.Name);
                        }
                    }
                }              
               
                MedTimeLine.Source = new Uri(CurrentPathsPlaylist[CurrentPlayTrackNumber], UriKind.RelativeOrAbsolute);
                //обновить отображение плейлиста
                listBoxPlaylist.DataContext = null;
                listBoxPlaylist.DataContext= CurrentViewList;
                //отображение текущего трека в статус бар
                StatusBarText = CurrentViewList[CurrentPlayTrackNumber];
                StatusBar.Header = StatusBarText;
            }
         }

        void CopyFromDir(string source )
        {
            foreach (string files in Directory.GetFiles(source))
            {
                string newFiles = source + "\\" + System.IO.Path.GetFileName(files);
                if (newFiles.EndsWith(".mp3") || newFiles.EndsWith(".asf") || newFiles.EndsWith(".wma") || newFiles.EndsWith(".wav") || newFiles.EndsWith(".au"))
                {
                    FileInfo file=new FileInfo(newFiles);
                    CurrentPathsPlaylist.Add(newFiles);
                    CurrentViewList.Add(file.Name);
                }
            }
            foreach (string dirs in Directory.GetDirectories(source))
            {
                CopyFromDir( source + "\\" + System.IO.Path.GetFileName(dirs));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {//добавить треки по кнопке через диалоговое окно
            ofd.Multiselect = true;
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "All files (*.mp3; *.asf; *.wma; *.wav; *.au) | *.mp3; *.asf; *.wma; *.wav; *.au";
            ofd.ShowDialog();
            string[] filenames = ofd.FileNames;
            ofd.Multiselect = true;
            foreach (var item in filenames)
            {//поместить обьекты в лист пути и лист отображения
                FileInfo file = new FileInfo(item);
                CurrentPathsPlaylist.Add(item);
                CurrentViewList.Add(file.Name);
            }
            //обновить список отображения
            listBoxPlaylist.DataContext = null;
            listBoxPlaylist.DataContext = CurrentViewList;
        }
     
        //кнопка вызова окна сохранения плейлиста
        private void SavePlst_Click(object sender, RoutedEventArgs e)
        {
            Enter_playlist_name epn=new Enter_playlist_name(this);
            epn.Show();
            epn.Focus();
        }

        private void ViewLikeList_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ViewLikeTile_Click(object sender, RoutedEventArgs e)
        {

        }
         
         //кнопка след. трек
        private void MenuNext_Click(object sender, RoutedEventArgs e)
        {
            if ((CurrentViewList.Count != 0) && (listBoxPlaylist.SelectedIndex != -1))
            {               
                CurrentPlayTrackNumber++;
                if (CurrentPlayTrackNumber<CurrentPathsPlaylist.Count)
                {
                    MedTimeLine = (MediaTimeline)st.Children[0];
                    MedTimeLine.Source = new Uri(CurrentPathsPlaylist[CurrentPlayTrackNumber], UriKind.RelativeOrAbsolute);
                    st.Begin();
                }
                else
                    CurrentPlayTrackNumber--;

                //отображение текущего трека в статус бар
                StatusBarText = CurrentViewList[CurrentPlayTrackNumber];
                StatusBar.Header = StatusBarText;
            }           
        }

        //кнопка пред. трек
        private void MenuPrevious_Click(object sender, RoutedEventArgs e)
        {
            if ((CurrentViewList.Count != 0) && (listBoxPlaylist.SelectedIndex != -1))
            {             
                CurrentPlayTrackNumber--;
                if (CurrentPlayTrackNumber >= 0)
                {
                    MedTimeLine = (MediaTimeline)st.Children[0];
                    MedTimeLine.Source = new Uri(CurrentPathsPlaylist[CurrentPlayTrackNumber], UriKind.RelativeOrAbsolute);
                    st.Begin();
                }
                else
                    CurrentPlayTrackNumber++;

                //отображение текущего трека в статус бар
                StatusBarText = CurrentViewList[CurrentPlayTrackNumber];
                StatusBar.Header = StatusBarText;
            }          
        }
 
        //двойной щелчок в по пукту в плейлисте
        private void PlaylistDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((CurrentViewList.Count != 0) && (listBoxPlaylist.SelectedIndex != -1))
            {
                CurrentPlayTrackNumber = listBoxPlaylist.SelectedIndex;

                MedTimeLine = (MediaTimeline)st.Children[0];
                MedTimeLine.Source = new Uri(CurrentPathsPlaylist[CurrentPlayTrackNumber], UriKind.RelativeOrAbsolute);
                st.Begin();  
                //отображение текущего трека в статус бар
                StatusBarText = CurrentViewList[CurrentPlayTrackNumber];
                StatusBar.Header = StatusBarText;        
            }          
        }

        //удаление трека из текущего листа 
        private void cm_DeleteClick(object sender, RoutedEventArgs e)
        {
            if ((CurrentViewList.Count != 0) && (listBoxPlaylist.SelectedIndex != -1))
            {            
                SelectedTrack = listBoxPlaylist.SelectedIndex;
                CurrentPathsPlaylist.Remove(CurrentPathsPlaylist[SelectedTrack]);
                CurrentViewList.Remove(CurrentViewList[SelectedTrack]);
                //обновить список отображения
                listBoxPlaylist.DataContext = null;
                listBoxPlaylist.DataContext = CurrentViewList;
            }           
        }

        //автопереключение на след трек 
        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if ((CurrentViewList.Count != 0) && (listBoxPlaylist.SelectedIndex != -1))
            {
                // починить таскалку
                CurrentPlayTrackNumber++;
                if (CurrentPlayTrackNumber < CurrentPathsPlaylist.Count)
                { 
                    MedTimeLine = (MediaTimeline)st.Children[0];
                    MedTimeLine.Source = new Uri(CurrentPathsPlaylist[CurrentPlayTrackNumber], UriKind.RelativeOrAbsolute);
                    st.Begin();  
                }
                else
                    CurrentPlayTrackNumber--; 
                //отображение текущего трека в статус бар
                StatusBarText = CurrentViewList[CurrentPlayTrackNumber];
                StatusBar.Header = StatusBarText;
            }          
        }

        private void cmdStopClick(object sender, RoutedEventArgs e)
        {
             st.Stop();            
        }

        private void cmdPauseClick(object sender, RoutedEventArgs e)
        {
             st.Pause();
        }

        private void cmdResumeClick(object sender, RoutedEventArgs e)
        {
             st.Resume();
        }

        private void cmdPlayClick(object sender, RoutedEventArgs e)
        {
            if (CurrentPathsPlaylist.Count>0)
            {
                MedTimeLine = (MediaTimeline)st.Children[0];
                MedTimeLine.Source = new Uri(CurrentPathsPlaylist[CurrentPlayTrackNumber], UriKind.RelativeOrAbsolute);
                st.Begin();
            }           
        }

        //выбор плейлиста
        private void ItemComboBoxMouseDown(object sender, MouseButtonEventArgs e)
        {        
            Console.WriteLine("ItemComboBoxMouseDown "+ cmb_Playlists.SelectedIndex.ToString());
            if ((PlaylistsListPaths.Count != 0) && (cmb_Playlists.SelectedIndex != -1))
            {
                CurrentPathsPlaylist.Clear();
                CurrentViewList.Clear();
                CurrentPlayTrackNumber = 0;
                selectedPlaylistId = cmb_Playlists.SelectedIndex;
                StreamReader f = new StreamReader(PlaylistsListPaths[selectedPlaylistId]);
                while (!f.EndOfStream)
                {
                    string s = f.ReadLine();
                    FileInfo file = new FileInfo(s);
                    CurrentPathsPlaylist.Add(s);
                    CurrentViewList.Add(file.Name);
                }
                f.Close();
                listBoxPlaylist.DataContext = null;
                listBoxPlaylist.DataContext = CurrentViewList;                                   
            }          
        }

        // кнопка удаления плейлиста
        private void DeletePlst_Click(object sender, RoutedEventArgs e)
        {
            System.IO.File.Delete(PlaylistsListPaths[selectedPlaylistId]);
            LoadPlaylists();
            listBoxPlaylist.DataContext = null;
        }

     
    }
}
