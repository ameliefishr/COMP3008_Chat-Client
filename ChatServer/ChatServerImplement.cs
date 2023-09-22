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
        private static ChatServerImplement instance;
        private UserDatabase userDB;
        private ChatRoomDatabase roomDB;

        private ChatServerImplement()
        {
            userDB = UserDatabase.GetInstance();
            roomDB = ChatRoomDatabase.GetInstance();
        }

        public static ChatServerImplement GetInstance()
        {
            if (instance == null)
            {
                instance = new ChatServerImplement();
            }
            return instance;
        }

        public bool createChatRoom(string roomName)
        {
            try
            {
                if (roomDB.CheckChatRoom(roomName) == false)
                {
                    roomDB.AddChatRoom(roomName, ChatRoom.RoomType.Public);
                    Console.WriteLine("Chat room added: " + roomName);
                    return true;
                }
                else
                {
                    throw new FaultException<ChatRoomAlreadyExistsFault>(new ChatRoomAlreadyExistsFault()
                    { ProblemType = "Chat room is invalid..." }, new FaultReason("Chat room name is taken!"));
                }
            }
            catch (FaultException<ChatRoomAlreadyExistsFault> e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


        }

        public void joinChatRoom(string roomName, string username)
        {
            ChatRoom room = roomDB.GetRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(username);
        }

        public void leaveChatRoom(string roomName, string username)
        {
            ChatRoom room = roomDB.GetRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(username);
        }

        public bool login(string username)
        {
            try
            {
                if (userDB.CheckUser(username) == false)
                {
                    userDB.AddUserByUsername(username);
                    Console.WriteLine("User added: " + username);
                    return true;
                }
                else
                {
                    throw new FaultException<UsernameNotValidFault>(new UsernameNotValidFault()
                    { ProblemType = "Username is invalid..." }, new FaultReason("Username is taken!"));
                }
            }
            catch (FaultException<UsernameNotValidFault> e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public List<string> GetChatRoomNamesList()
        {
            return roomDB.GetRoomList().Select(room => room.GetRoomName()).ToList();
        }

        public void SendMessage(ChatMessage message, ChatRoom chatRoom, string username)
        {
            if (chatRoom != null)
            {
                // Create a chat message and add it to the room
                if (message.MessageType == MessageType.Text)
                {
                    var chatMessage = new ChatMessage
                    {
                        MessageText = username + ": " + message.MessageText,
                        MessageType = message.MessageType
                    };
                    chatRoom.AddMessage(chatMessage);
                }
                else if (message.MessageType == MessageType.File)
                {
                    var chatMessage = new ChatMessage
                    {
                        MessageText = username + " uploaded file:",
                        MessageType = MessageType.Text
                    };
                    var fileMessage = new ChatMessage
                    {
                        MessageText = message.MessageText,
                        MessageType = message.MessageType
                    };
                    chatRoom.AddMessage(chatMessage);
                    chatRoom.AddMessage(fileMessage);
                }
            }
        }

        public ChatRoom FindChatRoom(string roomName)
        {
            foreach (ChatRoom room in roomDB.GetRoomList())
            {
                if (room.GetRoomName().Equals(roomName))
                {
                    return room;
                }
            }
            return null;

        }

        public void logout(string username)
        {
            userDB.RemoveUserByUsername(username);
            Console.WriteLine("User "+ username + " logged out.");
        }

        public List<ChatMessage> GetChatRoomMessage(string roomName)
        {
            return roomDB.GetMessages(roomName);
        }
    }
}
