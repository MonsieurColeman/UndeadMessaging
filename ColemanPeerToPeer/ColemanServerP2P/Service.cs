
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
        int i;

        public void SendMSG(string msg)
        {
            Console.WriteLine("\n  Service received message {0} , i = {1}", i++);
        }

        public void TestMSG(string msg)
        {
            Console.WriteLine("\n  Service received message {0} , i = {1}", i++);

        }

        public string GetMSG()
        {
            return "";
            new MessageProtocol
            {
                sourceEndpoint = "Server",
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin,
                messageBody = "test",
                destinationEndpoint = "unkown"
            };
        }
    }
}
