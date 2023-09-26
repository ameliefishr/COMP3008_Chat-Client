using System.Collections.Generic;

namespace DatabaseLib
{
    // database to store all of the private chat messages
    public class ChatMessageDatabase
    {
        private List<ChatPrivateMessage> msgs;
        private static ChatMessageDatabase instance;
        private ChatMessageDatabase()
        {
            msgs = new List<ChatPrivateMessage>();
        }

        // returns the full database
        public static ChatMessageDatabase GetInstance()
        {
            if(instance == null)
            {
                instance = new ChatMessageDatabase();
            }
            return instance;
        }

        // add's a private message to the database
        public void addPrivateMessage(string sender, string recipient, ChatMessage message)
        {
            ChatPrivateMessage chat = new ChatPrivateMessage(sender, recipient, message);
            msgs.Add(chat);
        }

        // gets a private message from the database
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
