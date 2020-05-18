using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Path = System.IO.Path;
using System.Security.Cryptography;
using System.Text;
using AudioPlayer.Models;

namespace AudioPlayer
{

    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        readonly IAuthService authService = new FileAuthService();
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var user = new User(Login.Text, User.Encrypt(Password.Password), License.Password == "159");
            authService.Save(user, MyPlayer.DefaultMainDirecrory);
            this.Close();
            //this.DialogResult = License.Password != "";
        }
        
    }
}
