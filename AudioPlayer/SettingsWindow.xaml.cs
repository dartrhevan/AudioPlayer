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
using System.Windows.Shapes;
using AudioPlayer.Models;
using MessageBox = System.Windows.MessageBox;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly User user;
        private string password = null;
        readonly RemoteAuthService service;
        public SettingsWindow(User user, RemoteAuthService service)
        {
            this.user = user;
            this.service = service;
            InitializeComponent();
            LoginField.Text = user.Login;
            VolumeSLider.Value = user.Volume;
            SimpleView.IsEnabled = user.IsExtended;
            SimpleView.IsChecked = user.UseSimple;
            if(!string.IsNullOrEmpty(user.MainDirectory))
                MainDir.Text = user.MainDirectory;
        }

        private async void AcceptClick(object sender, RoutedEventArgs e)
        {
            user.Login = LoginField.Text;
            user.Volume = VolumeSLider.Value;
            user.MainDirectory = MainDir.Text;
            //SimpleView.IsEnabled = user.IsExtended;
            user.IsSimple = SimpleView.IsChecked.Value;
            if (Expander.IsExpanded && RemoteAuthService.PasswordCheck(CurrentPassword.Password, user.PasswordHash)
                                    && NewPassword.Password == NewPasswordConfirm.Password)
                password = NewPassword.Password;
            var result = await service.Edit(user, password);
            if (string.IsNullOrEmpty(result))
                /*var fs = new Rem();
                fs.Save(user);*/
                this.Close();
            else
                MessageBox.Show(result);
        }

        private void ChangeDirectoryClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                MainDir.Text = dialog.SelectedPath;
        }
    }
}
