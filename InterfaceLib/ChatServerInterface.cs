using DatabaseLib;
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
        void createChatRoom(string roomName);

        [OperationContract]
        void logout(User username);

        [OperationContract]
        void setCurrentUser(User username);

        [OperationContract]
        User getCurrentUser();
    }
}
