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
using AudioPlayer.Models;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl, IMainPage
    {
        public readonly MyPlayer Player;
        //readonly List<Album> Albums;

        public MainPage(MyPlayer player)
        {
            Player = player;
            InitializeComponent();
            //Albums = Player.Albums;
            foreach (var album in Player.Albums)
                AlbumsPanel.Children.Add(album.Value);
        }

        public void Update()
        {
            var albs = new List<Album>();
            if (AlbumsPanel.Children.Count < Player.Albums.Count)
                foreach (Album album in AlbumsPanel.Children)
                    if(Player.Albums.ContainsKey(album.AlbumName.Content as string))
                        albs.Add(album);
            foreach (var album in albs)
                AlbumsPanel.Children.Add(album);
            /*
                for(var i = AlbumsPanel.Children.Count; i < Player.Albums.Count; ++i)
                    AlbumsPanel.Children.Add(Player.Albums[i]);*/
        }

        public void Reset()
        {
            AlbumsPanel.Children.Clear();
            foreach (var album in Player.Albums)
                AlbumsPanel.Children.Add(album.Value);
        }
    }
}
