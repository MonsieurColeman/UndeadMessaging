using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

namespace ColemanPeerToPeer.Service
{
    /*
     This file only stores the implementation
        of the service interface
     */



    class Peer : IPeer
    {


        /*
         * Create a communication channel to converse with server
         * using contract IBasicService.
         * 
         * Channel doesn't attempt to connect to server
         * until first service call.
         */



        public void SendMessage(MessageProtocol msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded
            Func<string> fnc = () => { Client.serverService.SendMSG(msg); return "service succeeded"; };
            string code = ServiceHandler.AttemptService(fnc);
        }

        public string GetMessage()
        {
            MessageProtocol msg;
            return "";
        }

        public void SendMSG(MessageProtocol msg)
        {
            Client._IncomingQueue.enQ(msg);
        }

        public MessageProtocol GetMSG()
        {
            throw new NotImplementedException();
        }

        public void GetListOfUsers(ObservableCollection<UserModel> users)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = users,
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin
            };
            Client._IncomingQueue.enQ(msg);
        }

        public void GetNewUser(UserModel newUser)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = newUser,
                messageProtocolType = MessageType.userJoined
            };
            Client._IncomingQueue.enQ(msg);
        }

        public void UserLeft(UserModel user)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = user,
                messageProtocolType = MessageType.userLeft
            };
            Client._IncomingQueue.enQ(msg);
        }

        public void GetListOfTopics(ObservableCollection<TopicModel> topics)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = topics,
                messageProtocolType = MessageType.receiveCurrentTopicsOnJoin
            };
            Client._IncomingQueue.enQ(msg);
        }

        public void GetNewTopic(TopicModel topic)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = topic,
                messageProtocolType = MessageType.topicCreate
            };
            Client._IncomingQueue.enQ(msg);
        }

        public void GetTopicMsg(MessageProtocol msg, TopicModel topic)
        {
            msg.messageFiller = topic;
            Client._IncomingQueue.enQ(msg);
        }

        public void UnsubscribeFromTopic(MessageProtocol msg, TopicModel topic)
        {
            msg.messageFiller = topic;
            Client._IncomingQueue.enQ(msg);
        }
    }
}
