﻿using ColemanPeerToPeer.Core;
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
        //current view messages
        //public  ObservableCollection<MessageModel> Messages2 { get; set; }

        private ObservableCollection<MessageModel> _Messages;

        public ObservableCollection<MessageModel> Messages
        {
            get { return _Messages; }
            set { _Messages = value;
                OnPropertyChanged();
            }
        }

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


            
            SendCommand = new RelayCommand(SendMessage);
            
        }

        public void SendMessage(object o)
        {
            if (SelectedChat == null || String.IsNullOrWhiteSpace(Message))
                return;

            //Displays message to my view only if, receipient got message
            if(DisplayMessageToTargetPeer(SelectedChat, Message))
                DisplayMessageToView();

            Message = ""; //Clears textbox
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

            //SelectedChat.Messages.Last();
            string s = SelectedChat.LastMessage;
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

        private UserModel AddDemoAttributesToUserMode(UserModel u)
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

        private void RemoveUserFromUserList(UserModel user)
        {
            UserModel u;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Endpoint == user.Endpoint)
                    Users.RemoveAt(i);
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
                        Users[i].Messages.Add(newMsg);
                    else
                    {
                        Users[i].Messages = new ObservableCollection<MessageModel>() { newMsg };
                        //Users[i].Messages.Add(newMsg);
                    }
                }
            }
        }

        private bool DisplayMessageToTargetPeer(UserModel user, string message)
        {
            return Client.SendPrivateMessage(user, message);
        }
    }
}


/*
 To add a chat to the view,
you add a user which is an ObservableCollection<UserModel>
that has a chat bound to it; which is ObservableCollection<MessageModel>
 */
