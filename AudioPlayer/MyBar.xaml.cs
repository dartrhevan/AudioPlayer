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
using Microsoft.Win32;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MyBar.xaml
    /// </summary>
    public partial class MyBar : UserControl
    {
        public readonly MyPlayer Player;
        public MyBar(MyPlayer player)
        {
            InitializeComponent();
            Player = player;
        }/*

        private void PlayStartButtonClick(object sender, RoutedEventArgs e)
        {

        }*/
        private void PlayStartButtonClick(object sender, RoutedEventArgs e)
        {
            if(Player.CurrePlayer.Position.Equals(new TimeSpan(0)))
                Player.CurrePlayer.Play();
            else Player.CurrePlayer.Pause();
        }

        private void OpenSongClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Player.AddSong(openFileDialog.FileName);
        }
    }
}
