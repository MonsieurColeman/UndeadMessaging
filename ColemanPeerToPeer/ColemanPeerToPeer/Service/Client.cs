using ServiceOutliner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Threading;

namespace ColemanPeerToPeer.Service
{
    public static class Client
    {

        public static IBasicService serverService;
        public static IPeer _PeerService;
        public static ServiceHost hostingService = null;
        public static BlockingQueue<MessageProtocol> _IncomingQueue = new BlockingQueue<MessageProtocol>();
        private static string _myEndpoint = ""; //gets set by ctor functions
        public static string _serverEndpoint = ""; //gets set by ctor functions
        private static UserModel myUserModel = null;

        public static void StartClientBehavior()
        {
            EstablishConnectionWithServer(LoginView._url);
            HostARecieveChannel();
        }

        public static void EstablishConnectionWithServer(string serverEndPoint)
        {
            _serverEndpoint = serverEndPoint; //set class varible
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(serverEndPoint);
            ChannelFactory<IBasicService> factory = new ChannelFactory<IBasicService>(binding, address);
            serverService = factory.CreateChannel(); //returns an object of that service  
        }

        public static void HostARecieveChannel()
        {
            StartListening(); //create channel and set endpoint

            //Start thread to listen to inbound blocking queue to process jobs
            Thread rvcThread = new Thread(ReceiveThreadProc);
            //rvcThread.IsBackground = true;
            rvcThread.Start();
        }

        private static void ReceiveThreadProc()
        {
            MessageProtocol job;
            while (true)
            {
                job = _IncomingQueue.deQ();
                Application.Current.Dispatcher.Invoke(() => RequestHandler.ProcessJobRequeust(job));
            }
        }

        private static void CreateRecvChannel(string address)
        {
            WSHttpBinding binding = new WSHttpBinding();
            Uri baseAddress = new Uri(address);
            hostingService = new ServiceHost(typeof(Peer), baseAddress);
            hostingService.AddServiceEndpoint(typeof(IPeer), binding, baseAddress);
            hostingService.Open();
        }

        static void StartListening()
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


        /* 
 * This function is responsible for setting up a connection relationship with the server
 */
        public static bool JoinServer(string username, string usernameColor, string imageSource = "")
        {
            //Get endpoint from instance variable
            string myEndpoint = _myEndpoint;

            //Perform Service Check
            if (serverService == null)
            {
                MessageBox.Show("Login Service Experienced an error");
                return false;
            }

            //Create a usermodel to be passed to other clients
            UserModel userProfile = SetMyUserModel(username, usernameColor, imageSource);

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

        public static void EstablishConnectionWithUser(string endpoint)
        {
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(endpoint);
            ChannelFactory<IPeer> factory = new ChannelFactory<IPeer>(binding, address);
            _PeerService = factory.CreateChannel(); //returns an object of that service  
        }

        public static bool SendPrivateMessage(UserModel user, string message)
        {
            EstablishConnectionWithUser(user.Endpoint);
            try
            {
                _PeerService.SendMSG(new MessageProtocol()
                {
                    sourceEndpoint = _myEndpoint,
                    messageBody = message,
                    messageProtocolType = MessageType.privateMessage,
                    destinationEndpoint = user.Endpoint
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendMessageToTopic(TopicModel topic, MessageModel msg)
        {
            /*
            try
            {*/
                serverService.MsgTopic(new MessageProtocol()
                {
                    sourceEndpoint = _myEndpoint,
                    messageProtocolType = MessageType.topicMsg,
                    destinationEndpoint = _serverEndpoint
                },
                msg, topic);
                return true; /*
            }
            catch (Exception ex)
            {
                return false;
            }*/
        }

        public static bool CreateTopic(string topicName)
        {
            return serverService.CreateTopic(new MessageProtocol()
            {
                sourceEndpoint= _myEndpoint,
                messageProtocolType= MessageType.topicCreate,
                destinationEndpoint = _serverEndpoint
            },
            new TopicModel()
            {
                ChatName = topicName,
                Username = "placeholder"
            });
        }

        public static void LeaveTopic(string topicName)
        {
            serverService.LeaveTopic(new MessageProtocol()
            {
                sourceEndpoint = _myEndpoint,
                messageBody = topicName,
                messageProtocolType = MessageType.leaveTopic,
                destinationEndpoint = _serverEndpoint
            },
            myUserModel
            );
            return;
        }

        public static void ShutdownChat(ObservableCollection<UserModel> Userlist)
        {
            UserModel user;
            for (int i = 0; i < Userlist.Count; i++)
            {
                if (Userlist[i] is TopicModel topic)
                    continue;
                user = Userlist[i];
                EstablishConnectionWithUser(user.Endpoint);
                _PeerService.UserLeft(myUserModel);
            }
            serverService.Leave(new MessageProtocol()
            {
                sourceEndpoint = _myEndpoint,
                messageProtocolType = MessageType.userLeft,
                destinationEndpoint = _serverEndpoint
            }, myUserModel);
        }

        public static UserModel SetMyUserModel(string username, string usernameColor, string imageSource = "")
        {
            UserModel userProfile = new UserModel()
            {
                Username = username,
                ImageSource = imageSource,
                UsernameColor = usernameColor,
                Endpoint = _myEndpoint,
            };
            myUserModel = userProfile;
            return userProfile;
        }

        public static UserModel GetMyUserModel()
        {
            return myUserModel;
        }

        public static string GetMyEndpoint()
        {
            return _myEndpoint;
        }
    }
}
