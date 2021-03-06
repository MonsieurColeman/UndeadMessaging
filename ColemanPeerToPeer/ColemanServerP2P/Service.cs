
using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ColemanServerP2P
{
    /*
   * InstanceContextMode determines the activation policy, e.g.:
   * 
   *   PerCall    - remote object created for each call
   *              - runs on thread dedicated to calling client
   *              - this is default activation policy
   *   PerSession - remote object created in session on first call
   *              - session times out unless called again within timeout period
   *              - runs on thread dedicated to calling client
   *   Singleton  - remote object created in session on first call
   *              - session times out unless called again within timeout period
   *              - runs on one thread so all clients see same instance
   *              - access must be synchronized
   */
    // [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class BasicService : IBasicService
    {
        public void SendMSG(MessageProtocol msg)
        {
            Console.WriteLine("\n  Server has received message from {0}", msg.sourceEndpoint);

            Host._IncomingQueue.enQ(msg);
        }

        /*
         * Interface for clients to immediately know if they can join
         */
        public bool Join(MessageProtocol m)
        {
            if (!(m.messageProtocolType == MessageType.join && UserList.UniqueUserCheck(m.messageBody)))
                return false;

            UserModel userModel = new JavaScriptSerializer().Deserialize<UserModel>(m.messageFiller);
            MessageProtocol msg = new MessageProtocol()
            {
                sourceEndpoint = m.sourceEndpoint,
                messageBody = m.messageBody,
                messageFiller = userModel,
                messageProtocolType = m.messageProtocolType
                ,
                destinationEndpoint = m.destinationEndpoint
            };

            Host._IncomingQueue.enQ(msg);
            return true;
        }

        public MessageProtocol GetMSG()
        {
            return Host._IncomingQueue.deQ();
        }

        public bool Join(MessageProtocol m, UserModel userProfile)
        {
            if (!(m.messageProtocolType == MessageType.join && UserList.UniqueUserCheck(m.messageBody)))
                return false;

            m.messageFiller = userProfile;
            Host._IncomingQueue.enQ(m);
            return true;
        }

        public void Leave(MessageProtocol msg, UserModel userProfile)
        {
            msg.messageFiller = userProfile;
            Host._IncomingQueue.enQ(msg);
        }

        public void LeaveTopic(MessageProtocol msg, UserModel user)
        {
            msg.messageFiller = user;
            Host._IncomingQueue.enQ(msg);
        }

        public void MsgTopic(MessageProtocol msg, MessageModel msgModel, TopicModel topic)
        {
            msg.messageBody = msgModel;
            msg.messageFiller = topic;
            Host._IncomingQueue.enQ(msg);
        }

        public bool CreateTopic(MessageProtocol msg, TopicModel topic)
        {
            if(!TopicList.UniqueTopicCheck(topic.ChatName))
                return false;

            msg.messageFiller = topic;
            Host._IncomingQueue.enQ(msg);
            return true;
        }
    }
}

