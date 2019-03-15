﻿using System;
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
            //Player.CurrentSong.Tag.
            InitializeComponent();
            timer.Tick += (sender, args) =>
            {
                if (playing && Player.CurrentPlayer.NaturalDuration.HasTimeSpan)
                    Progress.Value = Player.CurrentPlayer.Position.TotalSeconds /
                                     Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalSeconds * 100;
                InvalidateVisual();
            };
            timer.Start();
        }

        private Timer timer = new Timer {Interval = 100};

        bool playing;

        private void PlayStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (!playing)
            {
                Player.CurrentPlayer.Play();
            }
            else Player.CurrentPlayer.Pause();
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
            CurrentName.Content = Player.CurrentSong.Name.Split('\\').Last();
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
            CurrentName.Content = Player.CurrentSong.Name.Split('\\').Last();
            InvalidateVisual();
            playing = false;
        }

        private void ProgressValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {/*
            if (Player.CurrentPlayer != null)
            {
                Player.CurrentPlayer.Pause();
                playing = false;
                Player.CurrentPlayer.Position =
                    new TimeSpan((long) (Player.CurrentPlayer.NaturalDuration.TimeSpan.Ticks * e.NewValue / 100));
            }*/
        }
    }
}
