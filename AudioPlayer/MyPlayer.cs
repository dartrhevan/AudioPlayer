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
        public MediaPlayer CurrePlayer { get; private set; }
        private static readonly DirectoryInfo MainDirectory = new DirectoryInfo(@"C:\MyPlayerDirectory");
        public readonly List<TagLib.File> Songs;// = new List<TagLib.File>();
        public List<Album> Albums { get; set; }

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

    }
}
