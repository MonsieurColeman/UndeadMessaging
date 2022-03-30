/*
 This file holds the Client, the heart of the application.
The Client is an intermediary between the GUI and the Service 
 */

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
        public static string _username = "";
        public static string _usernameColor = GlobalStrings.color_black;
        public static string _profilePicture = GlobalStrings.lipsum_Image;

        #region ServiceSetup
        public static void StartClientBehavior()
        {
            EstablishConnectionWithServer(LoginView._url);
            HostARecieveChannel();
        }
        public static void EstablishConnectionWithServer(string serverEndPoint)
        /* Connects to server's service*/
        {
            _serverEndpoint = serverEndPoint; //set class varible
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(serverEndPoint);
            ChannelFactory<IBasicService> factory = new ChannelFactory<IBasicService>(binding, address);
            serverService = factory.CreateChannel(); //returns an object of that service  
        }

        public static void HostARecieveChannel()
        /* Hosts a Peer Service and sets a thread to wait at the respective queue */
        {
            //create channel and set endpoint
            StartListening();

            //Start thread to listen to inbound blocking queue to process jobs
            Thread rvcThread = new Thread(ReceiveThreadProc);

            //rvcThread.IsBackground = true;
            rvcThread.Start();
        }

        private static void ReceiveThreadProc()
        /* Sets a thread to wait at the incoming blocking queue
           which will call the main thread to update the ui when it receives
           an order */
        {
            MessageProtocol job;
            while (true)
            {
                job = _IncomingQueue.deQ();
                if (job.messageProtocolType == MessageType.endQueue)
                    continue;
                Application.Current.Dispatcher.Invoke(() => RequestHandler.ProcessJobRequeust(job));
            }
        }

        private static void CreateRecvChannel(string address)
        /* Creates a channel to listen to at the given address of Peer type service */
        {
            WSHttpBinding binding = new WSHttpBinding();
            Uri baseAddress = new Uri(address);
            hostingService = new ServiceHost(typeof(Peer), baseAddress);
            hostingService.AddServiceEndpoint(typeof(IPeer), binding, baseAddress);
            hostingService.Open();
        }

        static void StartListening()
        /* Queries various ports for an open port to host a channel on */
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
                    Console.WriteLine(ex.Message);
                }
            }

        }
        #endregion

        public static bool JoinServer(string username, string usernameColor, string imageSource = "")
        // Sends a message to the Server's service indicating our intention to join
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
                messageProtocolType = MessageType.join,
                destinationEndpoint = _serverEndpoint
            };

            //Send Message To Server
            return serverService.Join(msg, userProfile);
        }

        public static void EstablishConnectionWithUser(string endpoint)
        //* Attempts to connect to a user's peer service based on given endpoint *//
        {
            WSHttpBinding binding = new WSHttpBinding();
            EndpointAddress address = new EndpointAddress(endpoint);
            ChannelFactory<IPeer> factory = new ChannelFactory<IPeer>(binding, address);
            _PeerService = factory.CreateChannel(); //returns an object of that service  
        }

        public static bool SendPrivateMessage(UserModel user, string message)
        // Connects to User by thier endpoints and delivers a message to their Peer Service
        {
            EstablishConnectionWithUser(user.Endpoint);
            try
            {
                _PeerService.SendMSG(CreateMsgProto(user.Endpoint, MessageType.privateMessage, message));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool SendMessageToTopic(TopicModel topic, MessageModel msg)
        // Delivers message to server's service to post to topic
        {
            try
            {
                serverService.MsgTopic(CreateMsgProto(_serverEndpoint, MessageType.topicMsg), msg, topic);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CreateTopic(string topicName)
        // Indicate to server's service via msg protocol that we wish to create a topic
        {
            return serverService.CreateTopic(
                CreateMsgProto(_serverEndpoint, MessageType.topicCreate),
                new TopicModel()
                {
                    ChatName = topicName,
                    Username = topicName,
                    TopicName = topicName
                });
        }

        public static void LeaveTopic(string topicName)
        //Delivers a msg to through the Server's Service indicating a desire to leave given topic
        {
            serverService.LeaveTopic(
                CreateMsgProto(_serverEndpoint, MessageType.leaveTopic, topicName),
                myUserModel);
            return;
        }

        public static void ShutdownChat(ObservableCollection<UserModel> Userlist)
        /* Broadcasts userLeft msg and shutdowns blocking queue */
        {
            //Tell every one that i am done talking
            UserModel user;
            for (int i = 0; i < Userlist.Count; i++)
            {
                if (Userlist[i] is TopicModel topic)
                    continue;
                user = Userlist[i];
                EstablishConnectionWithUser(user.Endpoint);
                _PeerService.UserLeft(myUserModel);
            }

            //tell my thread to stop working
            _IncomingQueue.enQ(CreateMsgProto(_myEndpoint, MessageType.endQueue));

            //tell server that it's over between us
            serverService.Leave(CreateMsgProto(_serverEndpoint,MessageType.userLeft), myUserModel);
        }

        #region Helper Functions
        public static UserModel SetMyUserModel(string username, string usernameColor, string imageSource = "")
        //Sets class variable
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
        //A function for other classes to get application's usermodel
        {
            return myUserModel;
        }

        public static string GetMyEndpoint()
        //A function for other classes to get application's endpoint
        {
            return _myEndpoint;
        }

        private static MessageProtocol CreateMsgProto(string destEndpoint, MessageType msgType, string msg="", dynamic filler=null)
        //A helper function to follow the D-R-Y principle
        {
            return new MessageProtocol()
            {
                destinationEndpoint = destEndpoint,
                messageBody = msg,
                messageFiller = filler,
                messageProtocolType=msgType,
                sourceEndpoint = _myEndpoint
            };
        }
        #endregion
    }
}

/*
 Maintenance History

0.3 File created to be the intermediary between service implementation and display view model
0.5 Added thread handling and blocking queue
0.6 Added core application variables to be reference by other classes
0.8 Finished added functionality for topics
1.0 Refactored code and added topics

 */