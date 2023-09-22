using InterfaceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private ChatServerInterface foob;
        private String username;
        public LoginWindow()
        {
            InitializeComponent();
            username = null;

            ChannelFactory<ChatServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/ChatService";
            foobFactory = new ChannelFactory<ChatServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
      
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e) 
        {
            if (foob.login(UsernameTxt.Text))
            {
                username = UsernameTxt.Text;
                RoomSelectWindow window2 = new RoomSelectWindow(foob, username);
                window2.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username is not valid: Login Error");
            }

        }
    }
}
