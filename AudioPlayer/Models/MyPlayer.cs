using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Application = System.Windows.Application;
using File = TagLib.File;

namespace AudioPlayer.Models
{
    public class MyPlayer
    {
        public int CurrentPlayListIndex { get; set; } = 0;
        public MainWindow Window => Application.Current.MainWindow as MainWindow;
        
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
        public readonly DirectoryInfo MainDirectory;// = new DirectoryInfo(@"C:\MyPlayerDirectory");
        private DirectoryInfo currentDirectory;// = MainDirectory;
        public int CurrentIndex { get; private set; }
        public List<File> Songs;
        public Dictionary<string, Album> Albums;
        private File currentSong;
        public readonly List<Tuple<int, Album>> PlayList;// = new List<Tuple<int, Album>>();

        public MyPlayer(User currentUser)
        {
            MainDirectory = new DirectoryInfo(currentUser.MainDirectory??@"C:\MyPlayerDirectory");
            CurrentUser = currentUser;
            currentDirectory = MainDirectory;
            CurrentPlayer.MediaEnded += (s, a) =>
            {
                Window.Bar.PauseStart();
                Next();
                Window.Bar.PauseStart();
            };
            //this.Window = Window;
            OpenCurrentDirectory();
            PlayList = currentUser.PlayList?.Select(t => Tuple.Create(t.Item1, Albums[t.Item2]))?.ToList() ?? new List<Tuple<int, Album>>();
        }

        private static Dictionary<string, Album> GetAlbums(List<File> files) => files.GroupBy(f => f.Tag.Album)
            .ToDictionary(g => g.Key??"", g => new Album(g, g.Key??"", String.Join(", ", g.First().Tag.Performers),
                (g.FirstOrDefault(a => a.Tag.Pictures.Length > 0) ?? g.First()).Tag.Pictures));
        /*
            .Select(g => new Album(g, g.Key, String.Join(", ", g.First().Tag.Performers), 
                (g.FirstOrDefault(a => a.Tag.Pictures.Length > 0) ?? g.First()).Tag.Pictures))
            .ToList();
            */
        private void OpenCurrentDirectory()
        {
            //File.AddFileTypeResolver(new File.FileTypeResolver());
            var func = new Func<IEnumerable<File>>(GetFiles);
            var res = func.BeginInvoke(null, null);
            var files = func.EndInvoke(res);
            Songs = new List<File>(files);
            Albums = GetAlbums(Songs);
            if (Songs.Count > 0)
            {
                CurrentAlbum = Albums.Last().Value;
                CurrentIndex = CurrentAlbum.Songs.Count - 1;
                CurrentSong = CurrentAlbum.Songs[CurrentIndex];
            }
            GC.Collect();
        }


        private IEnumerable<File> GetFiles()
        {
            return currentDirectory.GetFiles()
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
                })
                .Where(f => f != null);
        }

        public void OpenFolder(string folderPath)
        {
            currentDirectory = new DirectoryInfo(folderPath);
            OpenCurrentDirectory();
        }

        public void AddSong(string path)
        {
            var song = File.Create(path);
            Songs.Add(song);
            var album = new Album(new[] { song }, song.Tag.Album, String.Join(", ", song.Tag.Performers), song.Tag.Pictures);
            var existedAlbum = Albums.FirstOrDefault(a => a.Value.AlbumName.Content as string == album.AlbumName.Content as string && a.Value.Author.Content as string == album.Author.Content as string);
            if (existedAlbum.Value == null)
            {
                Albums[album.Name] = album;
                CurrentIndex = -1;//albInd;
            }
            else
            {
                CurrentIndex = Albums.Count;
                existedAlbum.Value.Songs.Add(song);
            }
            CurrentAlbum = album;
            CurrentSong = song;

        }

        public readonly User CurrentUser;
    }
}
