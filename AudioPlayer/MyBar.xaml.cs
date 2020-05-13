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
using AudioPlayer.Models;
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

        public MyPlayer Player { get; set; }
        //public bool Playing => playing;

        public MyBar()
        {
            InitializeComponent();
            UserLabel.Text = "Currnet user:\n" + MyPlayer.CurrentUser.Login;
            timer.Tick += (sender, args) =>
            {
                if (Player.CurrentSong == null) return;
                if (Player.Playing && Player.CurrentPlayer.NaturalDuration.HasTimeSpan)
                    Progress.Value = Player.CurrentPlayer.Position.TotalSeconds /
                                     Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                //Условие для мелодий длительностью больше часа
                if (Player.CurrentPlayer.NaturalDuration.HasTimeSpan && Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalHours > 0)
                {
                    Begin.Content = String.Format("{0:00}:{1:00}:{2:00}", Player.CurrentPlayer.Position.Hours, Player.CurrentPlayer.Position.Minutes, Player.CurrentPlayer.Position.Seconds);
                    End.Content = String.Format("{0:00}:{1:00}:{2:00}", Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalHours, Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalMinutes, Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalSeconds);
                    //if (CurrentName.Content as string != Player.CurrentSong.Tag.Title)
                    //{
                }
                else
                {
                    if (Player.CurrentPlayer.NaturalDuration.HasTimeSpan)
                    {
                        Begin.Content = String.Format("{0:00}:{1:00}", Player.CurrentPlayer.Position.Minutes, Player.CurrentPlayer.Position.Seconds);
                        End.Content = String.Format("{0:00}:{1:00}", Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalMinutes, Player.CurrentPlayer.NaturalDuration.TimeSpan.Seconds);
                    }
                }
                if (Player.CurrentSong.Tag.Pictures.Length > 0)
                        CurrentCover.Source = GetImage();
                CurrentName.Content = Player.CurrentSong.Tag.Title?? Player.CurrentSong.Name.Split('\\').Last();
                //}

                InvalidateVisual();
            };
            timer.Start();
        }

        private Timer timer = new Timer {Interval = 100};


        private void PlayStartButtonClick(object sender, RoutedEventArgs e)
        {
            PauseStart();
        }

        public void PauseStart()
        {                

            CurrentName.Content = Player.CurrentSong.Tag.Title;


            if (Player.CurrentSong.Tag.Pictures.Length > 0)
                CurrentCover.Source = ((ImageSource)ic.ConvertFrom(Player.CurrentSong.Tag.Pictures[0].Data.Data));
            if (!Player.Playing)
                Player.CurrentPlayer.Play();
            else Player.CurrentPlayer.Pause();
            Player.Playing = !Player.Playing;
        }

        private void OpenSongClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var res = openFileDialog.ShowDialog();
            if (!(res == DialogResult.Yes || res == DialogResult.OK && openFileDialog.FileName != "")) return;
            Player.AddSong(openFileDialog.FileName);
            var albums = GetMainPage();
            albums.Update();

            CurrentName.Content = Player.CurrentSong.Tag.Title ?? Player.CurrentSong.Name.Split('\\').Last(); if (Player.CurrentSong.Tag.Pictures.Length > 0)

            CurrentCover.Source = GetImage();
            InvalidateVisual();
            Player.Playing = false;
        }

        private ImageSource GetImage() => ((ImageSource)ic.ConvertFrom(Player.CurrentSong.Tag.Pictures[0].Data.Data));

        private IMainPage GetMainPage()
        {
            var par = (Parent as DockPanel).Parent as MainWindow;
            var albums = par.RealMainPage; 
            return albums;
        }

        private void OpenDirectoryClick(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new FolderBrowserDialog();
            var res = openFolderDialog.ShowDialog();
            if (!(res == DialogResult.Yes || res == DialogResult.OK && openFolderDialog.SelectedPath != "")) return;
                Player.OpenFolder(openFolderDialog.SelectedPath);
            var albums = GetMainPage();
            albums.Reset();
            InvalidateVisual();
            Player.Playing = false;
        }

        private void ProgressValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (Player.CurrentPlayer != null && Player.CurrentPlayer.NaturalDuration.HasTimeSpan)
            {
                var dif = Math.Abs(e.NewValue - e.OldValue);
                if (dif > 120 / Player.CurrentPlayer.NaturalDuration.TimeSpan.TotalMilliseconds)
                {
                    Player.CurrentPlayer.Position =
                        new TimeSpan((long) (Player.CurrentPlayer.NaturalDuration.TimeSpan.Ticks * e.NewValue));
                }
            }
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Player.Playing)
                PauseStart();
            Player.Previous();
            PauseStart();
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if(!Player.Playing)
                PauseStart();
            Player.Next();
            PauseStart();
        }
    }
}
