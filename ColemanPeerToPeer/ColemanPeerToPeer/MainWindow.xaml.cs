﻿using System;

using System.Windows;
using ColemanPeerToPeer.MVVM.ViewModel;
using ColemanPeerToPeer.MVVM.Model;
using System.Windows.Input;


namespace ColemanPeerToPeer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Btn_Maxmize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                App.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void Btn_SendMessage(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("click");
            MainViewModel ad = new MainViewModel();
            MessageModel aa = new MessageModel();
            aa.Message = "sdsdsd";
            ad.Messages.Add(aa);
        }
    }
}
