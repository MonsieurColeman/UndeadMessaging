
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        public bool Join(MessageProtocol msg)
        {
            if(msg.messageProtocolType == MessageType.join && UserList.UniqueUserCheck(msg.messageBody))
            {
                Host._IncomingQueue.enQ(msg);
                return true;
            }
            return false;
        }

        public MessageProtocol GetMSG()
        {
            return Host._IncomingQueue.deQ();
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
