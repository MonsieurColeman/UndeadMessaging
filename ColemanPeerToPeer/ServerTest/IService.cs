using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Week7WCF
{
    /*
     * Service contract is used to define an interface which defines how 
     * -the clients will communicate with the service
     * -aka: what the requests are that they can send to the service
     */
    //[ServiceContract(SessionMode = SessionMode.Required)] for later
    [ServiceContract]
    public interface IBasicService
    {
        [OperationContract] void sendMessage(string msg);
        [OperationContract] string getMessage();
        [OperationContract] void UploadFile(RemoteFileInfo request);

        //[OperationContract(IsOneWay = true)] means that the client can return, we are not returning anything back
    }



    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream; //input iostream

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}