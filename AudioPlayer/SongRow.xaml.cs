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

namespace AudioPlayer
{
    /// <summary>
    /// Строка одной песни в списке
    /// </summary>
    public partial class SongRow : UserControl
    {
        public readonly MainWindow Window;
        public readonly Album Album;
        public readonly int Index;
        public SongRow(MainWindow window, int index, Album album)
        {
            this.Window = window;
            InitializeComponent();
            Album = album;
            SongIndex.Content = Album.Songs[index].Tag.Track.ToString();
            Label.Content = Album.Songs[index].Tag.Title ?? Album.Songs[index].Name.Split('\\').Last();
            Duration.Content = String.Format("{0:00}:{1:00}", Album.Songs[index].Properties.Duration.Minutes, Album.Songs[index].Properties.Duration.Seconds);
           // Margin = new Thickness(0, 0, 0, 0);
            Height = 25;
            Index = index;
        }

        private void Edit_Click(object sender, RoutedEventArgs args)
        {
            var window = new SongEditWindow(Album, Index);
            window.Show();
        }

        private void PlayOnClick(object sender, RoutedEventArgs e)
        {
            if (Index != Window.Player.CurrentIndex || Album != Window.Player.CurrentAlbum)
            {
                if (Window.Player.Playing) Window.Bar.PauseStart();
                Window.Player.SetCurrentSongByIndexAndAlbum(Index, Album);
                //if (!Window.Bar.Playing) Window.Bar.PauseStart();
            }
            //else
            Window.Bar.PauseStart();
        }
    }
}
