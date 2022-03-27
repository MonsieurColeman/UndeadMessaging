/*
 This file hands the GUI logic of the Login View

 This file interactions with the mainView's ctor to determine login success
 */


using ColemanPeerToPeer.Service;
using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static string _url = "http://localhost:8080/BasicService";

        public LoginView(MainWindow main)
        {
            InitializeComponent();
            _MainWindow = main;
            Client.StartClientBehavior();
        }

        private void Btn_Login(object sender, RoutedEventArgs e)
        //Login behavior to the system, - activated by a button behavior
        {
            string proposedUsername = usernameTextbox.Text;

            //make sure textboxes have values
            if (String.IsNullOrWhiteSpace(userColorTextbox.Text) || String.IsNullOrWhiteSpace(proposedUsername))
                return;

            //truncate username if too long
            if(proposedUsername.Length > 15)
                proposedUsername = proposedUsername.Substring(0, 15);

            //If string doesnt match the requirements, tell the user
            if (!Regex.IsMatch(proposedUsername, @"^[A-Za-z0-9_-]*$"))
            {
                MessageBox.Show(GlobalStrings.inputValidation_Login);
                return;
            }

            //Make call to server
            if (!PerformLogin(proposedUsername, userColorTextbox.Text))
            {
                NotifyUserNameWasTaken();
                return;
            }

            //Make client obj known of username
            Client._username = proposedUsername;

            //Set the Dashboard Username
            ViewManager.GetMainViewModelInstance().Username = proposedUsername;

            //Show Dashboard
            _MainWindow.loginSuccessful = true;
            this.Close();
        }

        private void NotifyUserNameWasTaken()
        //displays a warning popup
        {
            MessageBox.Show(GlobalStrings.popup_duplicateUsername);
        }


        private bool PerformLogin(string username, string usernameColor)
        //interfaces with the servers service and returns a bool representing
        //the success of the call
        {
            return Client.JoinServer(username, usernameColor);
        }
    }
}

/*
 Maintenance History

0.7 Added login functionality
0.8 Added cooperation with main view
0.9 Added input validation
1.0 Refactoring and commenting
 */
