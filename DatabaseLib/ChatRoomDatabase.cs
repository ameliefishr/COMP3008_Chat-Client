using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DatabaseLib.ChatRoom;

namespace DatabaseLib
{
    public class ChatRoomDatabase
    {
        private List<ChatRoom> public_roomsList;
        private List<ChatRoom> private_roomsList;
        private static ChatRoomDatabase instance;

        private ChatRoomDatabase()
        {
            public_roomsList = new List<ChatRoom>();
            private_roomsList = new List<ChatRoom>();
        }

        public static ChatRoomDatabase GetInstance()
        {
            if (instance == null)
            {
                instance = new ChatRoomDatabase();
            }
            return instance;
        }

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

        public Boolean CheckPublicChatRoom(string name)
        {
            Boolean found = false;
            if (public_roomsList.Contains(GetPublicChatRoomByName(name)))
            { found = true; }
            return found;
        }

        public Boolean CheckPrivateChatRoom(string name)
        {
            Boolean found = false;
            if (private_roomsList.Contains(GetPrivateChatRoomByName(name)))
            { found = true; }
            return found;
        }

        public List<ChatRoom> GetPublicRoomList()
        {
            return public_roomsList;
        }

        public List<ChatRoom> GetPrivateRoomList()
        {
            return private_roomsList;
        }

        public List<ChatMessage> GetPublicMessages(string chatRoom)
        {
            ChatRoom room = GetPublicChatRoomByName(chatRoom);
            return room.GetMessage();
        }

        public List<ChatMessage> GetPrivateMessages(string chatRoom)
        {
            ChatRoom room = GetPrivateChatRoomByName(chatRoom);
            return room.GetMessage();
        }
    }
}

