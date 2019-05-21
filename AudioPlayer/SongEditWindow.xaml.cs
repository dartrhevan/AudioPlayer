using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TagLib;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;/*
using Microsoft.Office.Interop.Word.Application;
using Microsoft.Office.Interop.Word.Document;
using Microsoft.Office.Interop.Word.Selection;
using Microsoft.Office.Interop.Word.Bookmark;
using Microsoft.Office.Interop.Word.Range;*/
using Microsoft.Office.Interop.Word;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using Task = System.Threading.Tasks.Task;
using Word = Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;

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
            Title.Text = Album.Songs[index].Tag.Title;
            AlbumTitle.Text = Album.Songs[index].Tag.Album??"";
            Year.Text = Album.Songs[index].Tag.Year.ToString();// != null ? Album.Songs[index].Tag.Year.ToString();
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
            var tagAlbum = Album.Songs[index].Tag.Album;
            if (tagAlbum != null && tagAlbum.ToString() == "System.String[]")
                AlbumTitle.Text = "Неизвестный альбом";
            if (Album.Songs[index].Tag.Year == 0)
                Year.Text = "0";
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            File song = Album.Songs[Index];
            song.Tag.Title = Title.Text;
            song.Tag.Album = AlbumTitle.Text;
            song.Tag.Year = uint.Parse(Year.Text);
            song.Tag.Genres = new[] { Genre.Text.Replace(';', ' ') };
            song.Tag.Performers = new[] { Performers.Text.Replace(';', ' ') };
            song.Tag.Track = uint.Parse(AlbumIndex.Text);

            Window.Player.CurrentPlayer.Stop();
            Window.Player.CurrentPlayer.Close();
            Window.Player.CurrentPlayer.Close();
            Thread.Sleep(200);
            //Window.Player.CurrentSong -
            //song.Dispose();,
            try
            {
                song.Save();

            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Не удалось изменить информацию", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            Window.Player.CurrentPlayer.Open(new Uri(song.Name));

            this.Close();
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public MainWindow Window
        {
            get => Application.Current.MainWindow as MainWindow;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new SaveFileDialog();
            var res = openFolderDialog.ShowDialog();
            if (!(res == System.Windows.Forms.DialogResult.Yes ||
                  res == System.Windows.Forms.DialogResult.OK && openFolderDialog.FileName != "")) return;


            var task = new Task(() =>
            {
                File song = Album.Songs[Index];
                var wdApp = new Word.Application();
                //wdApp.Visible = true;
                wdApp.Documents.Add();
                var docum = wdApp.Documents.get_Item(1);
                wdApp.Selection.Text =
                    $"Название: {song.Tag.Title ?? ""}\nАльбом: {song.Tag.Album ?? ""}\nИсполнитель: {string.Join(", ", song.Tag.Performers)}\n Жанр: {string.Join(", ", song.Tag.Genres)}\n Год: {song.Tag.Year}\n";
                docum.SaveAs(openFolderDialog.FileName);
                docum.Close();
            });
            task.Start();
            await task;
        }
    }
}
