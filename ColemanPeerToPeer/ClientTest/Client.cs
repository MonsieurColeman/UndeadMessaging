using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;
using Week7WCF;

namespace ServiceClient
{
    class Client
    {
        IBasicService svc; //the service, which gets defined on client creatation

        /*
         * Create a communication channel to converse with server
         * using contract IBasicService.
         * 
         * Channel doesn't attempt to connect to server
         * until first service call.
         */
        Client(string url)
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
        void SendMessage(string msg)
        {
            // input string is captured in body of functor
            // Func<string> return string is discarded

            Func<string> fnc = () => { svc.sendMessage(msg); return "service succeeded"; };
            ServiceRetryWrapper(fnc);
        }

        string GetMessage()
        {
            string msg;
            Func<string> fnc = () => { msg = svc.getMessage(); return msg; };
            return ServiceRetryWrapper(fnc);
        }

        static void Main(string[] args)
        {
            Console.Title = "BasicHttp Client";
            Console.Write("\n  Starting Programmatic Basic Service Client");
            Console.Write("\n ============================================\n");
            string msg = "a";
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

            RemoteFileInfo
                   uploadRequestInfo = new RemoteFileInfo();

            using (System.IO.FileStream stream =
                   new System.IO.FileStream("./test.zip",
                   System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                uploadRequestInfo.FileName = "./test.zip";
                uploadRequestInfo.Length = fileInfo.Length;
                uploadRequestInfo.FileByteStream = stream;
                client.svc.UploadFile(uploadRequestInfo);
                //clientUpload.UploadFile(stream);
            }

        }
    }
}
