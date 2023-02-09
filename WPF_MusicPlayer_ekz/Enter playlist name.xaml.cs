using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace WPF_MusicPlayer_ekz
{
    /// <summary>
    /// Interaction logic for Enter_playlist_name.xaml
    /// </summary>
    public partial class Enter_playlist_name : Window
    {
        MainWindow mw;
        // напомнить  как обратится отсюда к полям главного окна
        public Enter_playlist_name(MainWindow mn)
        {
            InitializeComponent();
            mw= mn;
        }
        

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            if (name.Count()>0)
            {
                StreamWriter SW = new StreamWriter(new FileStream($"Playlists/{name}.mpdf", FileMode.Create, FileAccess.Write));
                foreach (var item in mw.CurrentPathsPlaylist)
                {
                    SW.WriteLine(item);
                }
                    SW.Close();
                mw.LoadPlaylists();
                this.Close();
            }
           
        }
    }
}
