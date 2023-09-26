using System.Runtime.Serialization;

namespace InterfaceLib
{
    // custom exception for when username is taken/invalid
    [DataContract]
    public class UsernameNotValidFault
    {
        private string problemType;

        [DataMember]
        public string ProblemType
        {
            get { return problemType; }
            set { problemType = value; }
        }
    }
}
