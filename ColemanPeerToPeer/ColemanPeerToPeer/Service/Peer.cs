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

        public void SendMessage(MessageProtocol msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded
            Func<string> fnc = () => { svc.TestMSG(msg); return "service succeeded"; };
            string code = ServiceHandler.AttemptService(fnc);
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
