using DatabaseLib;
using InterfaceLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
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
        private String username;
        private String roomName;

        private System.Threading.Timer userListUpdateTimer;
        private System.Threading.Timer messageUpdateTimer;
        private TimeSpan updateInterval = TimeSpan.FromSeconds(0.5);

        public ChatroomWindow(ChatServerInterface foobFromWindow1, String pUsername, String pRoomName)
        {
            InitializeComponent();
            foob = foobFromWindow1;
            username = pUsername;
            roomName = pRoomName;
            ChatRoomNameTextBlock.Text=(roomName);

            List<string> users = foob.FindChatRoom(roomName).GetUsers();
            userListView.ItemsSource = users;

            userListUpdateTimer = new System.Threading.Timer(UpdateUsersList, null, TimeSpan.Zero, updateInterval);
            messageUpdateTimer = new System.Threading.Timer(UpdateMessages, null, TimeSpan.Zero, updateInterval);

        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;

            var chatMessage = new ChatMessage
            {
                MessageText = message,
                MessageType = MessageType.Text
            };
            ChatRoom room = foob.FindChatRoom(roomName);
            foob.SendMessage(chatMessage, room, username);
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
                ChatRoom room = foob.FindChatRoom(roomName);
                foob.SendMessage(chatMessage, room, username);
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


        private async void UpdateUsersList(object state)
        {
            List<string> users = await Task.Run(() => foob.FindChatRoom(roomName).GetUsers());

            Dispatcher.Invoke(() => userListView.ItemsSource = users);
        }

        private async void UpdateMessages(object state)
        {
            List<ChatMessage> chat = await Task.Run(() => foob.GetChatRoomMessage(roomName));

            Dispatcher.Invoke(() => chatRoomListView.ItemsSource = chat);
        }
        private void PrivateMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string recipient = "recipient"; //change recipient later to the selected user's username, just hardcoded for now
            PrivateMessageWindow privateMessageWindow = new PrivateMessageWindow(foob, username, recipient); 
            privateMessageWindow.Show();
        }
    }
}
