using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Path = System.IO.Path;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        //private bool? result = null;
        private readonly DirectoryInfo dir;
        private IEnumerable<User> users;
        public AutorizationWindow()
        {
            InitializeComponent();
            dir = new DirectoryInfo(Path.Combine(MyPlayer.MainDirectory.FullName, "Users"));
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    ////base.OnClosed(e);
        //    //DialogResult = result;
        //}

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    DialogResult = null;
        //}

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(dir.FullName))
            {
                Directory.CreateDirectory(dir.FullName);
            }
            users = dir.GetFiles().Select(f => Open(f));//.ToArray();//dir.GetFiles().Select(f => Open(f));
            var user = users.FirstOrDefault(s => s.Login == Login.Text && s.PasswordHash == User.Encrypt(Password.Password));
            if (user == null)
            {
                MessageBox.Show("Fuck!");
                return;
            }
            DialogResult = user.IsExtended; //if (user)
            this.Close();
        }


        private User Open(FileInfo f)
        {
            var fileStream = File.Open(f.FullName, FileMode.Open);
            var binaryFormatter = new BinaryFormatter();
            var user = binaryFormatter.Deserialize(fileStream) as User;
            fileStream.Close();
            return user;
        }

        private void RegisterButtonOnClick(object sender, RoutedEventArgs e)
        {
            var r = new RegisterWindow().ShowDialog();
            
        }
    }
}
