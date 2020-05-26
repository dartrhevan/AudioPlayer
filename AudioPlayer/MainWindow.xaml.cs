using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly IAuthService authService = new RemoteAuthService();
        public readonly MyPlayer Player;
        public readonly MainPage MainPage;
        private readonly RowAlbumPage RowAlbumPage;
        public readonly IMainPage RealMainPage;// { get; set; }
        public AlbumPage Album { get; set; }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            authService.Logout();
        }

        public MainWindow()
        {
            var authDialog = new AutorizationWindow(authService);
            var r = authDialog.ShowDialog();
            //MessageBox.Show(r.ToString());
            Player = new MyPlayer(authDialog.User);
            InitializeComponent();
            Background = Brushes.DarkSlateGray;
            Bar.Player = Player;
            if(r == null) Close();
            else if (!authDialog.User.UseSimple)
            {
                MainPage = new MainPage(Player);
                RealMainPage = MainPage;
                Panel.Children.Add(MainPage);
            }
            else
            {
                RowAlbumPage = new RowAlbumPage();
                RealMainPage = RowAlbumPage;
                Panel.Children.Add(RowAlbumPage);
            }
        }
    }
}
