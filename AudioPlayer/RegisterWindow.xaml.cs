using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Path = System.IO.Path;
using System.Security.Cryptography;
using System.Text;

namespace AudioPlayer
{

    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly DirectoryInfo dir;

        public RegisterWindow()
        {
            InitializeComponent();
            dir = new DirectoryInfo(Path.Combine(MyPlayer.MainDirectory.FullName, "Users"));
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {

            var user = new User(Login.Text, User.Encrypt(Password.Password), License.Password == "159");
            Save(user);
            this.Close();
            //this.DialogResult = License.Password != "";
        }

        public void Save(User user)
        {
            var fileStream = File.Open(Path.Combine(MyPlayer.MainDirectory.FullName, "Users", user.Login), FileMode.Create);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, user);
            fileStream.Close();
        }
    }
}
