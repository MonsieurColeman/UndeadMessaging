/*
 This file handles the display logic of the client dashboard

This file associates with "Client" to make service calls
This file also associates with mainWindow for GUI related calls
 */

using ColemanPeerToPeer.Core;
using ColemanPeerToPeer.Service;
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
        #region Backing Properties
        private ObservableCollection<MessageModel> _Messages;
        private ObservableCollection<UserModel> _users;
        private string _message;
        private string _username;
        public RelayCommand SendCommand { get; set; }
        private UserModel _selectedChat;
        #endregion

        #region Public Properties
        public ObservableCollection<MessageModel> Messages //list of messages on screen???
        {
            get { return _Messages; }
            set { _Messages = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UserModel> Users //list of users
        {
            get { return _users; }
            set { _users = value;
                OnPropertyChanged();
            }
        }
        public string Username //public interface for username
        {
            get { return _username; }
            set { _username = value;
                OnPropertyChanged();
            }
        }
        public UserModel SelectedChat //The user model that was selected to chat
        {
            get { return _selectedChat; }
            set {
                _selectedChat = value; 
                OnPropertyChanged();
            }
        }
        public string Message //The current string in the textbar
        {
            get { return _message; }
            set { 
                _message = value; 
                OnPropertyChanged();
            }
        }
        #endregion


        public MainViewModel()
        {
            //pass instance to static viewManager
            ViewManager.SetMainViewModelInstance(this);

            //create a new list for this view
            Messages = new ObservableCollection<MessageModel>();

            //create a new list for this view
            Users = new ObservableCollection<UserModel>();

            //Textbox On enter code
            SendCommand = new RelayCommand(SendMessage);
        }


        public void SendMessage(object o)
        /*Chat Textbox behavior*/
        {
            //do nothing if text in textbox is invalid or a chat isnt selected
            if (SelectedChat == null || String.IsNullOrWhiteSpace(Message))
                return;

            //If the Chat that was selected was a TopicType
            if (SelectedChat is TopicModel tm)
                DisplayMessageToTopic(tm);


            //Displays message to my view only if, receipient got message
            else
                if (DisplayMessageToTargetPeer(SelectedChat, Message))
                    DisplayMessageToView();

            //Clear textbox
            Message = "";
        }

        private void DisplayMessageToView()
        //Behavior for displaying a chat message to the currently selected chat view from me
        {
            //Add Msg to the current chat object
            if (SelectedChat.Messages != null) SelectedChat.Messages.Add(GetMsgFromUsermodel(Client.GetMyUserModel(), Message));
            else SelectedChat.Messages = new ObservableCollection<MessageModel>() { GetMsgFromUsermodel(Client.GetMyUserModel(), Message) };

            //Activate Notify Property to update dashboard
            SelectedChat = SelectedChat;
            SelectedChat.LastMessage = SelectedChat.LastMessage;
        }


        #region Main Topic Functions
        public void SetDashTopics(ObservableCollection<TopicModel> topics)
        //Behavior to display list of topics on dash upon login
        {
            //add topic models to the chat list
            foreach (TopicModel topic in topics)
                Users.Add(topic);
        }

        public void AddTopicToDashboard(TopicModel givenTopic)
        //Adds topic chat item to dashboard
        {
            UserModel topic = givenTopic;
            Users.Add(topic);
        }

        public void AddTopicMessageToChat(TopicModel topic, string endpointSrc, string msg)
        //Puts message in dashboard topic object and notifies user that a message has been received
        {
            //paranoia check
            if (Users == null)
                return;

            //Get the actual topicModel
            foreach (UserModel userM in Users)
                if (userM is TopicModel top)
                    if (top.ChatName == topic.ChatName)
                        topic = top;

            //Add new message to topic object
            if (topic.Messages == null)
                topic.Messages = new ObservableCollection<MessageModel>() { GetMsgFromUsermodel(GetUserModelFromEndpoint(endpointSrc), msg) };
            else
                topic.Messages.Add(GetMsgFromUsermodel(GetUserModelFromEndpoint(endpointSrc), msg));

            //Call Notify Property on Topic to show latest message
            topic.LastMessage = topic.LastMessage;
        }

        public void RemoveTopicFromDash(TopicModel topic)
        /* Looks within chat items to find topic model and then removes it*/
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i] is TopicModel tm)
                    if (tm.ChatName == topic.ChatName)
                    {
                        Users.RemoveAt(i);
                        return;
                    }
        }

        private bool DisplayMessageToTopic(TopicModel model)
        //Sends server a msq that we would like to post to a topic
        //bool for no reason
        {
            return Client.SendMessageToTopic(model, GetMsgFromUsermodel(Client.GetMyUserModel(), Message));
        }
        #endregion

        #region Main User Functions
        public void SetUsers(ObservableCollection<UserModel> users)
        //Behavior to display list of users on dash upon login
        {
            Users = users;
        }

        public void AddUserToDashboard(UserModel user)
        //Adds user chat item to dashboard
        {
            Users.Add(user);
        }

        public void RemoveUserFromDashboard(UserModel user)
        //Removes users model from chat items list
        {
            //null validation
            if (Users == null)
                return;
            if (!UserListContainsUser(user))
                return;

            //remove usermodel for chat by comparing endpoints
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == user.Endpoint)
                {
                    Users.RemoveAt(i);
                    return;
                }
        }

        public void AddPrivateMessageToChat(string sourceEndpoint, string msg)
        //Adds msg to the dashboard user object of the sending user's endpoint
        {
            //do nothing if i dont know this person | stranger danger
            if (!UserListContainsUser(sourceEndpoint))
                return;

            //find the user within the user list
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == sourceEndpoint)
                {
                    //Add Message to dashboard object
                    if (Users[i].Messages != null) Users[i].Messages.Add(GetMsgFromUsermodel(Users[i], msg));
                    else Users[i].Messages = new ObservableCollection<MessageModel>() { GetMsgFromUsermodel(Users[i], msg) };

                    //activate notify property
                    Users[i].LastMessage = Users[i].LastMessage;
                    return;
                }
        }
        #endregion

        #region Main Btn Functions
        public void ShutdownChat()
        //Performs Cleanup, when client wants to exit application
        {
            Client.ShutdownChat(Users);
        }

        public void ShowCreateTopicDialog()
        //Shows dialog window when user wants to create a new topic
        {
            CreateTopicView newTopicWindow = new CreateTopicView();
            newTopicWindow.ShowDialog();
        }

        public void LeaveTopic()
        //Sends msq to server that we want to leave the topic
        {
            if (SelectedChat == null)
                return;

            if (!(SelectedChat is TopicModel t))
                return;

            Client.LeaveTopic(SelectedChat.ChatName);
            return;
        }
        #endregion

        #region HelperFunctions
        private MessageModel GetMsgFromUsermodel(UserModel user, string message)
        //Creates a chat message (to be displayed) from a string and user profile
        {
            return new MessageModel()
            {
                Username = user.Username,
                UsernameColor = user.UsernameColor,
                ImageSource = user.ImageSource,
                Message = message,
                Time = DateTime.Now,
                IsFromMe = false,
                FirstMessage = true
            };
        }

        private bool UserListContainsUser(UserModel user)
        //Iterates over list to find a matching endpoint
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == user.Endpoint)
                    return true;
            return false;
        }

        private bool UserListContainsUser(string endPoint)
        //Iterates over list to find a matching endpoint
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == endPoint)
                    return true;
            return false;
        }

        private string GetUsernameFromEndpoint(string endPoint)
        //Performs endpoint comparison to find matching username
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == endPoint)
                    return Users[i].Username;
            throw new NotImplementedException();
        }

        private UserModel GetUserModelFromEndpoint(string endPoint)
        //Performs endpoint comparison to find matching usermodel
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == endPoint)
                    return Users[i];
            if (endPoint == Client.GetMyEndpoint())
                return Client.GetMyUserModel();
            throw new NotImplementedException();
        }

        private bool DisplayMessageToTargetPeer(UserModel user, string message)
        //Helper method which returns true if the target received the message to indicated
        //that we should display the message to our own dashboard
        {
            return Client.SendPrivateMessage(user, message);
        }

        private bool ConsecutiveMessages(UserModel chatModel, string endpoint)
        /*An after project function | adds functionality to change message display
         based on if a person has send consecutives messages*/
        {
            bool firstMsg = true;
            if (chatModel.Messages != null)
                if (chatModel.Messages.Count > 0)
                    if (chatModel.Messages.Last().Username == GetUsernameFromEndpoint(endpoint))
                        firstMsg = false;
            return firstMsg;
        }
        #endregion
    }
}

/* 
 * Maintenance History
 * 
 * 0.1: Made prototype of messaging dashboard utilizing binding and properties
 * 0.3: Added inheritance via Observable Objects which leverages reflection
 * 0.4: Added relationship with view manager due to sporatic instantiation
 * 0.6: This file now has a relationship with Client
 * 0.7: Added Peer to Peer functionality
 * 0.8: Added topic functionality
 * 0.9: Heavy refactoring and commenting
 */
