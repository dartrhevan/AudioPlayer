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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly MyPlayer Player;
        public readonly MainPage MainPage;

        public AlbumPage Album { get; set; }

        public MainWindow()
        {
            var r = (new AutorizationWindow()).ShowDialog();
            MessageBox.Show(r.ToString());
            Player = new MyPlayer(this);
            InitializeComponent();
            MainPage = new MainPage(Player);
            Bar.Player = Player;
            Panel.Children.Add(MainPage);
        }
    }
}
