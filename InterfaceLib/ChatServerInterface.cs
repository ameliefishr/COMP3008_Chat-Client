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
        void joinChatRoom(string roomName, User user);

        [OperationContract]
        void leaveChatRoom(string roomName, User user);

        [OperationContract]
        bool createChatRoom(string roomName);

        [OperationContract]
        List<string> GetChatRooms();

        [OperationContract]
        void SendMessage(string message, string roomName, string username);

        [OperationContract]
        ChatRoom FindChatRoom(string roomName);

        [OperationContract]
        void logout(User username);

        [OperationContract]
        void setCurrentUser(User username);

        [OperationContract]
        User getCurrentUser();
    }
}
