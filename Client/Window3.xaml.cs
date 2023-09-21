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
        private String username;
        private String roomName;
        public Window3(ChatServerInterface foobFromWindow1, String pUsername, String pRoomName)
        {
            InitializeComponent();
            foob = foobFromWindow1;
            username = pUsername;
            roomName = pRoomName;
            ChatRoomNameTextBlock.Text=(roomName);

            List<string> users = foob.FindChatRoom(roomName).GetUsers();
            userListView.ItemsSource = users;

        }
        private void PrivateMessageButton_Click (object sender, RoutedEventArgs e)
        {
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;

            foob.SendMessage(message, roomName, username);
            ChatRoom room = foob.FindChatRoom(roomName);
            List<String> msgs = room.GetMessage();

            chatRoomListView.ItemsSource = msgs;
        }
    }
}
