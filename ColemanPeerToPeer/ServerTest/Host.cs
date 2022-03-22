using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Week7WCF
{
    class Host //server
    {
        static ServiceHost CreateChannel(string url)
        {
            WSHttpBinding binding = new WSHttpBinding(); //provide binding to clients | its a handshake to agree on protocols and security
            Uri address = new Uri(url); //endpoint
            Type service = typeof(BasicService); //the type of the serviceClass
            ServiceHost host = new ServiceHost(service, address); //create host object
            host.AddServiceEndpoint(typeof(IBasicService), binding, address); //connects the service to the endpoint address with the communication protocols binded
            return host;
        }
        static void Main(string[] args)
        {
            Console.Title = "BasicHttp Service Host";
            Console.Write("\n  Starting Programmatic Basic Service");
            Console.Write("\n =====================================\n");

            ServiceHost host = null;
            try
            {
                host = CreateChannel("http://localhost:8080/BasicService");
                host.Open(); //instantiates object to receive communication
                Console.Write("\n  Started BasicService - Press key to exit:\n");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}\n\n", ex.Message);
                return;
            }
            host.Close(); //destroys object
        }
    }
}
