using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColemanServerP2P
{
    public static class JobManager
    {

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
            //Return a list of users
            Dictionary<string, string> users = (UserList.GetNumberOfUsers() != 0) ? UserList.GetCurrentUsers() : new Dictionary<string, string>();
            Host._OutboundQueue.enQ(new MessageProtocol
            {
                sourceEndpoint = "Server",
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin,
                messageBody = users,
                destinationEndpoint = job.sourceEndpoint
            });

            //send user list of topics
            List<string> topics = (TopicList.GetNumberOfTopics() != 0) ? TopicList.GetCurrentTopics() : new List<string>();
            Host._OutboundQueue.enQ(new MessageProtocol
            {
                sourceEndpoint = "Server",
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin,
                messageBody = topics,
                destinationEndpoint = job.sourceEndpoint
            });

            //send everyone else in the userlist the user's endpoint
            //TODO
        }

        private static void UserLeft(MessageProtocol job)
        {
            //send users a message that person has left

            //remove person from userlist
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
