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

        private ObservableCollection<MessageModel> _Messages;
        private ObservableCollection<UserModel> _users;
        private ObservableCollection<TopicModel> _topics;
        private string _message;
        public string userNameColor;
        private string _username;
        public string _profilePicture = "https://picsum.photos/200/300";
        public RelayCommand SendCommand { get; set; }
        private UserModel _selectedChat;
        private TopicModel _selectedTopic;

        public TopicModel SelectedTopic //the topic model that was selected to chat
        {
            get { return _selectedTopic; }
            set { _selectedTopic = value;
                OnPropertyChanged();
            }
        }
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
        {//public interface for username
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
        public ObservableCollection<TopicModel> Topics //list of topics
        {
            get { return _topics; }
            set
            {
                _topics = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            //pass instance to static viewManager
            ViewManager.SetMainViewModelInstance(this);
            //create a new list for this view
            Messages = new ObservableCollection<MessageModel>();
            //create a new list for this view
            Users = new ObservableCollection<UserModel>();
            //my username color
            userNameColor = "#000000";
            //legacy code that im afraid to delete
            SendCommand = new RelayCommand(SendMessage);
        }

        public void SendMessage(object o)
        /*                              */
        {
            //do nothing if text in textbox is invalid
            if (SelectedChat == null || String.IsNullOrWhiteSpace(Message))
                return;

            if (SelectedChat is TopicModel tm)
            {
                DisplayMessageToTopic(tm);
            }
            else
            {
                //Displays message to my view only if, receipient got message
                if (DisplayMessageToTargetPeer(SelectedChat, Message))
                    DisplayMessageToView();
            }

            //Clear textbox
            Message = "";
        }

        private void DisplayMessageToView()
        {
            MessageModel newMsg = new MessageModel
            {
                Username = Username,
                UsernameColor = userNameColor,
                ImageSource = _profilePicture,
                Message = Message,
                Time = DateTime.Now,
                IsFromMe = true,
                FirstMessage = true
            };

            if (SelectedChat.Messages == null)
                SelectedChat.Messages = new ObservableCollection<MessageModel> { newMsg };
            else
                SelectedChat.Messages.Add(newMsg);

            SelectedChat = SelectedChat;
            SelectedChat.LastMessage = SelectedChat.LastMessage; //just calling it updates user card
            //int truncatedNum = (Message.Count() < 30) ? Message.Count() : 30;
            //SelectedChat.LastMessage = "You: " + Message.Substring(0, truncatedNum);
        }

        public void SetUsers(ObservableCollection<UserModel> U)
        {
            Users = U;
        }

        public void SetTopics(ObservableCollection<TopicModel> topics)
        {
            if (topics.Count == 0)
                return;

            //ObservableCollection<UserModel> topicList = Converter.TopicListToUserList(topics);
            foreach(TopicModel topic in topics)
                Users.Add(topic);
        }

        public void GainUser(UserModel U)
        {
            U = AddDemoAttributesToUserMode(U);
            if (Users == null)
                Users = new ObservableCollection<UserModel>(){U};
            else
                Users.Add(U);
        }

        public void GainTopic(TopicModel givenTopic)
        {
            givenTopic = AddDemoAttributesToTopicsMode(givenTopic);
            UserModel topic = givenTopic;//Converter.TopicModelToUserModel(givenTopic);

            if (Users == null)
                Users = new ObservableCollection<UserModel>() { topic };
            else
                Users.Add(topic);
        }

        public void RemoveUser(UserModel user)
        {
            if (Users == null)
                return;

            if (!UserListContainsUser(user))
                return;

            RemoveUserFromUserList(user);
        }

        public void AddMessageToChat(MessageModel message)
        {
            if(_selectedChat!= null)
                _selectedChat.Messages.Add(message);
        }

        public void AddPrivateMessageToChat(MessageProtocol newMsq)
        {
            //if we receive a message from someone not in our user list,
            //do nothing
            if (!UserListContainsUser(newMsq.sourceEndpoint))
                return;

            AddMessageByEndpoint(newMsq.sourceEndpoint, newMsq.messageBody);
        }


        public void AddTopicMessageToChat(MessageProtocol newMsg)
        {
            TopicModel topic = newMsg.messageFiller;

            if (Users == null)
                return ;

            /*for (int i = 0; i < Users.Count; i++)
                if (Users[i].ChatName == topic.ChatName)
                    topic = (TopicModel)Users[i];
                else
                {
                    Console.WriteLine("--"+Users[i].Username);
                    Console.WriteLine(topic.ChatName);
                }
            */
            foreach (UserModel userM in Users)
                if (userM is TopicModel top)
                {
                    if (top.ChatName == topic.ChatName)
                        topic = top;
                    else
                    {
                        Console.WriteLine("--" + top.Username);
                        Console.WriteLine(topic.ChatName);
                    }
                }

            //firstMsg = true;
            if (topic.Messages == null)
                topic.Messages = new ObservableCollection<MessageModel>();

            //if (!topic.Messages.Count == 0)

            UserModel user = GetUserModelFromEndpoint(newMsg.sourceEndpoint);
            MessageModel dashMsg = new MessageModel()
            {
                Username = user.Username,
                UsernameColor = user.UsernameColor,
                ImageSource = user.ImageSource,
                Message = newMsg.messageBody,
                Time = DateTime.Now,
                IsFromMe = false,
                FirstMessage = true
            };
            topic.Messages.Add(dashMsg);
            SelectedChat.LastMessage = SelectedChat.LastMessage;
        }

        /*
        public void AddTopicMessageToChat(MessageProtocol newMsg)
        {
            TopicModel topic = newMsg.messageFiller;

            if (Topics == null)
                Topics = new ObservableCollection<TopicModel>() {topic};
            
            for(int i=0;i<Topics.Count;i++)
                if(Topics[i].ChatName == topic.ChatName)
                    topic = Topics[i];

            //firstMsg = true;
            if (topic.Messages == null)
                topic.Messages = new ObservableCollection<MessageModel>();

            //if (!topic.Messages.Count == 0)

            UserModel user = GetUserModelFromEndpoint(newMsg.sourceEndpoint);
            MessageModel dashMsg = new MessageModel()
            {
                Username = user.Username,
                UsernameColor = user.UsernameColor,
                ImageSource = user.ImageSource,
                Message = newMsg.messageBody,
                Time = DateTime.Now,
                IsFromMe = false,
                FirstMessage = true
            };
            topic.Messages.Add(dashMsg);
        }
        */

        private UserModel AddDemoAttributesToUserMode(UserModel u)
        {
            u.ImageSource = "https://picsum.photos/200/300";
            return u;
        }

        private TopicModel AddDemoAttributesToTopicsMode(TopicModel u)
        {
            u.ImageSource = "https://picsum.photos/200/300";
            return u;
        }

        public void ShutdownChat()
        {
            Client.ShutdownChat(Users);
        }


        private bool UserListContainsUser(UserModel user)
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == user.Endpoint)
                    return true;
            return false;
        }

        private bool UserListContainsUser(string endPoint)
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == endPoint)
                    return true;
            return false;
        }

        private string GetUsernameFromEndpoint(string endPoint)
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == endPoint)
                    return Users[i].Username;
            throw new NotImplementedException();
        }

        private UserModel GetUserModelFromEndpoint(string endPoint)
        {
            for (int i = 0; i < Users.Count; i++)
                if (Users[i].Endpoint == endPoint)
                    return Users[i];
            if(endPoint == Client.GetMyEndpoint())
                return Client.GetMyUserModel();
            throw new NotImplementedException();
        }

        private void RemoveUserFromUserList(UserModel user)
        {
            UserModel u;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Endpoint == user.Endpoint)
                    Users.RemoveAt(i);
            }
        }

        public void RemoveTopicFromDash(TopicModel topic)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if(Users[i] is TopicModel tm)
                    if (tm.ChatName == topic.ChatName)
                    {
                        Users.RemoveAt(i);
                        return;
                    }
            }
        }

        private void AddMessageByEndpoint(string endpoint, string msg)
        {
            bool firstMsg;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Endpoint == endpoint)
                {
                    //safe way of checking for first msg without risking
                    //null exception
                    //Only switch true to false if intending to implement
                    //different display of messages when the last message is by the
                    //same person sending a new message
                    firstMsg = true;
                    if(Users[i].Messages != null)
                        if(Users[i].Messages.Count > 0)
                            if(Users[i].Messages.Last().Username == GetUsernameFromEndpoint(endpoint))
                                //firstMsg = false;
                                firstMsg = true;

                    MessageModel newMsg = new MessageModel()
                    {
                        Username = Users[i].Username,
                        UsernameColor = Users[i].UsernameColor,
                        ImageSource = Users[i].ImageSource,
                        Message = msg,
                        Time = DateTime.Now,
                        IsFromMe = false,
                        FirstMessage = firstMsg
                    };

                    //Add Message
                    if (Users[i].Messages != null)
                    {
                        Users[i].Messages.Add(newMsg);

                    }
                    else
                    {
                        Users[i].Messages = new ObservableCollection<MessageModel>() { newMsg };

                    }
                    int truncatedNum = (msg.Count() < 30) ? msg.Count() : 30;
                    //Users[i].LastMessage = "You: " + msg.Substring(0, truncatedNum);
                    Users[i].LastMessage = ""; //just setting it updates card
                }
            }
        }

        private bool DisplayMessageToTargetPeer(UserModel user, string message)
        {
            return Client.SendPrivateMessage(user, message);
        }

        private bool DisplayMessageToTopic(TopicModel model)
        {
            MessageModel newMsg = new MessageModel
            {
                Username = Username,
                UsernameColor = userNameColor,
                ImageSource = _profilePicture,
                Message = Message,
                Time = DateTime.Now,
                IsFromMe = true,
                FirstMessage = true
            };

            return Client.SendMessageToTopic(model, newMsg);
        }

        public void LeaveTopic()
        {
            if (SelectedChat == null)
                return;

            if (!(SelectedChat is TopicModel t))
                return;

            string s = SelectedChat.ChatName;

            Client.LeaveTopic(SelectedChat.ChatName);
            return;
        }
        /*
        private bool DisplayMessageToTopic(UserModel model)
        {
            MessageModel newMsg = new MessageModel
            {
                Username = Username,
                UsernameColor = userNameColor,
                ImageSource = _profilePicture,
                Message = Message,
                Time = DateTime.Now,
                IsFromMe = true,
                FirstMessage = true
            };

            return Client.SendMessageToTopic(Converter.UserModelToTopicModel(model), newMsg);
        }
        */
        public void CreateTopic()
        {
            Client.CreateTopic("testTopic"); 
        }
    }
}


/*
 To add a chat to the view,
you add a user which is an ObservableCollection<UserModel>
that has a chat bound to it; which is ObservableCollection<MessageModel>
 */
