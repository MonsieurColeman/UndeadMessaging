using ColemanPeerToPeer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ColemanPeerToPeer
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        MainWindow _MainWindow;
        static string url = "http://localhost:8080/BasicService";
        Peer peer = new Peer(url);

        public LoginView(MainWindow main)
        {
            InitializeComponent();
            _MainWindow = main;
        }

        private void Btn_Login(object sender, RoutedEventArgs e)
        {
            PerformLogin();
            _MainWindow.loginSuccessful = true;
            this.Close();
        }


        private void PerformLogin()
        {
            peer.SendMessage(GetJoinMessage());
        }

        private MessageProtocol GetJoinMessage()
        {
            return new MessageProtocol()
            {
                sourceEndpoint = url,
                messageProtocolType = MessageType.join,
                messageBody = "thisIsATest",
                destinationEndpoint = "server"
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("oo");
        }

        /*
        static void Main(string[] args)
        {
            Console.Title = "BasicHttp Client";
            Console.Write("\n  Starting Programmatic Basic Service Client");
            Console.Write("\n ============================================\n");
            string msg = args[0];
            string url = "http://localhost:8080/BasicService";
            Client client = new Client(url);


            client.SendMessage(msg);
            client.SendMessage(msg);
            client.SendMessage(msg);
            client.SendMessage(msg);
            client.SendMessage(msg);
            msg = client.GetMessage();
            Console.Write("\n  Message recieved from Service: {0}\n\n", msg);

            System.IO.FileInfo fileInfo =
               new System.IO.FileInfo("./test.zip");

        }
        */
    }
}
