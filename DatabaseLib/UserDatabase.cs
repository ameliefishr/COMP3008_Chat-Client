using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class UserDatabase
    {
        List<string> usernames;

        public UserDatabase()
        {
            usernames = new List<string>();
        }

        public void AddUser(string username)
        {
            usernames.Add(username);
        }

        public void RemoveUser(string username)
        {
            usernames.Remove(username);
        }

        public Boolean CheckUser(string username)
        {
            return usernames.Contains(username);
        }
    }
}
