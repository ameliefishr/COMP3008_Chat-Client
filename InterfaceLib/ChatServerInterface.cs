using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace InterfaceLib
{
    [ServiceContract]
    public interface ChatServerInterface
    {
        [OperationContract]
        [FaultContract(typeof(UsernameNotValidFault))]
        bool login(string username);

        [OperationContract]
        void joinChatRoom(string roomName, string username);

        [OperationContract]
        void leaveChatRoom(string roomName, string username);

        [OperationContract]
        bool createPublicChatRoom(string roomName);

        [OperationContract]
        bool createPrivateChatRoom(string roomName);

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
        ChatRoom FindPrivateChatRoom(string roomName);

        [OperationContract]
        void logout(string username);
    }
}
