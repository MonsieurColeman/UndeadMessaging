using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    class Peer : IPeer
    {
        IBasicService serverService;
        ServiceHost hostingService = null;
        public static BlockingQueue<MessageProtocol> _IncomingQueue = new BlockingQueue<MessageProtocol>();
        string _myEndpoint = ""; //gets set by ctor functions
        string _serverEndpoint = ""; //gets set by ctor functions


        /*
         * Create a communication channel to converse with server
         * using contract IBasicService.
         * 
         * Channel doesn't attempt to connect to server
         * until first service call.
         */
        public Peer()
        {
            EstablishConnectionWithServer(LoginView.url);
            HostARecieveChannel();
        }

        public void EstablishConnectionWithServer(string serverEndPoint)
        {
            _serverEndpoint = serverEndPoint; //set class varible
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(serverEndPoint);
            ChannelFactory<IBasicService> factory = new ChannelFactory<IBasicService>(binding, address);
            serverService = factory.CreateChannel(); //returns an object of that service  
        }

        public void HostARecieveChannel()
        {
            StartListening(); //create channel and set endpoint

            //Start thread to listen to inbound blocking queue to process jobs
            Thread rvcThread = new Thread(ReceiveThreadProc);
            //rvcThread.IsBackground = true;
            rvcThread.Start();
        }

        private void ReceiveThreadProc()
        {
            MessageProtocol job;
            while (true)
            {
                job = _IncomingQueue.deQ();
                RequestHandler.ProcessJobRequeust(job);
            }
        }

        private void CreateRecvChannel(string address)
        {
            WSHttpBinding binding = new WSHttpBinding();
            Uri baseAddress = new Uri(address);
            hostingService = new ServiceHost(typeof(Peer), baseAddress);
            hostingService.AddServiceEndpoint(typeof(IPeer), binding, baseAddress);
            hostingService.Open();
        }

        void StartListening()
        {
            int localPort = 4040;
            while (true)
            {
                try
                {
                    string endpoint = "http://localhost:" + localPort + "/IPeer";
                    CreateRecvChannel(endpoint);
                    _myEndpoint = endpoint;
                    return;
                }
                catch (Exception ex)
                {
                    localPort++;
                    if ((localPort - 4040) > 10000)
                    {
                        return;
                    }
                    Console.WriteLine(ex.Message + "ooga booga");
                }
            }

        }




















        public void SendMessage(MessageProtocol msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded
            Func<string> fnc = () => { serverService.SendMSG(msg); return "service succeeded"; };
            string code = ServiceHandler.AttemptService(fnc);
        }

        /* 
         * This function is responsible for setting up a connection relationship with the server
         */
        public bool JoinServer(string username, string usernameColor, string imageSource = "")
        {
            //Get endpoint from instance variable
            string myEndpoint = this._myEndpoint;
            
            //Perform Service Check
            if (serverService == null)
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
            return serverService.Join(msg);
        }

        public string GetMessage()
        {
            MessageProtocol msg;
            return "";
        }

        public void SendMSG(MessageProtocol msg)
        {
            throw new NotImplementedException();
        }

        public MessageProtocol GetMSG()
        {
            throw new NotImplementedException();
        }

        public void GetListOfUsers(ObservableCollection<UserModel> users)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = users,
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin
            };
            Peer._IncomingQueue.enQ(msg);
        }

        public void GetNewUser(UserModel newUser)
        {
            MessageProtocol msg = new MessageProtocol()
            {
                messageFiller = newUser,
                messageProtocolType = MessageType.receiveCurrentUsersOnJoin
            };
            Peer._IncomingQueue.enQ(msg);
        }
    }
}
