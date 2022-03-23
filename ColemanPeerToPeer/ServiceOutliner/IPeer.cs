using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOutliner
{
  
    [ServiceContract]
    public interface IPeer
    {
        [OperationContract]
        void SendMSG(MessageProtocol msg);

        [OperationContract]
        void GetListOfUsers(ObservableCollection<UserModel> users);

        /*
        [OperationContract]
        void SendListOfTopics(ObservableCollection<> users);
        */

        [OperationContract]
        void GetNewUser(UserModel newUser);

        [OperationContract]
        void UserLeft(UserModel newUser);


        [OperationContract]
        MessageProtocol GetMSG();
    }

}
