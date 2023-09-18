using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    [DataContract]
    public class User
    {
        [DataMember]
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
