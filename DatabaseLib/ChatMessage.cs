using System;
using System.Runtime.Serialization;

namespace DatabaseLib
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string SenderUsername { get; set; } 

        [DataMember]
        public string Content { get; set; } 

        [DataMember]
        public DateTime Timestamp { get; set; } 

        public ChatMessage(string senderUsername, string content)
        {
            SenderUsername = senderUsername;
            Content = content;
            Timestamp = DateTime.Now; // sets to curent time/date
        }
    }
}