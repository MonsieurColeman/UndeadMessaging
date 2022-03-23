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

        public void GetAnEndPoint()
        {

        }

        void startListening()
        {
            int localPort = 4040;
            while (true)
            {
                try
                {
                    string endpoint = "http://localhost:" + localPort + "/ICommunicator";
                    CreateRecvChannel(endpoint);
                    // create receive thread which calls rcvBlockingQ.deQ() (see ThreadProc above)


                    OnListenerCreated.Invoke(endpoint);
                    return;
                }
                catch (Exception ex)
                {
                    localPort++;
                    if ((localPort - 4040) > 10000)
                    {
                        OnListenerCreated.Invoke("");
                        return;
                    }
                    Console.WriteLine(ex.Message);
                }
            }

        }




















        public void SendMessage(MessageProtocol msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded
            Func<string> fnc = () => { svc.SendMSG(msg); return "service succeeded"; };
            string code = ServiceHandler.AttemptService(fnc);
        }

        /* 
         * This function is responsible for setting up a connection relationship with the server
         */
        public bool JoinServer(string username, string usernameColor, string myEndpoint = "me", string imageSource = "")
        {
            //Perform Service Check
            if (svc == null)
            {
                MessageBox.Show("Login Service Experienced an error");
                return false;
            }

            //Create a usermodel to be passed to other clients
            UserModel userProfile = new UserModel()
            {
                Username = username,
                ImageSource = imageSource,
                UsernameColor = usernameColor,
                Endpoint = myEndpoint,
            };

            //Create Protocol for service
            MessageProtocol msg = new MessageProtocol()
            {
                sourceEndpoint = myEndpoint,
                messageBody = username,
                messageFiller = new JavaScriptSerializer().Serialize(userProfile),
                messageProtocolType = MessageType.join,
                destinationEndpoint = "Server"
            };

            //Send Message To Server
            return svc.Join(msg);
        }

        public string GetMessage()
        {
            MessageProtocol msg;
            return "";
        }
    }
}
