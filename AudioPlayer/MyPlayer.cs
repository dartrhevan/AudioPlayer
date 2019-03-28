using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TagLib;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using File = TagLib.File;

namespace AudioPlayer
{
    public class MyPlayer
    {
        private readonly MainWindow window;
        public Album CurrentAlbum { get; set; }
        public File CurrentSong
        {
            get => currentSong;
            private set
            {
                currentSong = value;
                CurrentPlayer.Close();
                CurrentPlayer.Open(new Uri(currentSong.Name, UriKind.Relative));
            }
        }
        public bool Playing { get; set; }

        public void Next()
        {
            SetCurrentSongByIndexAndAlbum(CurrentIndex + 1, CurrentAlbum);
        }

        public void Previous()
        {
            SetCurrentSongByIndexAndAlbum(CurrentIndex - 1, CurrentAlbum);
        }

        public void SetCurrentSongByIndexAndAlbum(int index, Album album)
        {
            if (album.Songs.Count > index && index >= 0)
            {
                CurrentSong = album.Songs[index];
                CurrentIndex = index;
                CurrentAlbum = album;
            }
            //else throw new Exception("Fuck you!");
        }

        public readonly MediaPlayer CurrentPlayer = new MediaPlayer();
        private static readonly DirectoryInfo MainDirectory = new DirectoryInfo(@"C:\MyPlayerDirectory");
        DirectoryInfo curreDirectory = MainDirectory;
        public int CurrentIndex { get; private set; }
        public List<File> Songs;
        public List<Album> Albums;
        private File currentSong;

        public MyPlayer(MainWindow window)
        {
            this.window = window;
            OpenCurrentDirectory();
        }

        private void OpenCurrentDirectory()
        {
            var files = curreDirectory.GetFiles()
                .Select(f =>
                {
                    try
                    {
                        return File.Create(Path.Combine(f.DirectoryName, f.Name));
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(f => f != null);
            Songs = new List<File>(files);
            Albums = new List<Album>(files.GroupBy(f => f.Tag.Album)
                .Select(g => new Album(g, g.Key, String.Join(", ", g.First().Tag.Performers), (g.FirstOrDefault(a => a.Tag.Pictures.Length > 0) ?? g.First()).Tag.Pictures, window)));
            //if (currentSong == null)
            //{
                CurrentAlbum = Albums[Albums.Count - 1];
                CurrentIndex = CurrentAlbum.Songs.Count - 1;
                CurrentSong = CurrentAlbum.Songs[CurrentIndex];
            //}
        }

        public void OpenFolder(string folderPath)
        {
            curreDirectory = new DirectoryInfo(folderPath);
            OpenCurrentDirectory();
        }

        public void AddSong(string path)
        {
            var song = File.Create(path);
            Songs.Add(song);
            var album = new Album(new[] { song }, song.Tag.Album, String.Join(", ", song.Tag.Performers), song.Tag.Pictures, window);
            var albInd = Albums.FindIndex(a => a.AlbumName.Content as string == album.AlbumName.Content as string && a.Author.Content as string == album.Author.Content as string);
            if (albInd == -1)
            {
                Albums.Add(album);
                CurrentIndex = albInd;
            }
            else
            {
                CurrentIndex = Albums.Count;
                Albums[albInd].Songs.Add(song);
            }
            CurrentAlbum = album;
            CurrentSong = song;

        }
    }
}
