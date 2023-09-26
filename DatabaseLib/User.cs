using System.Runtime.Serialization;

namespace DatabaseLib
{
    // user class to store user details
    [DataContract]
    public class User
    {
        [DataMember]
        private string username;
        public User(string pUsername)
        {
            username = pUsername;
        }

        // sets user's username
        public void setUsername(string pUsername)
        {
            username = pUsername;
        }

        // gets user's username
        public string getUsername()
        {
            return username;
        }
    }
}
