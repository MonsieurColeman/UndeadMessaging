using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace ColemanPeerToPeer.Service
{
    [ServiceContract]
    public interface IPeer
    {
        [OperationContract]
        void SendMSG(MessageProtocol msg);


        [OperationContract]
        MessageProtocol GetMSG();
    }
}
