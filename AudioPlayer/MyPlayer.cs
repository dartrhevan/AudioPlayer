﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TagLib;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using File = TagLib.File;

namespace AudioPlayer
{
    public class MyPlayer
    {
        //public DoubleAnimation CurrentSongAnimation { get; private set; } = new DoubleAnimation();
        public Album CurrAlbum { get; set; }
        public TagLib.File CurrentSong
        {
            get => currentSong;
            set
            {
                currentSong = value;
                CurrentPlayer.Close();
                CurrentPlayer.Open(new Uri(currentSong.Name, UriKind.Relative));
                //CurrentSongAnimation.Duration = CurrentPlayer.NaturalDuration;
            }
        }

        public readonly MediaPlayer CurrentPlayer = new MediaPlayer();// { get; private set; }
        private static readonly DirectoryInfo MainDirectory = new DirectoryInfo(@"C:\MyPlayerDirectory");
        DirectoryInfo curreDirectory = MainDirectory;
        public List<File> Songs;// = new List<TagLib.File>();
        public List<Album> Albums;// { get; set; }
        private File currentSong;

        public MyPlayer()
        {
            OpenCurrentDirectory();
            //CurrentSongAnimation.From = 0;
            //CurrentSongAnimation.To = 100;
        }

        private void OpenCurrentDirectory()
        {
            var files = curreDirectory.GetFiles()
                .Select(f =>
                {
                    try
                    {
                        return File.Create(Path.Combine(f.DirectoryName, f.Name));
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(f => f != null);
            Songs = new List<File>(files);
            Albums = new List<Album>(files.GroupBy(f => f.Tag.Album)
                .Select(g => new Album(g, g.Key, String.Join(", ", g.First().Tag.Performers), (g.FirstOrDefault(a => a.Tag.Pictures.Length > 0) ?? g.First()).Tag.Pictures)));
            CurrentSong = Songs[Songs.Count - 1];
        }

        public void OpenFolder(string folderPath)
        {
            curreDirectory = new DirectoryInfo(folderPath);
            OpenCurrentDirectory();
        }

        /*
        void InitialiseSongsList().
        {
            var files = MainDirectory.GetFiles()
                .Select(f => TagLib.File.Create(Path.Combine(f.DirectoryName, f.Name)));

        }*/
        public void AddSong(string path)
        {
            var song = File.Create(path);
            Songs.Add(song);
            var album = new Album(new[] { song }, song.Tag.Album, String.Join(", ", song.Tag.Performers), song.Tag.Pictures);
            var alb_ind = Albums.FindIndex(a => a.AlbumName.Content as string == album.AlbumName.Content as string && a.Author.Content as string == album.Author.Content as string);
            if (alb_ind == -1)
                Albums.Add(album);
            else Albums[alb_ind].Songs.Add(song);
            CurrAlbum = album;
            //CurrentSong.
            CurrentSong = song;
        }

    }
}
