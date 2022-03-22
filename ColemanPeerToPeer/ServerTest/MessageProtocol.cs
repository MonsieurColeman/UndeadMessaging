using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week7WCF
{
    public class MessageProtocol
    {
        public string sourceEndpoint;
        public MessageType messageProtocolType;
        public dynamic messageBody;
        public dynamic messageFiller;
        public string destinationEndpoint;
    }

    public enum MessageType
    {
        //server related
        join,
        receiveCurrentUsersOnJoin,
        receiveCurrentTopicsOnJoin,

        //dash related
        userJoined,
        userLeft,
        topicCreate,
        topicEnded,

        //personal
        privateMessage,
        topicMsg,
        leaveTopic,
    }
    

}
