using ColemanPeerToPeer.Service;
using ServiceOutliner;
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

namespace ColemanPeerToPeer
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        MainWindow _MainWindow;
        static string url = "http://localhost:8080/BasicService";
        Peer peer = new Peer(url);

        public LoginView(MainWindow main)
        {
            InitializeComponent();
            _MainWindow = main;
        }

        private void Btn_Login(object sender, RoutedEventArgs e)
        {
            //make sure textboxes have values
            if (String.IsNullOrWhiteSpace(userColorTextbox.Text) || String.IsNullOrWhiteSpace(usernameTextbox.Text))
                return;

            //Make call to server
            if (!PerformLogin(usernameTextbox.Text, userColorTextbox.Text))
            {
                NotifyUserNameWasTaken();
                return;
            }

            //Set the Dashboard Username
            ViewManager.GetMainViewModelInstance().Username = usernameTextbox.Text;

            //Show Dashboard
            _MainWindow.loginSuccessful = true;
            this.Close();
        }

        private void NotifyUserNameWasTaken()
        {
            MessageBox.Show("That name is in use!");
        }


        private bool PerformLogin(string username, string usernameColor)
        {
            return peer.JoinServer(username, usernameColor);
        }

        private MessageProtocol GetJoinMessage()
        {
            return new MessageProtocol()
            {
                sourceEndpoint = url,
                messageProtocolType = MessageType.join,
                messageBody = "thisIsATest",
                destinationEndpoint = "server"
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("oo");
        }
    }
}
