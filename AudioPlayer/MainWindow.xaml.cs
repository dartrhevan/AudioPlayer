﻿using System;
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
        public readonly MyPlayer Player = new MyPlayer();
        public readonly MainPage MainPage;// = new MainPage();
        public MainWindow()
        {
            InitializeComponent();
            MainPage = new MainPage(Player);
            Panel.Children.Add(MainPage);
        }
    }
}
