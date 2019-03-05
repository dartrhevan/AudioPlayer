using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TagLib;
using System.IO;
using File = TagLib.File;

namespace AudioPlayer
{
    public class MyPlayer
    {
        public Album CurrAlbum { get; set; }
        public TagLib.File CurrentSong { get; set; }
        public readonly MediaPlayer CurrePlayer = new MediaPlayer();// { get; private set; }
        private static readonly DirectoryInfo MainDirectory = new DirectoryInfo(@"C:\MyPlayerDirectory");
        public readonly List<TagLib.File> Songs;// = new List<TagLib.File>();
        public readonly List<Album> Albums;// { get; set; }

        public MyPlayer()
        {
            var files = MainDirectory.GetFiles()
                .Select(f => TagLib.File.Create(Path.Combine(f.DirectoryName, f.Name)));
            Songs = new List<TagLib.File>(files);
            Albums = new List<Album>(files.GroupBy(f => f.Tag.Album)
                .Select(g => new Album(g, g.Key, String.Join(", ", g.First().Tag.Performers), g.First().Tag.Pictures)));
        }
        /*
        void InitialiseSongsList().
        {
            var files = MainDirectory.GetFiles()
                .Select(f => TagLib.File.Create(Path.Combine(f.DirectoryName, f.Name)));

        }*/
        public void AddSong(string path)
        {
            var song = TagLib.File.Create(path);
            Songs.Add(song);
            var album = new Album(new[] {song}, song.Tag.Album, String.Join(", ", song.Tag.Performers), song.Tag.Pictures);
            var alb_ind = Albums.FindIndex(a => a.AlbumName.Content as string == album.AlbumName.Content as string && a.Author.Content as string  == album.Author.Content as string);
            if (alb_ind == -1)
                Albums.Add(album);
            else Albums[alb_ind].Songs.Add(song);
            CurrAlbum = album;
            CurrentSong = song;
            CurrePlayer.Close();
            CurrePlayer.Open(new Uri(path, UriKind.Relative));
        }
    }
}
