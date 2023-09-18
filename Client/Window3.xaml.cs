using DatabaseLib;
using InterfaceLib;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        ChatServerInterface foob;
        ChatRoom chatRoom;
        public Window3(ChatServerInterface foobFromWindow1)
        {
            InitializeComponent();
            foob = foobFromWindow1;
        }
        private void PrivateMessageButton_Click (object sender, RoutedEventArgs e)
        {
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;

            foob.SendMessage(message, "chat1", "user1");
            ChatRoom room = foob.FindChatRoom("chat1");
            List<String> msgs = room.GetMessage();

            chatRoomListView.ItemsSource = msgs;
        }
    }
}
