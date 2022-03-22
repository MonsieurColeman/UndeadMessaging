using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ColemanPeerToPeer.Service
{
    class Peer
    {
        IBasicService svc;

        /*
         * Create a communication channel to converse with server
         * using contract IBasicService.
         * 
         * Channel doesn't attempt to connect to server
         * until first service call.
         */
        public Peer(string url)
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
            /*
            //Func<string> fnc = () => { svc.SendMSG(Converter.MessageProtocolToJSON(msg)); return "service succeeded"; };
            string code = ServiceRetryWrapper(fnc);
            */
            //Func<string> fnc = () => { svc.SendMSG("testMessage"); return "service succeeded"; };
            Func<string> fnc = () => { svc.TestMSG(msg); return "service succeeded"; };
            string code = ServiceRetryWrapper(fnc);
        }

        public string GetMessage()
        {
            MessageProtocol msg;
            //Func<string> fnc = () => { msg = svc.GetMSG(); return "something something something"; };
            //return ServiceRetryWrapper(fnc);
            return "";
        }
    }
}
