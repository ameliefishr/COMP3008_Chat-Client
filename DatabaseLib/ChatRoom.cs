using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class ChatRoom
    {
        private List<String> users;
        private string roomName;

        public ChatRoom(String roomName)
        {
            this.roomName = roomName;
            users = new List<String>();
        }

        public string GetRoomName() 
        { 
            return roomName; 
        }

        public void AddToRoom(String username)
        {
            users.Add(username);
        }

        public void RemoveFromRoom(String username)
        {
            users.Remove(username);
        }

        public Boolean CheckInRoom(String roomName)
        {
            return users.Contains(roomName);
        }

    }
}
