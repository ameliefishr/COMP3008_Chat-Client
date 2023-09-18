using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLib
{
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
