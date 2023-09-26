using System.Runtime.Serialization;

namespace DatabaseLib
{
    // chat message class for our chat message objects
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        // set/gets message text
        public string MessageText { get; set; }
      
        [DataMember]
        // sets/gets message type
        public MessageType MessageType { get; set; }

        // gets the message text
        public string getMessage()
        {
            return MessageText;
        }
    }

    // enum to set the message type to text or file
    public enum MessageType
    {
        Text,
        File
    }

}