using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseLib;
using InterfaceLib;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ChatServerImplement : ChatServerInterface
    {
        private Database db;

        public ChatServerImplement()
        {
            db = new Database();
        }
        public void login(string username)
        {
            if(db.CheckUser(username) == false)
            {
                db.AddUser(username);
            }
        }
    }
}
