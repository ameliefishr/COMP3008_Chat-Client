using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DatabaseLib
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string SenderUsername { get; set; }

        [DataMember]
        public string RecipientUsername {  get; set; }

        [DataMember]
        public List<String> content;

        [DataMember]
        public DateTime Timestamp { get; set; } 

        public List<String> GetContent() { return content; }

        public void AddContent(string msg)
        {
            content.Add(msg);
        }

        public ChatMessage(string senderUsername, string recipientUsername, string msg)
        {
            SenderUsername = senderUsername;
            RecipientUsername = recipientUsername;
            content.Add(msg);
            Timestamp = DateTime.Now; // sets to curent time/date
        }
    }
}