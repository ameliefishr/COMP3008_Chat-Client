using DatabaseLib;
using System.Collections.Generic;
using System.ServiceModel;

namespace InterfaceLib
{
    // interface for chat server
    [ServiceContract]
    public interface ChatServerInterface
    {
        [OperationContract]
        [FaultContract(typeof(UsernameNotValidFault))]
        bool login(string username);

        [OperationContract]
        void joinChatRoom(string roomName, string username);
        [OperationContract]
        void joinPrivateChatRoom(string senderName, string recipient);

        [OperationContract]
        void leaveChatRoom(string roomName, string username);

        [OperationContract]
        void leavePrivateChatRoom(string senderName, string recipient, string username);

        [OperationContract]
        bool createPublicChatRoom(string roomName);

        [OperationContract]
        bool createPrivateChatRoom(string sender, string recipient);

        [OperationContract]
        List<string> GetChatRoomNamesList();

        [OperationContract]
        void SendMessage(ChatMessage message, string chatRoom, string username);

        [OperationContract]
        void SendPrivateMessage(ChatMessage message, string chatRoom, string username);

        [OperationContract]
        List<ChatMessage> GetChatRoomMessage(string roomName);

        [OperationContract]
        ChatRoom FindPublicChatRoom(string roomName);

        [OperationContract]
        ChatRoom FindPrivateChatRoom(string sender, string recipient);

        [OperationContract]
        void logout(string username);
    }
}
