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
        List<ChatRoom> roomsList;
        private static ChatRoomDatabase instance;

        private ChatRoomDatabase()
        {
            roomsList = new List<ChatRoom>();
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
            roomsList.Add(room);
        }


        public ChatRoom GetChatRoomByName(string name)
        {
            foreach (ChatRoom room in roomsList)
            {
                if (room.GetRoomName().Equals(name))
                {
                    return room;
                }
            }
            return null;
        }

        public Boolean CheckChatRoom(string name)
        {
            Boolean found = false;
            if (roomsList.Contains(GetChatRoomByName(name)))
            { found = true; }
            return found;
        }
        public List<ChatRoom> GetRoomList()
        {
            return roomsList;
        }

        public List<ChatMessage> GetMessages(string chatRoom)
        {
            ChatRoom room = GetChatRoomByName(chatRoom);
            return room.GetMessage();
        }
    }
}

