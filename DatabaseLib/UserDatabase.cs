using System;
using System.Collections.Generic;

namespace DatabaseLib
{
    // database to store all our user data
    public class UserDatabase
    {
        List<User> users;
        private static UserDatabase instance;

        private UserDatabase()
        {
            users = new List<User>();
        }

        // returns the full database
        public static UserDatabase GetInstance()
        {
            if (instance == null)
            {
                instance = new UserDatabase();
            }
            return instance;
        }

        // adds a user to the database
        public void AddUser(User user)
        {
            users.Add(user);
        }

        // adds a user by their username to the database
        public void AddUserByUsername(string username)
        {
            User newUser = new User(username);
            users.Add(newUser);

        }

        // finds a user by their username
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

        // removes a user by their username
        public void RemoveUserByUsername(string username)
        {
            User userToRemove = GetUserByUsername(username);
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
            }
        }

        // removes a user
        public void RemoveUser(User user)
        {
            users.Remove(user);
        }

        // check if user is found in database
        public Boolean CheckUser(string username)
        {
            Boolean found = false;
            if(users.Contains(GetUserByUsername(username)))
                { found = true; }
            return found;
        }


    }
}
