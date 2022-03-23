using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColemanServerP2P
{
    public static class Host
    {
        public static BlockingQueue<MessageProtocol> _IncomingQueue = new BlockingQueue<MessageProtocol>();
        public static BlockingQueue<MessageProtocol> _OutboundQueue = new BlockingQueue<MessageProtocol>();
        public static Dictionary<string, string> _UserList = new Dictionary<string, string>();

        /* 
         * Establishes a channel for clients to connect to
         */
        static ServiceHost CreateChannel(string url)
        {
            WSHttpBinding binding = new WSHttpBinding(); //provide binding to clients | its a handshake to agree on protocols and security
            Uri address = new Uri(url); //endpoint
            Type service = typeof(BasicService); //the type of the serviceClass
            ServiceHost host = new ServiceHost(service, address); //create host object
            host.AddServiceEndpoint(typeof(IBasicService), binding, address); //connects the service to the endpoint address with the communication protocols binded
            return host;
        }

        /* 
         * Perform job
         */

        private static void DoSomething(MessageProtocol job)
        {
            Console.WriteLine("I did something");
        }

        private static void WorkOnInboundQueue()
        {
            MessageProtocol job;
            while (true)
            {
                job = _IncomingQueue.deQ();
                DoSomething(job);
            }
        }

        private static void WorkOnOutboundQueue()
        {
            MessageProtocol job;
            while (true)
            {
                //fix
                job = _OutboundQueue.deQ();
                DoSomething(job);
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "WSHttpBinding Service Host";
            Console.Write("\n  Starting Programmatic Service");
            Console.Write("\n =====================================\n");

            ServiceHost host = null;

            try
            {
                //Open Connection
                host = CreateChannel("http://localhost:8080/BasicService");
                host.Open();
                Console.Write("\n  The service is running - Press key to exit:\n");
                
                //Start thread to accept requests
                Thread InboundThread = new Thread(WorkOnInboundQueue);
                Thread OutboundThread = new Thread(WorkOnOutboundQueue);
                InboundThread.Start();
                //OutboundThread.Start();

                //Block to allow console to stay up
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}\n\n", ex.Message);
                return;
            }
            host.Close();
        }
    }
}
