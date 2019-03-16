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
        public  MyPlayer Player { get; set; }

        public Album Album
        {
            get => album;
            set
            {
                album = value;
                foreach (var song in album.Songs.Select(s =>
                {
                    var res = new SongRow(s);// {Content = s.Tag.Title, Margin = new Thickness(0, 0, 0, 12), Height = 20};
                    res.MouseUp += (send, args) =>
                    {
                        Player.CurrentSong =
                            Player.Songs.Find(f => f.Tag.Title == ((SongRow) send).Label.Content as string);
                    };
                    res.Template = (ControlTemplate) Resources["btTemplate"];
                    res.Style = (Style)Resources["MainStyle"];
                    return res;
                }))
                {
                    Stack.Children.Add(song);
                }

            }
        }

        public AlbumPage()
        {
            InitializeComponent();
        }
    }
}
