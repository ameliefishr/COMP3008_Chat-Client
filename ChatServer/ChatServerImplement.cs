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
        private UserDatabase db;
        private List<ChatRoom> roomList;
        private Dictionary<User, ChatRoom> userRoomList;
        private User currentUser;

        public ChatServerImplement()
        {
            db = new UserDatabase();
            roomList = new List<ChatRoom>();
        }

        public void createChatRoom(string roomName)
        {
            ChatRoom room = new ChatRoom(roomName);
            roomList.Add(room);
        }

        public void joinChatRoom(string roomName, User user)
        {
            ChatRoom room = roomList.Find(x => x.GetRoomName().Equals(roomName));
            room.AddToRoom(user.getUsername());
        }

        public void leaveChatRoom(string roomName, User user)
        {
            ChatRoom room = roomList.Find(x => x.GetRoomName().Equals(roomName));
            room.RemoveFromRoom(user.getUsername());
        }

        public bool login(string username)
        {
            try
            {
                if (db.CheckUser(username) == false)
                {
                    User newUser = new User(username);
                    currentUser = newUser;
                    db.AddUserByUsername(username);
                    Console.WriteLine("User " + username + " added");
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

        public void logout(User user)
        {
            db.RemoveUserByUsername(user.getUsername());
            Console.WriteLine("User " + user.getUsername() + " logged out");
        }

        public void setCurrentUser(User user)
        { currentUser = user; }

        public User getCurrentUser() { return currentUser; }
    }
    }
