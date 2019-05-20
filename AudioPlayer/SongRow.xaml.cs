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
        public MainWindow Window
        {
            get => Application.Current.MainWindow as MainWindow;
        }
        public readonly Album Album;
        public readonly int Index;
        public SongRow(int index, Album album)
        {
            
            //this.Window = window;
            InitializeComponent();
            Album = album;
            SongIndex.Content = Album.Songs[index].Tag.Track.ToString();
            Label.Content = Album.Songs[index].Tag.Title ?? Album.Songs[index].Name.Split('\\').Last();
            Duration.Content = String.Format("{0:00}:{1:00}", Album.Songs[index].Properties.Duration.Minutes, Album.Songs[index].Properties.Duration.Seconds);
           // Margin = new Thickness(0, 0, 0, 0);
            Height = 25;
            Index = index;
            
        }

        private void EditClick(object sender, RoutedEventArgs args)
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

        private void ToPlayListOnClick(object sender, RoutedEventArgs e)
        {
            Window.Player.PlayList.Add(Tuple.Create(Index, Album));
            var row = new PlayListRow(Index, Album, Window.Player.PlayList.Count - 1);
            row.MouseDown += (send, args) =>
            {
                var song = (send as PlayListRow);
                if (song.Index != song.Window.Player.CurrentIndex || song.Album != song.Window.Player.CurrentAlbum)
                    Window.Player.SetCurrentSongByIndexAndAlbum(song.Index, Album);
            };
            row.Style = (Style)Window.Album.Resources["MainStyle2"];
            if (Window.Player.PlayList.Count == 1)
            {
                //Window.Bar.PauseStart();
                Window.Player.SetCurrentSongByIndexAndAlbum(Index, Album);
                Window.Player.CurrentPlayListIndex = Window.Player.PlayList.Count - 1;

            }

            Window.Album.PlayList.Children.Add(row);

        }
    }
}
