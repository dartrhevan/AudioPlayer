using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using AudioPlayer.Models;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для AlbumPage.xaml
    /// </summary>
    
    public partial class RowAlbumPage : UserControl, IMainPage
    {
        public readonly MyPlayer Player;
        public MainWindow Window
        {
            get => Application.Current.MainWindow as MainWindow;
        }
        public RowAlbumPage()
        {
            Player = Window.Player;
            InitializeComponent();
            var i = 0;
            foreach (var album in Player.Albums)
            {
                var albumButton = new Button()
                {
                    Content = album.Value.AlbumName.Content,
                };
                albumButton.MouseDown += (se, arg) =>
                {
                    while (SongStack.Children.Count > 0)
                    {
                        SongStack.Children.RemoveAt(SongStack.Children.Count - 1);
                    }
                    int ind = 0;
                    foreach (var song in album.Value.Songs.Select(s =>
                    {
                        var res = new SongRow(ind++, album.Value);
                        res.MouseLeftButtonDown += (send, args) =>
                        {
                            var song = (send as SongRow);
                            if (song.Index != song.Window.Player.CurrentIndex || song.Album != song.Window.Player.CurrentAlbum)
                                Window.Player.SetCurrentSongByIndexAndAlbum(song.Index, album.Value);
                        };
                        res.Style = (Style)Resources["MainStyle"];
                        return res;
                    }))
                    {
                        SongStack.Children.Add(song);
                    }
                };
                if (i % 2 == 0)
                    albumButton.Background = Brushes.White;
                else albumButton.Background = Brushes.LightGray;
                AlbumStack.Children.Add(albumButton);
                i++;
            }
        }


        public void Update()
        {/*
            if (AlbumStack.Children.Count < Player.Albums.Count)
                for (var i = AlbumStack.Children.Count; i < Player.Albums.Count; ++i)
                    AlbumStack.Children.Add(Player.Albums[i]);*/

            var albs = new List<Album>();
            if (AlbumStack.Children.Count < Player.Albums.Count)
                foreach (Album album in AlbumStack.Children)
                    if (Player.Albums.ContainsKey(album.AlbumName.Content as string))
                        albs.Add(album);
            foreach (var album in albs)
                AlbumStack.Children.Add(album);
        }

        public void Reset()
        {
            AlbumStack.Children.Clear();
            foreach (var album in Player.Albums)
                AlbumStack.Children.Add(album.Value);
        }
    }
}
