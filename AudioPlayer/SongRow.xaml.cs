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
            Label.Content = Album.Songs[index].Tag.Title??Album.Songs[index].Name.Split('\\').Last();
            Margin = new Thickness(0, 0, 0, 12);
            Height = 20;
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
    }
}
