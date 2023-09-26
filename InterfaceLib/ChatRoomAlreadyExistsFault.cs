using System.Runtime.Serialization;

namespace InterfaceLib
{
    // custom exception for when a room name is taken already
    [DataContract]
    public class ChatRoomAlreadyExistsFault
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
