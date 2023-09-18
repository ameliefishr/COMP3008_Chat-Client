using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        List<string> GetChatRooms();

        [OperationContract]
        void SendMessage(string message, string roomName, string username);

        [OperationContract]
        ChatRoom FindChatRoom(string roomName);
    }
}
