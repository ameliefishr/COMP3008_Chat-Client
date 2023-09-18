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

        public void createChatRoom(string roomName)
        {
            ChatRoom room = new ChatRoom(roomName);
            roomList.Add(room);
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
                    db.AddUser(username);
                    Console.WriteLine("User added");
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
    }
}
