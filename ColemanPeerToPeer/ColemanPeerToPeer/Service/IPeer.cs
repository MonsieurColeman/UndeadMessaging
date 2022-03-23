using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace ColemanPeerToPeer.Service
{
    [ServiceContract]
    public interface IBasicService
    {
        [OperationContract]
        void SendMSG(MessageProtocol msg);

        [OperationContract]
        MessageProtocol GetMSG();

        [OperationContract]
        bool Join(MessageProtocol msg);

        [OperationContract]
        void SendComplicatedMsg(string msg);

        [OperationContract]
        bool JoinComplicated(string m);
    }
}
