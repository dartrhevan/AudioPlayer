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
    /// Логика взаимодействия для PlayListRow.xaml
    /// </summary>
    public partial class PlayListRow : UserControl
    {
        public MainWindow Window
        {
            get => Application.Current.MainWindow as MainWindow;
        }
        public readonly Album Album;
        public readonly int Index;
        public readonly int PlayListIndex;

        public PlayListRow(int index, Album album, int playListIndex)
        {
            PlayListIndex = playListIndex;
            //this.Window = Window;
            InitializeComponent();
            Album = album;
            SongIndex.Content = index + 1;
            Label.Content = Album.Songs[index].Tag.Title ?? Album.Songs[index].Name.Split('\\').Last();
            // Margin = new Thickness(0, 0, 0, 0);
            Height = 25;
            Index = index;

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

        private void FromPlayListOnClick(object sender, RoutedEventArgs e)
        {
            Window.Player.PlayList.RemoveAt(PlayListIndex);
            Window.Bar.PauseStart();
            Window.Player.SetCurrentSongByIndexAndAlbum(Index, Album);
            Window.Album.PlayList.Children.Remove(this);
        }
    }
}
