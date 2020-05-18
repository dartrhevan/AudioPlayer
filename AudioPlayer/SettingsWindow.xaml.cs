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
using System.Windows.Shapes;
using AudioPlayer.Models;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly User user;
        public SettingsWindow(User user)
        {
            this.user = user;
            InitializeComponent();
            LoginField.Text = user.Login;
            VolumeSLider.Value = user.Volume;
            SimpleView.IsEnabled = user.IsExtended;
            SimpleView.IsChecked = user.UseSimple;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            //LoginField.Text = user.Login;
            user.Volume = VolumeSLider.Value;
            //SimpleView.IsEnabled = user.IsExtended;
            user.IsSimple = SimpleView.IsChecked.Value;
            var fs = new FileAuthService();
            fs.Save(user);
            this.Close();
        }
    }
}
