using System.ServiceModel;

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
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress address = new EndpointAddress(url);
            ChannelFactory<IBasicService> factory = new ChannelFactory<IBasicService>(binding, address);
            svc = factory.CreateChannel();
        }

        //----< Wrapper attempts to call service method several times >------
        /*
         *  Func<string> is a delegate that invokes functions
         *  which take no arguments and return strings
         */
        string ServiceRetryWrapper(Func<string> fnc)
        {
            int count = 0;
            string msg;
            while (true)
            {
                try
                {
                    msg = fnc.Invoke();
                    break;
                }
                catch (Exception exc)
                {
                    if (count > 4)
                    {
                        return "Max retries exceeded";
                    }
                    Console.Write("\n  {0}", exc.Message);
                    Console.Write("\n  service failed {0} times - trying again", ++count);
                    Thread.Sleep(100);
                    continue;
                }
            }
            return msg;
        }
        void SendMessage(MessageProtocol msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded

            Func<string> fnc = () => { svc.SendMSG(new MessageProtocol { }); return "service succeeded"; };
            ServiceRetryWrapper(fnc);
        }

        string GetMessage()
        {
            MessageProtocol msg;
            Func<string> fnc = () => { msg = svc.GetMSG(); return "this does something"; };
            return ServiceRetryWrapper(fnc);
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


            client.SendMessage(msg);
            client.SendMessage(msg);
            client.SendMessage(msg);
            client.SendMessage(msg);
            client.SendMessage(msg);
            mmssgg = client.GetMessage();
            Console.Write("\n  Message recieved from Service: {0}\n\n", mmssgg);
        }
    }
}