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
        bool createChatRoom(string roomName);

        [OperationContract]
        List<string> GetChatRoomNamesList();

        [OperationContract]
        void SendMessage(ChatMessage message, string roomName, string username);

        [OperationContract]
        List<string> GetChatRoomMessage(string roomName);

        [OperationContract]
        ChatRoom FindChatRoom(string roomName);

        [OperationContract]
        void logout(User user);
    }
}
