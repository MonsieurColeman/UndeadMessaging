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
            _Dashboard.SetUsers(setupMsg.messageFiller);
        }

        private static void SetupTopicForDashboard(MessageProtocol setupMsg)
        {
            ObservableCollection<TopicModel> topics = setupMsg.messageFiller;
            //ObservableCollection<UserModel> topicDashboardObjs = new ObservableCollection<UserModel>();


            if (_Dashboard == null)
                _Dashboard = ViewManager.GetMainViewModelInstance();
            _Dashboard.SetTopics(topics);
        }

        private static void AddTopicToDashboard(MessageProtocol dashMsg)
        {
            //_Dashboard.GainTopic(Converter.TopicModelToUserModel(dashMsg.messageFiller));
            _Dashboard.GainTopic(dashMsg.messageFiller);
        }

        private static void AddUserToDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.GainUser(dashMsg.messageFiller);
        }

        private static void RemoveUserFromDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.RemoveUser(dashMsg.messageFiller);
        }

        private static void AddMessageToDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.AddPrivateMessageToChat(dashMsg);
        }

        private static void AddTopicMsgToDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.AddTopicMessageToChat(dashMsg);
        }

        private static void RemoveTopicFromDashboard(MessageProtocol dashMsg)
        {
            _Dashboard.RemoveTopicFromDash(dashMsg.messageFiller);
        }
    }
}
