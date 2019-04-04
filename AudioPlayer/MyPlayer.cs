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
using System.Windows.Forms;
using System.Windows.Media.Animation;
using File = TagLib.File;

namespace AudioPlayer
{
    public class MyPlayer
    {
        public int CurrentPlayListIndex { get; set; } = 0;
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
            if(PlayList.Count == 0)
                SetCurrentSongByIndexAndAlbum(CurrentIndex + 1, CurrentAlbum);
            else
            {
                if(CurrentPlayListIndex == PlayList.Count - 1) return;
                var track = PlayList[++CurrentPlayListIndex];
                SetCurrentSongByIndexAndAlbum(track.Item1, track.Item2);
            }

        }

        public void Previous()
        {
            if (PlayList.Count == 0)
                SetCurrentSongByIndexAndAlbum(CurrentIndex - 1, CurrentAlbum);
            else
            {
                if (CurrentPlayListIndex == 0) return;
                var track = PlayList[--CurrentPlayListIndex];
                SetCurrentSongByIndexAndAlbum(track.Item1, track.Item2);
            }
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
        public static readonly DirectoryInfo MainDirectory = new DirectoryInfo(@"C:\MyPlayerDirectory");
        DirectoryInfo curreDirectory = MainDirectory;
        public int CurrentIndex { get; private set; }
        public List<File> Songs;
        public List<Album> Albums;
        private File currentSong;
        public readonly List<Tuple<int, Album>> PlayList = new List<Tuple<int, Album>>();

        public MyPlayer(MainWindow window)
        {
            
            CurrentPlayer.MediaEnded += (s, a) =>
            {
                window.Bar.PauseStart();
                Next();
                window.Bar.PauseStart();
            };
            this.window = window;
            OpenCurrentDirectory();
        }

        private void OpenCurrentDirectory()
        {
            //File.AddFileTypeResolver(new File.FileTypeResolver());
            var files = curreDirectory.GetFiles()
                .Select(f =>
                {
                    try
                    {
                        var dim = f.Name.Substring(f.Name.Length - 3);
                        switch (dim)
                        {
                            case "mp3":
                            case "waw":
                            case "wma":
                                return File.Create(Path.Combine(f.DirectoryName, f.Name));
                            default: return null;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(f => f != null);
            Songs = new List<File>(files);
            Albums = new List<Album>(files.GroupBy(f => f.Tag.Album)
                .Select(g => new Album(g, g.Key, String.Join(", ", g.First().Tag.Performers), (g.FirstOrDefault(a => a.Tag.Pictures.Length > 0) ?? g.First()).Tag.Pictures, window)));
            //if (curreDirectory.GetFiles().Length <= 0) return;
            if (Songs.Count > 0)
            {
                CurrentAlbum = Albums.Last();
                CurrentIndex = CurrentAlbum.Songs.Count - 1;
                CurrentSong = CurrentAlbum.Songs[CurrentIndex];
                //var ind = 0;
                //PlayList = Albums[0].Songs.Select(s => Tuple.Create(ind++, Albums[0])).ToList();
            }
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
