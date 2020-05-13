using System.Collections.Generic;
using TagLib;

namespace AudioPlayer.Models
{
    public class AlbumM
    {
        private IEnumerable<TagLib.File> songs;
        private string albumName;
        private string author;
        private IPicture pictures;
    }
}