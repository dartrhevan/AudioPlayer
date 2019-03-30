using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Path = System.IO.Path;

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
            var user = new User(Login.Text, Password.Password.GetHashCode(), License.Password == "159");
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
