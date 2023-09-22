using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    [DataContract]
    public class ChatRoom
    {
        [DataMember]
        private List<String> users;

        [DataMember]
        private List<ChatMessage> messages;

        [DataMember]
        private string roomName;
        private RoomType roomType;

        public ChatRoom(String roomName, RoomType roomType)
        {
            this.roomName = roomName;
            users = new List<String>();
            messages = new List<ChatMessage>();
            this.roomType = roomType;
        }

        public enum RoomType
        {
            Public,
            Private
        }

        public string GetRoomName()
        {
            return roomName;
        }

        public RoomType GetRoomType()
        {
            return roomType;
        }

        public void AddToRoom(String username)
        {
            users.Add(username);
        }

        public void RemoveFromRoom(String username)
        {
            users.Remove(username);
        }

        public Boolean CheckInRoom(String username)
        {
            return users.Contains(username);
        }

        public void AddMessage(ChatMessage chatMessage)
        {
            messages.Add(chatMessage);
        }

        public List<ChatMessage> GetMessage()
        {
            return messages;
        }

        public List<String> GetUsers()
        {
            return users;
        }
    }
}
