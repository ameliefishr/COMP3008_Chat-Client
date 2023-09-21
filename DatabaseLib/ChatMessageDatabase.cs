using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class ChatMessageDatabase
    {
        private List<ChatPrivateMessage> msgs;
        private static ChatMessageDatabase instance;
        private ChatMessageDatabase()
        {
            msgs = new List<ChatPrivateMessage>();
        }

        public static ChatMessageDatabase GetInstance()
        {
            if(instance == null)
            {
                instance = new ChatMessageDatabase();
            }
            return instance;
        }

        public void addPrivateMessage(string sender, string recipient, ChatMessage message)
        {
            ChatPrivateMessage chat = new ChatPrivateMessage(sender, recipient, message);
            msgs.Add(chat);
        }

        public ChatPrivateMessage getPrivateMessage(string sender, string recipient, ChatMessage message)
        {
            foreach(ChatPrivateMessage chat in msgs)
            {
                if(chat.Sender == sender && chat.Recipient == recipient)
                {
                    return chat;
                }
            }
            return null;
        }
    }
}
