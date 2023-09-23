using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseLib;
using InterfaceLib;
using System.Security.Cryptography;
using System.Text;

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

        public bool createPublicChatRoom(string roomName)
        {
            try
            {
                if (roomDB.CheckPublicChatRoom(roomName) == false)
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

        public bool createPrivateChatRoom(string sender, string recipient)
        {
            string roomName = GenerateUniqueRoomId(sender, recipient);
            try
            {
                if (roomDB.CheckPrivateChatRoom(roomName) == false)
                {
                    roomDB.AddChatRoom(roomName, ChatRoom.RoomType.Private);
                    Console.WriteLine("Private chat room added: " + roomName);
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

        private static string GenerateUniqueRoomId(string username1, string username2)
        {
            // Sort the usernames alphabetically
            string[] sortedUsernames = { username1, username2 };
            Array.Sort(sortedUsernames);

            // Concatenate the sorted usernames
            string combinedUsernames = sortedUsernames[0] + sortedUsernames[1];

            // Compute a hash value
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedUsernames));

                // Convert the hash bytes to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public void joinChatRoom(string roomName, string username)
        {
            ChatRoom room = roomDB.GetPublicRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(username);
        }
        public void joinPrivateChatRoom(string senderName, string recipient)
        {
            string roomName = GenerateUniqueRoomId(senderName, recipient);
            ChatRoom room = roomDB.GetPrivateRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(senderName);
        }

        public void leaveChatRoom(string roomName, string username)
        {
            ChatRoom room = roomDB.GetPublicRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(username);
        }

        public void leavePrivateChatRoom(string senderName, string recipient, string username)
        {
            string roomName = GenerateUniqueRoomId(senderName, recipient);
            ChatRoom room = roomDB.GetPrivateRoomList().Find(x => x.GetRoomName().Equals(roomName));
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
            return roomDB.GetPublicRoomList().Select(room => room.GetRoomName()).ToList();
        }

        public void SendMessage(ChatMessage message, string roomName, string username)
        {
            ChatRoom tempRoom = null;

            foreach (ChatRoom room in roomDB.GetPublicRoomList())
            {
                if (room.GetRoomName() == roomName)
                {
                    tempRoom = room;
                    break; // No need to continue searching once the room is found
                }
            }

            if (tempRoom != null)
            {
                // Create a chat message and add it to the room
                if (message.MessageType == MessageType.Text)
                {
                    var chatMessage = new ChatMessage
                    {
                        MessageText = username + ": " + message.MessageText,
                        MessageType = message.MessageType
                    };
                    tempRoom.AddMessage(chatMessage);
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
                    tempRoom.AddMessage(chatMessage);
                    tempRoom.AddMessage(fileMessage);
                }
            }
        }

        public void SendPrivateMessage(ChatMessage message, string senderName, string recipientName)
        {
            ChatRoom tempRoom = null;
            string roomName = GenerateUniqueRoomId(senderName, recipientName);

            foreach (ChatRoom room in roomDB.GetPrivateRoomList())
            {
                if (room.GetRoomName() == roomName)
                {
                    tempRoom = room;
                    break; // No need to continue searching once the room is found
                }
            }

            if (tempRoom != null)
            {
                // Create a chat message and add it to the room
                if (message.MessageType == MessageType.Text)
                {
                    var chatMessage = new ChatMessage
                    {
                        MessageText = senderName + ": " + message.MessageText,
                        MessageType = message.MessageType
                    };
                    tempRoom.AddMessage(chatMessage);
                }
                else if (message.MessageType == MessageType.File)
                {
                    var chatMessage = new ChatMessage
                    {
                        MessageText = senderName + " uploaded file:",
                        MessageType = MessageType.Text
                    };
                    var fileMessage = new ChatMessage
                    {
                        MessageText = message.MessageText,
                        MessageType = message.MessageType
                    };
                    tempRoom.AddMessage(chatMessage);
                    tempRoom.AddMessage(fileMessage);
                }
            }
        }

        public ChatRoom FindPublicChatRoom(string roomName)
        {
            foreach (ChatRoom room in roomDB.GetPublicRoomList())
            {
                if (room.GetRoomName().Equals(roomName))
                {
                    return room;
                }
            }
            return null;

        }

        public ChatRoom FindPrivateChatRoom(string sender, string recipient)
        {
            string roomName = GenerateUniqueRoomId(sender, recipient);
            foreach (ChatRoom room in roomDB.GetPrivateRoomList())
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
            foreach (ChatRoom room in roomDB.GetPublicRoomList())
            {
                foreach (string user in room.GetUsers())
                {
                    if (user.Equals(username))
                    {
                        room.RemoveFromRoom(username);
                    }
                }
            }
        }

        public List<ChatMessage> GetChatRoomMessage(string roomName)
        {
            return roomDB.GetPublicMessages(roomName);
        }


    }
}
