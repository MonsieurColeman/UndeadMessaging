using ColemanPeerToPeer.Core;
using ColemanPeerToPeer.TestScripts;
using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ColemanPeerToPeer.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        //current view messages
        public  ObservableCollection<MessageModel> Messages { get; set; }
        //current view users

        private ObservableCollection<UserModel> _users;

        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set { _users = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserModel> Users2 { get; set; }
        //stuff
        private string _message;
        //otherstuff
        public string userNameColor;

        private string _username;
        public string _profilePicture = "https://picsum.photos/200/300";


        //public interface for username
        public string Username
        {
            get { return _username; }
            set { _username = value;
                OnPropertyChanged();
            }
        }



        /* Commands */
        public RelayCommand SendCommand { get; set; }

        private UserModel _selectedChat;

        public UserModel SelectedChat
        {
            get { return _selectedChat; }
            set {
                _selectedChat = value; 
                OnPropertyChanged();
            }
        }
        
        public string Message
        {
            get { return _message; }
            set { 
                _message = value; 
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            //pass to viewManager
            ViewManager.SetMainViewModelInstance(this);
            //create a new list for this view
            Messages = new ObservableCollection<MessageModel>();
            //create a new lsit for this view
            Users = new ObservableCollection<UserModel>();

            userNameColor = "#000000";

            //populate the user list with a random number of users
            int users = new Random().Next(0, 100);

            
            SendCommand = new RelayCommand(SendMessage);
            
            
            
            //Test objects
            //SetUsers(UserMessageTester.GetUsers());
            //GainUser(UserMessageTester.GetExampleTopic());

        }

        public void SendMessage(object o)
        {
            if (SelectedChat == null || String.IsNullOrWhiteSpace(Message))
                return;
            DisplayMessageToView();

            Message = ""; //Clears textbox
        }

        private void DisplayMessageToView()
        {
            SelectedChat.Messages.Add(new MessageModel
            {
                Username = Username,
                UsernameColor = userNameColor,
                ImageSource = _profilePicture,
                Message = Message,
                Time = DateTime.Now,
                IsFromMe = true,
                FirstMessage = true
            });
        }

        public void SetUsers(ObservableCollection<UserModel> U)
        {
            Users = U;
        }

        public void GainUser(UserModel U)
        {
            U = AddDemoAttributesToUserMode(U);
            if (Users == null)
                Users = new ObservableCollection<UserModel>(){U};
            else
                Users.Add(U);
        }

        public void AddMessageToChat(MessageModel message)
        {
            if(_selectedChat!= null)
                _selectedChat.Messages.Add(message);
        }

        private UserModel AddDemoAttributesToUserMode(UserModel u)
        {
            u.ImageSource = "https://picsum.photos/200/300";
            return u;
        }
    }
}


/*
 To add a chat to the view,
you add a user which is an ObservableCollection<UserModel>
that has a chat bound to it; which is ObservableCollection<MessageModel>
 */
