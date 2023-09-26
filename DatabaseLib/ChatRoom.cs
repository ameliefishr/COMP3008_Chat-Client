using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DatabaseLib
{
    // class for our chat room objects
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

        // enum to set the room type to eiter public or private
        public enum RoomType
        {
            Public,
            Private
        }

        // returns the name of the chat room
        public string GetRoomName()
        {
            return roomName;
        }

        // returns the type of the chat room
        public RoomType GetRoomType()
        {
            return roomType;
        }

        // add's a user to the chat room by their username
        public void AddToRoom(String username)
        {
            users.Add(username);
        }

        // removes a user from the chat room by their username
        public void RemoveFromRoom(String username)
        {
            users.Remove(username);
        }

        // check's if a user is in the chat room by their useranme
        public Boolean CheckInRoom(String username)
        {
            return users.Contains(username);
        }

        // add's a message to the chat room
        public void AddMessage(ChatMessage chatMessage)
        {
            messages.Add(chatMessage);
        }

        // gets the messages in the chat room
        public List<ChatMessage> GetMessage()
        {
            return messages;
        }

        // gets the users in the chat room
        public List<String> GetUsers()
        {
            return users;
        }
    }
}
