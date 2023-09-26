using System;
using System.Collections.Generic;
using static DatabaseLib.ChatRoom;

namespace DatabaseLib
{
    public class ChatRoomDatabase
    {
        // database to store all of our chat rooms
        // different lists for public/private rooms to prevent mixing them up
        private List<ChatRoom> public_roomsList;
        private List<ChatRoom> private_roomsList;
        private static ChatRoomDatabase instance;

        private ChatRoomDatabase()
        {
            public_roomsList = new List<ChatRoom>();
            private_roomsList = new List<ChatRoom>();
        }

        // get the full database
        public static ChatRoomDatabase GetInstance()
        {
            if (instance == null)
            {
                instance = new ChatRoomDatabase();
            }
            return instance;
        }

        // add's a chat room to the database
        public void AddChatRoom(String roomName, RoomType roomType)
        {
            ChatRoom room = new ChatRoom(roomName, roomType);
            if(roomType == RoomType.Public)
            {
                public_roomsList.Add(room);
            }
            else if(roomType == RoomType.Private)
            {
                private_roomsList.Add(room);
            }
            
        }

        // finds a public chat room by it's name
        public ChatRoom GetPublicChatRoomByName(string name)
        {
            foreach (ChatRoom room in public_roomsList)
            {
                if (room.GetRoomName().Equals(name))
                {
                    return room;
                }
            }
            return null;
        }

        // finds a private chat room by it's name
        public ChatRoom GetPrivateChatRoomByName(string name)
        {
            foreach (ChatRoom room in private_roomsList)
            {
                if (room.GetRoomName().Equals(name))
                {
                    return room;
                }
            }
            return null;
        }

        // checks if a public chat room exists in the database
        public Boolean CheckPublicChatRoom(string name)
        {
            Boolean found = false;
            if (public_roomsList.Contains(GetPublicChatRoomByName(name)))
            { found = true; }
            return found;
        }

        // checks if a private chat room exists in the database
        public Boolean CheckPrivateChatRoom(string name)
        {
            Boolean found = false;
            if (private_roomsList.Contains(GetPrivateChatRoomByName(name)))
            { found = true; }
            return found;
        }

        // gets full list of available public chat rooms
        public List<ChatRoom> GetPublicRoomList()
        {
            return public_roomsList;
        }

        // gets full list of private chat rooms
        public List<ChatRoom> GetPrivateRoomList()
        {
            return private_roomsList;
        }

        // gets all the messages within a public chat room
        public List<ChatMessage> GetPublicMessages(string chatRoom)
        {
            ChatRoom room = GetPublicChatRoomByName(chatRoom);
            return room.GetMessage();
        }

        // gets all the messages within a private chat room
        public List<ChatMessage> GetPrivateMessages(string chatRoom)
        {
            ChatRoom room = GetPrivateChatRoomByName(chatRoom);
            return room.GetMessage();
        }
    }
}

