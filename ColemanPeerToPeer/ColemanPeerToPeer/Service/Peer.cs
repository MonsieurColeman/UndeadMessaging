using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColemanPeerToPeer.Service
{
    class Peer
    {
        IPeer svc;

        /*
         * Create a communication channel to converse with server
         * using contract IBasicService.
         * 
         * Channel doesn't attempt to connect to server
         * until first service call.
         */
        public Peer(string url)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress address = new EndpointAddress(url);
            ChannelFactory<IPeer> factory = new ChannelFactory<IPeer>(binding, address);
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
                try //invoke the function
                {
                    msg = fnc.Invoke();
                    break;
                }
                catch (Exception exc) //if you cant invoke (send message)
                {
                    if (count > 4)
                    {
                        return "Max retries exceeded";
                    } //say so
                    Console.Write("\n  {0}", exc.Message);
                    Console.Write("\n  service failed {0} times - trying again", ++count);
                    Thread.Sleep(100);
                    continue;
                }
            }
            return msg;
        }
        public void SendMessage(MessageProtocol msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded

            Func<string> fnc = () => { svc.SendMSG(msg); return "service succeeded"; };
            ServiceRetryWrapper(fnc);
        }

        public string GetMessage()
        {
            MessageProtocol msg;
            Func<string> fnc = () => { msg = svc.GetMSG(); return "something something something"; };
            return ServiceRetryWrapper(fnc);
        }
    }
}
