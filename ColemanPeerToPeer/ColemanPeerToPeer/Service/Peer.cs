using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
            Func<string> fnc = () => { svc.SendMSG(msg); return "service succeeded"; };
            string code = ServiceHandler.AttemptService(fnc);
        }

        public bool JoinServer(string username, string usernameColor, string myEndpoint = "me", string imageSource = "")
        {
            UserModel userProfile = new UserModel()
            {
                Username = username,
                ImageSource = imageSource,
                UsernameColor = usernameColor,
                Endpoint = myEndpoint,
            };

            string s = new JavaScriptSerializer().Serialize(userProfile);
            s = "a";

            MessageProtocol msg = new MessageProtocol()
            {
                sourceEndpoint = myEndpoint,
                messageBody = username,
                //messageFiller = s,
                messageProtocolType = MessageType.userLeft,
                destinationEndpoint = "Server"
            };

            //UserModel userProfile2 = new JavaScriptSerializer().Deserialize<UserModel>(msg.messageFiller);

            if (svc != null)
                return svc.Join(msg);
            return false;
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
