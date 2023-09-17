using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLib
{
    [ServiceContract]
    public interface ChatServerInterface
    {
        [OperationContract]
        void login(string username);
    }
}
