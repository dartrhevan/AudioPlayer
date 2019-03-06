using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MyBar.xaml
    /// </summary>
    public partial class MyBar : UserControl
    {
        public  MyPlayer Player { get; set; }
        public MyBar()
        {
            InitializeComponent();
        }


        bool playing;

        private void PlayStartButtonClick(object sender, RoutedEventArgs e)
        {
            if(!playing)
                Player.CurrePlayer.Play();
            else Player.CurrePlayer.Pause();
            playing = !playing;
        }

        private void OpenSongClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var res = openFileDialog.ShowDialog();
            if (res == DialogResult.Yes || res == DialogResult.OK)
                Player.AddSong(openFileDialog.FileName);
            var par = Parent as DockPanel;
            var albums = par.Children[par.Children.Count - 1] as MainPage;
            albums.Update();
            InvalidateVisual();
            playing = false;
        }

        private void OpenDirectoryClick(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new FolderBrowserDialog();
            var res = openFolderDialog.ShowDialog();
            if (res == DialogResult.Yes || res  == DialogResult.OK)
                Player.OpenFolder(openFolderDialog.SelectedPath);
            var par = Parent as DockPanel;
            var albums = par.Children[par.Children.Count - 1] as MainPage;
            albums.Reset();
            InvalidateVisual();
            playing = false;
        }
    }
}
