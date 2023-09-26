using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using DatabaseLib;
using InterfaceLib;
using System.Security.Cryptography;
using System.Windows;

namespace ChatServer
{
    // implementation of our chat server functionalities
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

        // creates a public chat room and add's it to room database
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
                //if chat room name already exists, throw error
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

        // create private chat room and add's it to room database
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
                // if chat room name already exists, throw error
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

        // generate unique room id based of sender and recipient usernames
        private static string GenerateUniqueRoomId(string username1, string username2)
        {
            // sorts names alphabetically
            string[] sortedUsernames = { username1, username2 };
            Array.Sort(sortedUsernames);

            // combines usernames
            string combinedUsernames = sortedUsernames[0] + sortedUsernames[1];

            // generate hash value
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedUsernames));

                // converts hash to hex string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                // return unique id string
                return builder.ToString();
            }
        }

        // adds a user to chat room
        public void joinChatRoom(string roomName, string username)
        {
            ChatRoom room = roomDB.GetPublicRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(username);
        }

        //  add a user to private chat room
        public void joinPrivateChatRoom(string senderName, string recipient)
        {
            string roomName = GenerateUniqueRoomId(senderName, recipient);
            ChatRoom room = roomDB.GetPrivateRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(senderName);
        }

        // remove user from chat room
        public void leaveChatRoom(string roomName, string username)
        {
            ChatRoom room = roomDB.GetPublicRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(username);
        }

        // remove user from private chat room
        public void leavePrivateChatRoom(string senderName, string recipient, string username)
        {
            string roomName = GenerateUniqueRoomId(senderName, recipient);
            ChatRoom room = roomDB.GetPrivateRoomList().Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(username);
        }

        // validates username, if it's valid, add user to user databbase
        public bool login(string username)
        {
            try
            {
                // validate username
                if (username.Equals("") || username == null)
                {
                    Console.WriteLine("Username cannot be empty");
                    return false;
                }
                else
                {
                    if (userDB.CheckUser(username) == false)
                    {
                        userDB.AddUserByUsername(username);
                        Console.WriteLine("User added: " + username);
                        return true;
                    }
                    // if username is taken
                    else
                    {
                        throw new FaultException<UsernameNotValidFault>(new UsernameNotValidFault()
                        { ProblemType = "Username is invalid..." }, new FaultReason("Username is taken!"));
                    }
                }
            }
            catch (FaultException<UsernameNotValidFault> e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // gets list of usernames currently connected to chat room
        public List<string> GetChatRoomNamesList()
        {
            return roomDB.GetPublicRoomList().Select(room => room.GetRoomName()).ToList();
        }

        // send's message in chat room
        public void SendMessage(ChatMessage message, string roomName, string username)
        {
            ChatRoom tempRoom = null;

            foreach (ChatRoom room in roomDB.GetPublicRoomList())
            {
                // find chat room
                if (room.GetRoomName() == roomName)
                {
                    tempRoom = room;
                    break; 
                }
            }

            // if room exists
            if (tempRoom != null)
            {
                // check message type
                if (message.MessageType == MessageType.Text)
                {
                    // validate message
                    if ((message.MessageText).Length > 280)
                    {
                        MessageBox.Show("Message cannot exceed 280 characters");
                    }
                    else if(message.MessageText.Equals("") || message.MessageText == null)
                    {
                        MessageBox.Show("Message cannot be empty");
                    }
                    // send message
                    else
                    {
                        var chatMessage = new ChatMessage
                        {
                            MessageText = username + ": " + message.MessageText,
                            MessageType = message.MessageType
                        };
                        tempRoom.AddMessage(chatMessage);
                    }
                }
                // if it's a file message
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

        // send's private message in chat room
        public void SendPrivateMessage(ChatMessage message, string senderName, string recipientName)
        {
            ChatRoom tempRoom = null;
            string roomName = GenerateUniqueRoomId(senderName, recipientName);

            // find room
            foreach (ChatRoom room in roomDB.GetPrivateRoomList())
            {
                if (room.GetRoomName() == roomName)
                {
                    tempRoom = room;
                    break; 
                }
            }

            // if room exists
            if (tempRoom != null)
            {
                // check message type
                if (message.MessageType == MessageType.Text)
                {
                    // validate message
                    if((message.MessageText).Length > 280)
                    {
                        MessageBox.Show("Message cannot exceed 280 characters");
                    }
                    else if (message.MessageText.Equals("") || message.MessageText == null)
                    {
                        MessageBox.Show("Message cannot be empty");
                    }
                    // send message
                    else
                    {
                        var chatMessage = new ChatMessage
                        {
                            MessageText = senderName + ": " + message.MessageText,
                            MessageType = message.MessageType
                        };
                        tempRoom.AddMessage(chatMessage);
                    }
                }
                // if it's a file message
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

        // find a public chat room by it's name
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

        // find private chat room by its name
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

        // logout fo the application, remove user from user DB remove them from any rooms they are in
        public void logout(string username)
        {
            userDB.RemoveUserByUsername(username);
            Console.WriteLine("User " + username + " logged out.");

            foreach (ChatRoom room in roomDB.GetPublicRoomList())
            {
                List<string> usersToRemove = room.GetUsers().Where(user => user.Equals(username)).ToList();

                foreach (string user in usersToRemove)
                {
                    room.RemoveFromRoom(user);
                }
            }

            foreach (ChatRoom room in roomDB.GetPrivateRoomList())
            {
                List<string> usersToRemove = room.GetUsers().Where(user => user.Equals(username)).ToList();

                foreach (string user in usersToRemove)
                {
                    room.RemoveFromRoom(user);
                }
            }
        }

        // get chat room messages
        public List<ChatMessage> GetChatRoomMessage(string roomName)
        {
            return roomDB.GetPublicMessages(roomName);
        }


    }
}
