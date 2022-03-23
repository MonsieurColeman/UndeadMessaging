using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ColemanServerP2P
{
    public static class JobManager
    {
        public static IPeer _PeerService;

        public static void EstablishConnectionWithUser(string endpoint)
        {
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(endpoint);
            ChannelFactory<IPeer> factory = new ChannelFactory<IPeer>(binding, address);
            _PeerService = factory.CreateChannel(); //returns an object of that service  
        }

        public static void ProcessJobRequeust(MessageProtocol job)
        {
            switch (job.messageProtocolType)
            {
                case MessageType.join:
                    UserJoined(job);
                    break;
                case MessageType.userLeft:
                    UserLeft(job);
                    break;
                case MessageType.topicCreate:
                    TopicWasCreated(job);
                    break;
                case MessageType.leaveTopic:
                    UserHasLeftTopic(job);
                    break;
                case MessageType.topicMsg:
                    TopicReceivedMessage(job);
                    break;
                default:
                    Console.WriteLine("I receive a weird message");
                    break;
            }
        }

        private static void UserJoined(MessageProtocol job)
        {
            //To Clear Confusion
            UserModel newUser = job.messageFiller;

            //Connect with user who sent job
            EstablishConnectionWithUser(job.sourceEndpoint);

            //Return a list of users
            ObservableCollection<UserModel> users = (UserList.GetNumberOfUsers() != 0) ? UserList.GetCurrentUsers() : new ObservableCollection<UserModel>();
            _PeerService.GetListOfUsers(users);

            //send user list of topics
            /*
            List<string> topics = (TopicList.GetNumberOfTopics() != 0) ? TopicList.GetCurrentTopics() : new List<string>();
            response = new MessageProtocol
            {
                sourceEndpoint = "Server",
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin,
                messageBody = topics,
                destinationEndpoint = job.sourceEndpoint
            };
            */
            //_PeerService.SendMSG(response);

            //send everyone else in the userlist the user's endpoint
            for(int i = 0; i < users.Count; i++)
            {
                UserModel a = users[i];
                EstablishConnectionWithUser(a.Endpoint);
                _PeerService.GetNewUser(newUser);
                Console.WriteLine("this happened");
            }

            //Add User To UserList
            UserList.AddUser(newUser);
        }

        private static void UserLeft(MessageProtocol job)
        {
            //remove person from userlist
            UserList.RemoveUser(job.messageFiller);

            //remove person from topic list || todo
        }

        private static void TopicWasCreated(MessageProtocol job)
        {
            //add to topic list

            //notify everyone
        }

        private static void UserHasLeftTopic(MessageProtocol job)
        {
            //remove from topic list

            //check to see if anyone is left in topic
            //if 0, delete
        }

        private static void TopicReceivedMessage(MessageProtocol job)
        {
            //check topic to make sure it still exists

            //if exist, notify all members with the topic of the topic message

        }
    }
}
