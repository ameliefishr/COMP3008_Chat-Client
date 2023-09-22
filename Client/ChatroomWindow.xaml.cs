using DatabaseLib;
using InterfaceLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
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
    public partial class ChatroomWindow : Window
    {
        ChatServerInterface foob;
        ChatRoom chatRoom;
        private String username;
        private String roomName;
        private String uploadedFilePath;
        public ChatroomWindow(ChatServerInterface foobFromWindow1, String pUsername, String pRoomName)
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

            var chatMessage = new ChatMessage
            {
                MessageText = message,
                MessageType = MessageType.Text
            };

            foob.SendMessage(chatMessage, roomName, username);
            ChatRoom room = foob.FindChatRoom(roomName);
            List<ChatMessage> msgs = room.GetMessage();
            // MessageBox.Show(message);
            chatRoomListView.ItemsSource = msgs;
        }
        private void LeaveRoomButton_Click(object sender, RoutedEventArgs e)
        {
            foob.leaveChatRoom(roomName, username);
            RoomSelectWindow window2 = new RoomSelectWindow(foob, username);
            window2.Show();
            this.Close();
         }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFile.ShowDialog();

            if (response == true)
            {
                string filepath = openFile.FileName;

                // Read the content of the selected file
                string fileContent = File.ReadAllText(filepath);

                // Create a file message
                var chatMessage = new ChatMessage
                {
                    MessageText = filepath,
                    MessageType = MessageType.File
                };
                uploadedFilePath = filepath;
                foob.SendMessage(chatMessage, roomName, username);
                ChatRoom room = foob.FindChatRoom(roomName);
                List<ChatMessage> messages = room.GetMessage(); // Update your ChatRoom class to return ChatMessage objects

                chatRoomListView.ItemsSource = messages;
            }
        }

        private void FileMessage_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event for file messages (e.g., open the file)
            var textBlock = (ListViewItem)sender;
            var chatMessage = (ChatMessage)textBlock.DataContext; // Get the associated ChatMessage

            if (chatMessage.MessageType == MessageType.File)
            {
                // Handle opening the file here
                FileDisplayWindow fileContentWindow = new FileDisplayWindow(chatMessage.MessageText);
                fileContentWindow.ShowDialog();
            }
        }
        private void ChatRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            List<ChatMessage> chat = foob.GetChatRoomMessage(roomName);
            chatRoomListView.ItemsSource = chat;
        }

        private void UserRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> users = foob.FindChatRoom(roomName).GetUsers();
            userListView.ItemsSource = users;
        }

        private void PrivateSendButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
