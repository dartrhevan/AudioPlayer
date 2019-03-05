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
using TagLib;
using System.IO;
using System.Drawing;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Album : UserControl
    {
        ImageSourceConverter ic = new ImageSourceConverter();
        public readonly List<TagLib.File> Songs;// = new List<TagLib.File>();
        public readonly ImageSource Cover;//хз где брать обложку, просто чтобы не забыть, что она должна здесь быть
        //public readonly string AlbumName;
        //public readonly string Author;

            //RenderTransformOrigin="0.519,0.685" не знаю что это, но я этого в гриде не писал


        public Album(IEnumerable<TagLib.File> songs, string albumName, string author, IPicture[] pictures)
        {
            InitializeComponent();

            if (pictures != null && pictures.Length > 0)
            {
                Cover = (ImageSource)ic.ConvertFrom(pictures[0].Data.Data);
                var bg = new ImageBrush(Cover);
                bg.Stretch = Stretch.Fill;
                Picture.Background = bg;
            }
            Songs = new List<TagLib.File>(songs);
            AlbumName.Content = albumName;
            Author.Content = author;
        }
    }
}
