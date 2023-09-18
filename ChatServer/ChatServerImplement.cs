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
        private UserDatabase db;
        private List<ChatRoom> roomList;
  

        public ChatServerImplement()
        {
            db = new UserDatabase();
            roomList = new List<ChatRoom>();
        }

        public bool createChatRoom(string roomName)
        {
            try
            {
                ChatRoom room = new ChatRoom(roomName);
                foreach (ChatRoom cRoom in roomList)
                {
                    if (cRoom.GetRoomName().Equals(roomName))
                    {
                        throw new FaultException<ChatRoomAlreadyExistsFault>(new ChatRoomAlreadyExistsFault()
                        { ProblemType = "Chat room name is taken" }, new FaultReason("Chat room name is taken"));
                    }
                    
                }
                roomList.Add(room);
                return true;
            }
            catch (FaultException<ChatRoomAlreadyExistsFault> e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


        }

        public void joinChatRoom(string roomName, string username)
        {
            ChatRoom room = roomList.Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(username);
        }

        public void leaveChatRoom(string roomName, string username)
        {
            ChatRoom room = roomList.Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(username);
        }

        public bool login(string username)
        {
            try
            {
                if (db.CheckUser(username) == false)
                {
                    db.AddUserByUsername(username);
                    Console.WriteLine("User added: "+ username);
                    return true;
                }
                else
                {
                    throw new FaultException<UsernameNotValidFault>(new UsernameNotValidFault()
                    { ProblemType = "Username is invalid..." }, new FaultReason("Username is taken!"));
                }
            }
            catch (FaultException<UsernameNotValidFault> e)
            { Console.WriteLine(e.Message);
                return false;
            }
        }
        public List<string> GetChatRooms()
        {
            return roomList.Select(room => room.GetRoomName()).ToList();
        }
    }
}
