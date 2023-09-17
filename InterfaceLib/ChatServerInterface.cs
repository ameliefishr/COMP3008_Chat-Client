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
        void login(string username);

        [OperationContract]
        void joinChatRoom(string roomName, string username);

        [OperationContract]
        void leaveChatRoom(string roomName, string username);

        [OperationContract]
        void createChatRoom(string roomName);
    }
}
