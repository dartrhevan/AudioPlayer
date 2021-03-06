﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TagLib;
using System.Windows.Media.Animation;

namespace AudioPlayer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Album : UserControl
    {
        public readonly List<TagLib.File> Songs;
        //public readonly ImageSource Cover;
        MainWindow window => Application.Current.MainWindow as MainWindow;

        public Album(IEnumerable<TagLib.File> songs, string albumName, string author, IPicture[] pictures)
        {
            //this.Window = Window;
            InitializeComponent();
            ImageSourceConverter ic = new ImageSourceConverter();

            if (pictures != null && pictures.Length > 0)
            {
                //Cover = (ImageSource)ic.ConvertFrom(pictures[0].Data.Data);
                var bg = new ImageBrush((ImageSource)ic.ConvertFrom(pictures[0].Data.Data));
                bg.Stretch = Stretch.Fill;
                Picture.Background = bg;
            }
            Songs = new List<TagLib.File>(songs);
            AlbumName.Content = albumName;
            Author.Content = author;
            var style = Resources["Style"] as Style;
            //style.Setters[0]
            //style.Triggers[0].EnterActions.Add(new SetterBase());
        }

        private void PictureClick(object sender, RoutedEventArgs e)
        {
            var mainPanel = window.Panel;//((((Parent as WrapPanel).Parent as ScrollViewer).Parent as Grid).Parent as MainPage).Parent as DockPanel;
            mainPanel.Children.RemoveAt(mainPanel.Children.Count - 1);
            //var wind = (MainWindow) mainPanel.Parent;
            window.Album = new AlbumPage();
            window.Album.Album = this;
            //wind.Album.Player = wind.Player;
            mainPanel.Children.Add(window.Album);
            ExecuteAlbumAnimation(window);
        }
        private static void ExecuteAlbumAnimation(MainWindow mainWindow)
        {
            var anim = new ThicknessAnimation
            {
                From = new Thickness(mainWindow.Width, 0, 0, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 250))
            };
            mainWindow.Album.BeginAnimation(MarginProperty, anim);
        }

    }
}
