using ColemanPeerToPeer.MVVM.ViewModel;
using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanPeerToPeer.Service
{

    public class RequestHandler
    {
        private static MainViewModel _Dashboard;

        public static void ProcessJobRequeust(MessageProtocol job)
        {
            switch (job.messageProtocolType)
            {
                case MessageType.receiveCurrentUsersOnJoin: //receive users
                    SetupUsersForDashboard(job);
                    break;
                case MessageType.receiveCurrentTopicsOnJoin: //receive topics
                    SetupTopicForDashboard(job);
                    break;
                case MessageType.topicCreate: //receive new topic || create topic
                    AddTopicToDashboard(job);
                    break;
                case MessageType.userJoined: //receive user and endpoint
                    AddUserToDashboard(job);
                    break;
                case MessageType.userLeft: //remove user from list
                    RemoveUserFromDashboard(job);
                    break;
                case MessageType.privateMessage: //receive message from user
                    AddMessageToDashboard(job);
                    break;
                case MessageType.topicMsg: //receive message from user
                    AddTopicMsgToDashboard(job);
                    break;
                case MessageType.leaveTopic: //leave topic
                    RemoveTopicFromDashboard(job);
                    break;
                default:
                    Console.WriteLine("I receive a weird message");
                    break;
            }
        }

        private static void SetupUsersForDashboard(MessageProtocol setupMsg)
        {
            if (_Dashboard == null)
                _Dashboard = ViewManager.GetMainViewModelInstance();
            //ObservableCollection<UserModel> 
            _Dashboard.SetUsers(setupMsg.messageFiller);
        }

        private static void SetupTopicForDashboard(MessageProtocol setupMsg)
        {
            ObservableCollection<TopicModel> topics = setupMsg.messageFiller;
            if(topics.Count == 0)
                return;


            if (_Dashboard == null)
                _Dashboard = ViewManager.GetMainViewModelInstance();
            _Dashboard.SetDashTopics(topics);
        }

        private static void AddTopicToDashboard(MessageProtocol dashMsg)
        {
            TopicModel topic = dashMsg.messageFiller;
            TopicModel decorateTopic = AddDemoAttributesToTopicsMode(topic);
            _Dashboard.AddTopicToDashboard(decorateTopic);
        }

        private static void AddUserToDashboard(MessageProtocol dashMsg)
        {
            UserModel user = dashMsg.messageFiller;
            user = AddDemoAttributesToUserMode(user);
            _Dashboard.AddUserToDashboard(user);
        }

        private static void RemoveUserFromDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.RemoveUserFromDashboard(dashMsg.messageFiller);
        }

        private static void AddMessageToDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.AddPrivateMessageToChat(dashMsg.sourceEndpoint, dashMsg.messageBody);
        }

        private static void AddTopicMsgToDashboard(MessageProtocol dashMsg)
        {
            TopicModel topic = dashMsg.messageFiller;
            string sourceEndpoint = dashMsg.sourceEndpoint;
            string msg = dashMsg.messageBody;
            _Dashboard.AddTopicMessageToChat(topic, sourceEndpoint, msg);
        }

        private static void RemoveTopicFromDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.RemoveTopicFromDash(dashMsg.messageFiller);
        }



        private static TopicModel AddDemoAttributesToTopicsMode(TopicModel u)
        {
            u.ImageSource = "https://picsum.photos/200/300";
            return u;
        }

        private static UserModel AddDemoAttributesToUserMode(UserModel u)
        {
            u.ImageSource = "https://picsum.photos/200/300";
            return u;
        }
    }
}
