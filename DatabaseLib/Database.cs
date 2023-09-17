using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class Database
    {
        List<string> usernames;

        public Database()
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
            if (usernames.Contains(username))
            {
                return true;
            }
            return false;
        }
    }
}
