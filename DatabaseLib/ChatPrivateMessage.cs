
namespace DatabaseLib
{
    // class for a private chat message object
    // unlike normal chat message it keeps track of the sender and receiver
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
