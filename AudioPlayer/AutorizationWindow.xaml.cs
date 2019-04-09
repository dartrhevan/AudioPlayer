using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Path = System.IO.Path;
using System.ComponentModel;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        private bool? result = null;
        private readonly DirectoryInfo dir;
        private IEnumerable<User> users;
        public AutorizationWindow()
        {
            InitializeComponent();
            dir = new DirectoryInfo(Path.Combine(MyPlayer.MainDirectory.FullName, "Users"));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DialogResult = result;
            if(result == null)
                App.Current.Shutdown();
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    ////base.OnClosed(e);
        //    DialogResult = result;
        //    return;
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

            var curHash = User.Encrypt(Password.Password);
            var user = users.FirstOrDefault(s => 
                s.Login == Login.Text && ArrayEquals(s.PasswordHash, curHash));
            if (user == null)
            {
                //MessageBox.Show("Fuck!");
                ErrorLabel.Content = "*This login or password are irregular";
                return;
            }
            result = user.IsExtended; //if (user)
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

        static bool ArrayEquals(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for (var i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i]) return false;
            }

            return true;
        }
    }
}
