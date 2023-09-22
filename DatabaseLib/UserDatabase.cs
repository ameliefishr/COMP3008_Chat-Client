using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class UserDatabase
    {
        List<User> users;
        private static UserDatabase instance;

        private UserDatabase()
        {
            users = new List<User>();
        }

        public static UserDatabase GetInstance()
        {
            if (instance == null)
            {
                instance = new UserDatabase();
            }
            return instance;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void AddUserByUsername(string username)
        {
            User newUser = new User(username);
            users.Add(newUser);

        }

        public User GetUserByUsername(string username)
        {
            foreach (User user in users)
            {
                if (user.getUsername().Equals(username))
                {
                    return user;
                }
            }
            return null;
        }

        public void RemoveUserByUsername(string username)
        {
            User userToRemove = GetUserByUsername(username);
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
            }
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
        }

        public Boolean CheckUser(string username)
        {
            Boolean found = false;
            if(users.Contains(GetUserByUsername(username)))
                { found = true; }
            return found;
        }


    }
}
