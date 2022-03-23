
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
        public bool Join(MessageProtocol Specialmsg)
        {
            Console.WriteLine($"Here's what I got: {0}",Specialmsg.sourceEndpoint);

            if (!(Specialmsg.messageProtocolType == MessageType.join && UserList.UniqueUserCheck(Specialmsg.messageBody)))
                return false;

            Host._IncomingQueue.enQ(Specialmsg);
            return true;
        }

        public void test()
        {

        }

        public MessageProtocol GetMSG()
        {
            return Host._IncomingQueue.deQ();
        }

        public void SendComplicatedMsg(string msg)
        {
            MessageProtocol convertedMsg = new JavaScriptSerializer().Deserialize<MessageProtocol>(msg);
            Host._IncomingQueue.enQ(convertedMsg);
        }

        public bool JoinComplicated(string m)
        {
            MessageProtocol msg = new JavaScriptSerializer().Deserialize<MessageProtocol>(m);

            if (msg.messageProtocolType == MessageType.join && UserList.UniqueUserCheck(msg.messageBody))
            {
                Host._IncomingQueue.enQ(msg);
                return true;
            }
            return false;
        }
    }
}


/*
            new MessageProtocol
            {
                sourceEndpoint = "Server",
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin,
                messageBody = "test",
                destinationEndpoint = "unkown"
            };
 
 */
