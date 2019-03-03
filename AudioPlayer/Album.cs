using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TagLib;
using System.IO;

namespace AudioPlayer
{
    class Album
    {
        public readonly List<TagLib.File> Songs;// = new List<TagLib.File>();
        public readonly AlbumIcon Cover;//хз где брать обложку, просто чтобы не забыть, что она должна здесь быть
        public readonly string Name;
        public readonly string Author;

        public Album(IEnumerable<TagLib.File> songs, string name, string author)
        {
            Songs = new List<TagLib.File>(songs);
            Name = name;
            Author = author;
        }

    }
}
