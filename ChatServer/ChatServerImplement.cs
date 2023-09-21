using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseLib;
using InterfaceLib;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ChatServerImplement : ChatServerInterface
    {
        private static ChatServerImplement instance;
        private UserDatabase db;
        private ChatRoomDatabase roomList;

        private ChatServerImplement()
        {
            db = UserDatabase.GetInstance();
            roomList = ChatRoomDatabase.GetInstance();
        }

        public static ChatServerImplement GetInstance()
        {
            if (instance == null)
            {
                instance = new ChatServerImplement();
            }
            return instance;
        }

        public bool createChatRoom(string roomName)
        {
            try
            {
                if (roomList.CheckChatRoom(roomName) == false)
                {
                    roomList.AddChatRoom(roomName);
                    Console.WriteLine("Chat room added: " + roomName);
                    return true;
                }
                else
                {
                    throw new FaultException<ChatRoomAlreadyExistsFault>(new ChatRoomAlreadyExistsFault()
                    { ProblemType = "Chat room is invalid..." }, new FaultReason("Chat room name is taken!"));
                }
            }
            catch (FaultException<ChatRoomAlreadyExistsFault> e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


        }

        public void joinChatRoom(string roomName, string username)
        {
            ChatRoom room = roomList.GetRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(username);
        }

        public void leaveChatRoom(string roomName, string username)
        {
            ChatRoom room = roomList.GetRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(username);
        }

        public bool login(string username)
        {
            try
            {
                if (db.CheckUser(username) == false)
                {
                    db.AddUserByUsername(username);
                    Console.WriteLine("User added: " + username);
                    return true;
                }
                else
                {
                    throw new FaultException<UsernameNotValidFault>(new UsernameNotValidFault()
                    { ProblemType = "Username is invalid..." }, new FaultReason("Username is taken!"));
                }
            }
            catch (FaultException<UsernameNotValidFault> e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public List<string> GetChatRoomNamesList()
        {
            return roomList.GetRoomList().Select(room => room.GetRoomName()).ToList();
        }

        public void SendMessage(string message, string roomName, string username)
        {
            ChatRoom tempRoom = null;

            foreach (ChatRoom room in roomList.GetRoomList())
            {
                if (room.GetRoomName() == roomName)
                {
                    tempRoom = room;
                }
            }
            tempRoom.AddMessage(username + ": " + message);
        }

        public ChatRoom FindChatRoom(string roomName)
        {
            foreach (ChatRoom room in roomList.GetRoomList())
            {
                if (room.GetRoomName().Equals(roomName))
                {
                    return room;
                }
            }
            return null;

        }

        public void logout(User user)
        {
            throw new NotImplementedException();
        }
    }
}
