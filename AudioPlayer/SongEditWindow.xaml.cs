using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagLib;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для SongEditWindow.xaml
    /// </summary>
    public partial class SongEditWindow : Window
    {
        Album Album;
        int Index;
        public SongEditWindow(Album album, int index)
        {
            InitializeComponent();
            Album = album;
            Index = index;
            Title.Text = Album.Songs[index].Tag.Title.ToString();
            AlbumTitle.Text = Album.Songs[index].Tag.Album.ToString();
            Year.Text = Album.Songs[index].Tag.Year.ToString();
            foreach( var e in Album.Songs[index].Tag.Genres)
            {
                Genre.Text += e + "; ";
                if (e == null) Genre.Text = "-";
            }
            foreach (var e in Album.Songs[index].Tag.Performers)
            {
               Performers.Text += e + "; ";
                if (e == null) Performers.Text = "Неизвестный исполнитель";
            }
            AlbumIndex.Text = Album.Songs[index].Tag.Track.ToString();
            if (Album.Songs[index].Tag.Title.ToString() == "System.String[]")
                Title.Text = "Без названия";
            if (Album.Songs[index].Tag.Album.ToString() == "System.String[]")
                AlbumTitle.Text = "Неизвестный альбом";
            if (Album.Songs[index].Tag.Year == 0)
                Year.Text = "?";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Album.Songs[Index].Tag.Title = Title.Text;
            Album.Songs[Index].Tag.Album = AlbumTitle.Text;
            Album.Songs[Index].Tag.Year = uint.Parse(Year.Text);
            Album.Songs[Index].Tag.Genres = new[] { Genre.Text.Replace(';', ' ') };
            Album.Songs[Index].Tag.Performers = new[] { Performers.Text.Replace(';', ' ') };
            Album.Songs[Index].Tag.Track = uint.Parse(AlbumIndex.Text);
            this.Close();
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
