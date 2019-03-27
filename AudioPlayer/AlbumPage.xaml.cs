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
        private Album album;
        public readonly MainWindow Window;// { get; set; }


        public Album Album
        {
            get => album;
            set
            {
                int ind = 0;
                album = value;
                foreach (var song in album.Songs.Select(s =>
                {
                    var res = new SongRow(s, Window, ind++, album);// {Content = s.Tag.Title, Margin = new Thickness(0, 0, 0, 12), Height = 20};
                    res.MouseDown += (send, args) =>
                    {
                        Window.Player.SetCurrentSongByIndexAndAlbum((send as SongRow).Index, album);
                        //Window.Player.CurrentSong =
                        //    Window.Player.Songs.Find(f => f.Tag.Title == ((SongRow) send).Label.Content as string);
                    };
                    //res.Template = (ControlTemplate) Resources["btTemplate"];
                    res.Style = (Style)Resources["MainStyle"];
                    return res;
                }))
                {
                    Stack.Children.Add(song);
                }

            }
        }

        public AlbumPage(MainWindow window)
        {
            Window = window;
            InitializeComponent();
        }

        //private MainPage GetMainPage()
        //{
        //    var par = (Parent as DockPanel).Parent as MainWindow;
        //    var albums = par.MainPage; //par.Children[par.Children.Count - 1] as MainPage;
        //    return albums;
        //}


        private void ReturnButtonOnClick(object sender, RoutedEventArgs e)
        {
            //var mainWindow = (Parent as DockPanel).Parent as MainWindow;
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
