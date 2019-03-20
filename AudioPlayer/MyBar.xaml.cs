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

        readonly ImageSourceConverter ic = new ImageSourceConverter();
        public  MyPlayer Player { get; set; }
        public MyBar()
        {
            //Player.CurrentSong.Tag.
            InitializeComponent();
            timer.Tick += (sender, args) =>
            {
                if (playing && Player.CurrentPlayer.NaturalDuration.HasTimeSpan)
                    Progress.Value = Player.CurrentPlayer.Position.TotalSeconds /
                                     Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                Begin.Content = Player.CurrentPlayer.Position.ToString();
                End.Content = Player.CurrentPlayer.NaturalDuration.ToString();
                if (CurrentName.Content as string != Player.CurrentSong.Tag.Title)
                {
                    if (Player.CurrentSong.Tag.Pictures.Length > 0)
                        CurrentCover.Source = GetImage();
                    CurrentName.Content = Player.CurrentSong.Tag.Title;
                }

                InvalidateVisual();
            };
            timer.Start();
        }

        private Timer timer = new Timer {Interval = 100};

        bool playing;

        private void PlayStartButtonClick(object sender, RoutedEventArgs e)
        {
            PlayStart();
        }

        public void PlayStart()
        {
            CurrentName.Content = Player.CurrentSong.Tag.Title;//.Name.Split('\\').Last();
            if (Player.CurrentSong.Tag.Pictures.Length > 0)
                CurrentCover.Source = ((ImageSource)ic.ConvertFrom(Player.CurrentSong.Tag.Pictures[0].Data.Data));
            if (!playing)
            {
                Player.CurrentPlayer.Play();
                //End.Content = Player.CurrentPlayer.NaturalDuration.ToString();

            }
            else Player.CurrentPlayer.Pause();
            playing = !playing;
        }

        private void OpenSongClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var res = openFileDialog.ShowDialog();
            if (!(res == DialogResult.Yes || res == DialogResult.OK && openFileDialog.FileName != "")) return;
            Player.AddSong(openFileDialog.FileName);
            var albums = GetMainPage();
            albums.Update();
            CurrentName.Content = Player.CurrentSong.Name.Split('\\').Last();
            if (Player.CurrentSong.Tag.Pictures.Length > 0)
                CurrentCover.Source = GetImage();
            InvalidateVisual();
            playing = false;
        }

        private ImageSource GetImage() => ((ImageSource)ic.ConvertFrom(Player.CurrentSong.Tag.Pictures[0].Data.Data));

        private MainPage GetMainPage()
        {
            var par = (Parent as DockPanel).Parent as MainWindow;
            var albums = par.MainPage; //par.Children[par.Children.Count - 1] as MainPage;
            return albums;
        }

        private void OpenDirectoryClick(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new FolderBrowserDialog();
            var res = openFolderDialog.ShowDialog();
            if (!(res == DialogResult.Yes || res == DialogResult.OK && openFolderDialog.SelectedPath != "")) return;
                Player.OpenFolder(openFolderDialog.SelectedPath);
            //var par = (Parent as DockPanel).Parent as MainWindow;
            var albums = GetMainPage();//par.Children[par.Children.Count - 1] as MainPage;
            albums.Reset();/*
            CurrentName.Content = Player.CurrentSong.Name.Split('\\').Last();
            if(Player.CurrentSong.Tag.Pictures.Length > 0)
                CurrentCover.Source = ((ImageSource)ic.ConvertFrom(Player.CurrentSong.Tag.Pictures[0].Data.Data));*/
            //End.Content = Player.CurrentPlayer.NaturalDuration.ToString();
            InvalidateVisual();
            playing = false;
        }

        private void ProgressValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (Player.CurrentPlayer != null && Player.CurrentPlayer.NaturalDuration.HasTimeSpan)
            {
                var dif = Math.Abs(e.NewValue - e.OldValue);
                //var stDif = (e.NewValue - e.OldValue) * Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                if (dif > 120 / Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalMilliseconds)
                {
                    /*Player.CurrentPlayer.Pause();
                    playing = false;*/
                    Player.CurrentPlayer.Position =
                        new TimeSpan((long) (Player.CurrentPlayer.NaturalDuration.TimeSpan.Ticks * e.NewValue));
                }
            }
        }
    }
}
