/*
 This file handles the sorting and preprocessing of tasks to be send
    to the main view model
 */

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
        //Calls a function based on the MessageType
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
                    Console.WriteLine(GlobalStrings.error_unexpectedMsgType);
                    break;
            }
        }

        private static void SetupUsersForDashboard(MessageProtocol setupMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure dash instance is acquired
            if (_Dashboard == null)
                _Dashboard = ViewManager.GetMainViewModelInstance();

            //Make sure error is caught here if wrong type is receive
            ObservableCollection<UserModel> users = setupMsg.messageFiller;

            //Tell Dash to Do its thing
            _Dashboard.SetUsers(users);
        }

        private static void SetupTopicForDashboard(MessageProtocol setupMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure dash instance is acquired
            if (_Dashboard == null)
                _Dashboard = ViewManager.GetMainViewModelInstance();

            //Make sure error is caught here if wrong type is receive
            ObservableCollection<TopicModel> topics = setupMsg.messageFiller;
            if(topics.Count == 0)
                return;

            //Tell Dash to Do its thing
            _Dashboard.SetDashTopics(topics);
        }

        private static void AddTopicToDashboard(MessageProtocol dashMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure error is caught here if wrong type is receive
            TopicModel topic = dashMsg.messageFiller;
            TopicModel decorateTopic = AddDemoAttributesToTopicsMode(topic);

            //Tell Dash to Do its thing
            _Dashboard.AddTopicToDashboard(decorateTopic);
        }

        private static void AddUserToDashboard(MessageProtocol dashMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure error is caught here if wrong type is receive
            UserModel user = dashMsg.messageFiller;
            user = AddDemoAttributesToUserMode(user);

            //Tell Dash to Do its thing
            _Dashboard.AddUserToDashboard(user);
        }

        private static void RemoveUserFromDashboard(MessageProtocol dashMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure error is caught here if wrong type is receive
            UserModel user = dashMsg.messageFiller;

            //Tell Dash to Do its thing
            _Dashboard.RemoveUserFromDashboard(user);
        }

        private static void AddMessageToDashboard(MessageProtocol dashMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure error is caught here if wrong type is receive
            string sourceEndpoint = dashMsg.sourceEndpoint;
            string msg = dashMsg.messageBody;

            //Tell Dash to Do its thing
            _Dashboard.AddPrivateMessageToChat(sourceEndpoint, msg);
        }

        private static void AddTopicMsgToDashboard(MessageProtocol dashMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure error is caught here if wrong type is receive
            TopicModel topic = dashMsg.messageFiller;
            string sourceEndpoint = dashMsg.sourceEndpoint;
            string msg = dashMsg.messageBody;

            //Tell Dash to Do its thing
            _Dashboard.AddTopicMessageToChat(topic, sourceEndpoint, msg);
        }

        private static void RemoveTopicFromDashboard(MessageProtocol dashMsg)
        //Performs job processing before passing to dashboard
        {
            //Make sure error is caught here if wrong type is receive
            TopicModel topic = dashMsg.messageFiller;

            //Tell Dash to Do its thing
            _Dashboard.RemoveTopicFromDash(dashMsg.messageFiller);
        }



        private static TopicModel AddDemoAttributesToTopicsMode(TopicModel u)
        //Adds image since project doesnt prompt user for image on login
        {
            u.ImageSource = GlobalStrings.lipsum_Image;
            return u;
        }

        private static UserModel AddDemoAttributesToUserMode(UserModel u)
        //Adds image since project doesnt prompt user for image on login
        {
            u.ImageSource = GlobalStrings.lipsum_Image;
            return u;
        }
    }
}

/*
 Maintenance History

0.5 Added skeleton functions along with switch statements to parse MessageTypes
0.8 Added functionality to the skeleton functions
0.9 Remove some processing from the mainviewmodel and put it here
0.95 Added explicit declarations to ensure an error trips in this file instead of the viewModel
1.0 Added comments
 */