using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class ChatRoom
    {
        private List<User> users;
        private string roomName;

        public ChatRoom(String roomName)
        {
            this.roomName = roomName;
            users = new List<User>();
        }

        public string GetRoomName() 
        { 
            return roomName; 
        }

        public void AddToRoom(User user)
        {
            users.Add(user);
        }

        public void RemoveFromRoom(User user)
        {
            users.Remove(user);
        }

        public Boolean CheckInRoom(String username)
        {
            for (int i = 0; i < users.Size(); i++)
            return users.Contains(username);
        }
    }
}
