using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class ChatPrivateMessage
    {
        public string Sender { get; }
        public string Recipient { get; }
        public ChatMessage Message { get; }

        public ChatPrivateMessage(string sender, string recipient, ChatMessage message)
        {
            Sender = sender;
            Recipient = recipient;
            Message = message;
        }
    }
}
