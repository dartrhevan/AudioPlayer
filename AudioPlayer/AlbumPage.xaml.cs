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

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для AlbumPage.xaml
    /// </summary>
    
    public partial class AlbumPage : UserControl
    {
        private Album _album;
        public MainWindow Window => Application.Current.MainWindow as MainWindow;
        

        public Album Album
        {
            get => _album;
            set
            {
                int ind = 0;
                _album = value;
                foreach (var song in _album.Songs.Select(s =>
                {
                    var res = new SongRow(ind++, _album);
                    res.MouseDown += (send, args) =>
                    {
                        var song = (send as SongRow);
                        if (song.Index != song.Window.Player.CurrentIndex || song.Album != song.Window.Player.CurrentAlbum)
                            Window.Player.SetCurrentSongByIndexAndAlbum(song.Index, _album);
                    };
                    res.Style = (Style)Resources["MainStyle"];
                    return res;
                }))
                    Stack.Children.Add(song);

                ind = 0;
                foreach (var track in Window.Player.PlayList.Select(s =>
                {
                    var res = new PlayListRow(s.Item1, s.Item2, ind++);
                    res.MouseDown += (send, args) =>
                    {
                        var song = (send as PlayListRow);
                        if (song.Index != song.Window.Player.CurrentIndex || song.Album != song.Window.Player.CurrentAlbum)
                            Window.Player.SetCurrentSongByIndexAndAlbum(song.Index, _album);
                    };
                    res.Style = (Style)Resources["MainStyle2"];
                    return res;
                }))
                    PlayList.Children.Add(track);
                
                Cover.Source = (_album.Picture.Background as ImageBrush)?.ImageSource;
                NameLabel.Content = _album.AlbumName.Content as string;
                Author.Content = _album.Author.Content as string;
                Count.Content = _album.Songs.Count.ToString();
            }
        }

        public AlbumPage()
        {
            //Window = Window;
            InitializeComponent();
        }

        private void ReturnButtonOnClick(object sender, RoutedEventArgs e)
        {
            Window.Panel.Children.RemoveAt(Window.Panel.Children.Count - 1);
            Window.Panel.Children.Add(Window.MainPage);
            ExecuteAnimation(Window);
        }

        private static void ExecuteAnimation(MainWindow mainWindow)
        {
            var anim = new ThicknessAnimation
            {
                From = new Thickness(-mainWindow.Width, 0, 0, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 250))
            };
            mainWindow.MainPage.BeginAnimation(MarginProperty, anim);
        }
    }
}
