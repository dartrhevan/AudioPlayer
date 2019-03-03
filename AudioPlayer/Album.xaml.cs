using System;
using System.Collections.Generic;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Album : UserControl
    {
        public readonly List<TagLib.File> Songs;// = new List<TagLib.File>();
        public readonly Album Cover;//хз где брать обложку, просто чтобы не забыть, что она должна здесь быть
        //public readonly string AlbumName;
        //public readonly string Author;


        public Album(IEnumerable<TagLib.File> songs, string albumName, string author)
        {
            InitializeComponent();
            Songs = new List<TagLib.File>(songs);
            AlbumName.Content = albumName;
            Author.Content = author;
        }
    }
}
