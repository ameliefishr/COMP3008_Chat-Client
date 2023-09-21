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
        private List<ChatMessage> msgs;
        private static ChatMessageDatabase instance;
        private ChatMessageDatabase()
        {
            msgs = new List<ChatMessage>();
        }

        public static ChatMessageDatabase GetInstance()
        {
            if(instance == null)
            {
                instance = new ChatMessageDatabase();
            }
            return instance;
        }

        public void addPrivateMessage(string sender, string recipient, string message)
        {
            ChatMessage chat = new ChatMessage(sender, recipient, message);
            msgs.Add(chat);
        }

        public ChatMessage getPrivateMessage(string sender, string recipient, string message)
        {
            foreach(ChatMessage chat in msgs)
            {
                if(chat.SenderUsername == sender && chat.RecipientUsername == recipient)
                {
                    return chat;
                }
            }
            return null;
        }
    }
}
