using ColemanPeerToPeer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
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
                default:
                    Console.WriteLine("I receive a weird message");
                    break;
            }
        }

        private static void SetupUsersForDashboard(MessageProtocol setupMsg)
        {
            if (_Dashboard == null)
                _Dashboard = ViewManager.GetMainViewModelInstance();
            _Dashboard.SetUsers(setupMsg.messageBody);
        }

        private static void SetupTopicForDashboard(MessageProtocol setupMsg)
        {
            //toDo
        }

        private static void AddTopicToDashboard(MessageProtocol dashMsg)
        {
            //toDo
        }

        private static void AddUserToDashboard(MessageProtocol dashMsg)
        {
            //toDo
        }

        private static void RemoveUserFromDashboard(MessageProtocol dashMsg)
        {
            //toDo
        }

        private static void AddMessageToDashboard(MessageProtocol dashMsg)
        {
            //toDo
        }
    }
}
