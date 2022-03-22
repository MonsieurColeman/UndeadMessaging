using delLater;
using System;
using System.ServiceModel;
using System.Threading;

namespace PeerServiceConsole
{
    class Client
    {
        public static string uuu = "http://localhost:8080/BasicService";
        IBasicService svc;

        /*
         * Create a communication channel to converse with server
         * using contract IBasicService.
         * 
         * Channel doesn't attempt to connect to server
         * until first service call.
         */
        Client(string url)
        {
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(url);
            ChannelFactory<IBasicService> factory = new ChannelFactory<IBasicService>(binding, address);
            svc = factory.CreateChannel(); //returns an object of that service           
        }

        //----< Wrapper attempts to call service method several times >------
        /*
         *  Func<string> is a delegate that invokes functions
         *  which take no arguments and return strings
         */

        void SendMessage(string msgg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded

            //Func<string> fnc = () => { svc.SendMSG(new MessageProtocol { }); return "service succeeded"; };
            string msg = "test";
            //svc.TestMSG(msg);
            if (svc == null)
                return;
            Func<string> fnc = () => { svc.sendMessage(msgg); return "service succeeded"; };

            ServiceHandler.AttemptService(fnc);
        }

        string GetMessage()
        {
            //MessageProtocol msg;
            //Func<string> fnc = () => { msg = svc.GetMSG(); return "this does something"; };
            //return ServiceRetryWrapper(fnc);
            return "";
        }

        static void Main(string[] args)
        {
            Console.Title = "BasicHttp Client";
            Console.Write("\n  Starting Programmatic Basic Service Client");
            Console.Write("\n ============================================\n");
            MessageProtocol msg = new MessageProtocol { };
            string url = "http://localhost:8080/BasicService";
            Client client = new Client(uuu);
            string mmssgg;

            client.SendMessage("this is a test");
            //client.SendMessage(msg);
            //client.SendMessage(msg);
            //client.SendMessage(msg);
            //client.SendMessage(msg);
            //client.SendMessage(msg);
            //mmssgg = client.GetMessage();
            //Console.Write("\n  Message recieved from Service: {0}\n\n", mmssgg);
        }
    }
}