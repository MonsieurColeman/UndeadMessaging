using ColemanPeerToPeer.Core;
using ColemanPeerToPeer.MVVM.Model;
using ColemanPeerToPeer.TestScripts;
using System;
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
        public ObservableCollection<UserModel> Users { get; set; }
        //stuff
        private string _message;
        //otherstuff
        public string userNameColor;

        private string _username;

        //public interface for username
        public string Username
        {
            get { return _username; }
            set { _username = value; }
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
            //local username
            Username = "Patrick";
            userNameColor = "#000000";

            //populate the user list with a random number of users
            int users = new Random().Next(0, 100);

            
            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    FirstMessage = false
                });
                Message = "";
            });
            
            
            
            //Test objects
            SetUsers(UserMessageTester.GetUsers());
            GainUser(UserMessageTester.GetExampleTopic());

        }

        public void SendMessage()
        {
            MessageBox.Show("test");
        }

        public void SetUsers(ObservableCollection<UserModel> U)
        {
            Users = U;
        }

        public void GainUser(UserModel U)
        {
            Users.Add(U);
        }

        public void AddMessageToChat(MessageModel message)
        {
            if(_selectedChat!= null)
                _selectedChat.Messages.Add(message);
        }
    }
}

/*
 To add a chat to the view,
you add a user which is an ObservableCollection<UserModel>
that has a chat bound to it; which is ObservableCollection<MessageModel>
 */
