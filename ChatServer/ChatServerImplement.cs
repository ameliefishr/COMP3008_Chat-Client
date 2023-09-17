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

        public void login(string username)
        {
            if(db.CheckUser(username) == false)
            {
                db.AddUser(username);
            }
        }
    }
}
