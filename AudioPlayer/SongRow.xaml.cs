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
    /// Строка одной песни в списке
    /// </summary>
    public partial class SongRow : UserControl
    {
        public SongRow(TagLib.File song)
        {
            InitializeComponent();
            //Content = song.Tag.Title;
            Label.Content = song.Tag.Title;
            Margin = new Thickness(0, 0, 0, 12);
            Height = 20;
        }
    }
}
