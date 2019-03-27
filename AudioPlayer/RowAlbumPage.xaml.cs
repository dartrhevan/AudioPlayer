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
    
    public partial class RowAlbumPage : UserControl
    {
        public readonly MyPlayer Player;
        public readonly MainWindow Window;// { get; set; }
        public RowAlbumPage(MyPlayer player)
        {
            Player = player;
            InitializeComponent();
            var i = 0;
            foreach (var album in Player.Albums)
            {
                var albumButton = new Button()
                {
                    Content = album.AlbumName.Content,
                };
                albumButton.MouseDown += delegate
                {
                    foreach (var song in album.Songs.Select(s =>
                    {
                        var res = new SongRow(s, Window);// {Content = s.Tag.Title, Margin = new Thickness(0, 0, 0, 12), Height = 20};
                        res.MouseDown += (send, args) =>
                        {
                            Window.Player.CurrentSong =
                                Window.Player.Songs.Find(f => f.Tag.Title == ((SongRow)send).Label.Content as string);
                        };
                        //res.Template = (ControlTemplate) Resources["btTemplate"];
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
        {
            if (AlbumStack.Children.Count < Player.Albums.Count)
                for (var i = AlbumStack.Children.Count; i < Player.Albums.Count; ++i)
                    AlbumStack.Children.Add(Player.Albums[i]);
        }

        public void Reset()
        {
            AlbumStack.Children.Clear();
            foreach (var album in Player.Albums)
                AlbumStack.Children.Add(album);
        }
    }
}
