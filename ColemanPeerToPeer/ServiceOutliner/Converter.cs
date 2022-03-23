using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace ServiceOutliner
{
    public static class Converter
    {
        public static string MessageProtocolToJSON(MessageProtocol mP)
        {
            //use extension
            var serializer = new JavaScriptSerializer();

            //serialize
            var serializedResult = serializer.Serialize(mP);

            //return
            return serializedResult;
        }

        public static MessageProtocol MessageProtocolToJSON(string JSON)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedResult = serializer.Deserialize<MessageProtocol>(JSON);
            return deserializedResult;
        }
    }
}
