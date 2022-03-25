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

            ObservableCollection<TopicModel> topics = (TopicList.GetNumberOfTopics() != 0) ? TopicList.GetCurrentTopics() : new ObservableCollection<TopicModel>();
            //_PeerService.GetListOfTopics(response);

            //send everyone else in the userlist the user's endpoint
            for (int i = 0; i < users.Count; i++)
            {
                UserModel a = users[i];
                EstablishConnectionWithUser(a.Endpoint);
                _PeerService.GetNewUser(newUser);
            }

            //Add User To UserList
            UserList.AddUser(newUser);

            //Add User To TopicList
            TopicList.AddUserToAllTopic(newUser);
        }

        private static void UserLeft(MessageProtocol job)
        {
            //This is what is expected
            UserModel user = job.messageFiller;

            //remove person from userlist
            UserList.RemoveUser(user);

            //remove person from topic list || todo
            TopicList.RemoveUserFromAllTopics(user);
        }

        private static void TopicWasCreated(MessageProtocol job)
        {
            TopicModel topic = job.messageFiller;

            //add to topic list
            TopicList.AddTopic(topic);

            //notify everyone
            foreach(UserModel user in UserList.GetCurrentUsers())
            {
                EstablishConnectionWithUser(user.Endpoint);
                _PeerService.GetNewTopic(topic);
            }
        }

        private static void UserHasLeftTopic(MessageProtocol job)
        {
            UserModel userModel = job.messageFiller;
            string topicName = job.messageBody;

            //remove from topic list
            TopicList.RemoveUserFromTopic(topicName, userModel);
        }

        private static void TopicReceivedMessage(MessageProtocol job)
        {
            //export topicModel from job for serialization reasons
            MessageModel msgModel = job.messageBody;
            TopicModel topic = job.messageFiller;
            job.messageBody = msgModel.Message;
            job.messageFiller = null;

            //if exist, notify all members with the topic of the topic message
            List<UserModel> usersInTopic = TopicList.TopicReceivedMsg(topic, msgModel);
            foreach (UserModel user in usersInTopic)
            {
                job.destinationEndpoint = user.Endpoint;
                EstablishConnectionWithUser(user.Endpoint);
                _PeerService.GetTopicMsg(job, topic);
            }
        }
    }
}
