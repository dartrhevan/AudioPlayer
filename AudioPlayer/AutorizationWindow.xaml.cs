using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Path = System.IO.Path;
using System.ComponentModel;
using AudioPlayer.Models;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        private bool? result = null;
        readonly IAuthService authService = new FileAuthService();
        public AutorizationWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DialogResult = result;
            if(result == null)
                App.Current.Shutdown();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var user = authService.Authenticate(Login.Text, Password.Password);
            if (user != null)
            { 
                result = user.IsExtended; //if (user)
                this.Close();
            }
            else
                ErrorLabel.Content = "*This login or password are irregular";
        }

        private void RegisterButtonOnClick(object sender, RoutedEventArgs e)
        {
            var r = new RegisterWindow().ShowDialog();
        }
    }
}
