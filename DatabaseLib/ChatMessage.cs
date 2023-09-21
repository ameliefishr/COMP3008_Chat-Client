using System;
using System.Runtime.Serialization;

namespace DatabaseLib
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public MessageType MessageType { get; set; }

        public string getMessage()
        {
            return MessageText;
        }
    }

    public enum MessageType
    {
        Text,
        File
    }

}