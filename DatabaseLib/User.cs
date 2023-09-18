using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    public class User
    {
        private string username;
        public User(string pUsername)
        {
            username = pUsername;
        }

        public void setUsername(string pUsername)
        {
            username = pUsername;
        }

        public string getUsername()
        {
            return username;
        }
    }
}
