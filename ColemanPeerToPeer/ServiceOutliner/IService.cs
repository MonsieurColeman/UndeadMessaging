

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOutliner
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
        bool JoinBetter(MessageProtocol msg, UserModel userProfile);

        [OperationContract]
        void Leave(MessageProtocol msg, UserModel userProfile);

        [OperationContract]
        void SendComplicatedMsg(string msg);

        [OperationContract]
        bool JoinComplicated(string m);

        [OperationContract]
        bool TestMessage(MessageProtocol m);

        [OperationContract]
        void LeaveTopic(MessageProtocol msg, UserModel user);

        [OperationContract]
        void MsgTopic(MessageProtocol msg, MessageModel msgModel, TopicModel topic);

        [OperationContract]
        bool CreateTopic(MessageProtocol msg, TopicModel topic);
    }
}
