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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public readonly MyPlayer Player;
        //readonly List<Album> Albums;

        public MainPage(MyPlayer player)
        {
            Player = player;
            InitializeComponent();
            //Albums = Player.Albums;
            foreach (var album in Player.Albums)
                AlbumsPanel.Children.Add(album);
        }

        public void Update()
        {
            if (AlbumsPanel.Children.Count < Player.Albums.Count)
                for(var i = AlbumsPanel.Children.Count; i < Player.Albums.Count; ++i)
                    AlbumsPanel.Children.Add(Player.Albums[i]);
        }
    }
}
