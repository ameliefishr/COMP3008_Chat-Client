using DatabaseLib;
using InterfaceLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

            List<string> users = foob.FindPublicChatRoom(roomName).GetUsers();
            userListView.ItemsSource = users;

            userListUpdateTimer = new System.Threading.Timer(UpdateUsersList, null, TimeSpan.Zero, updateInterval);
            messageUpdateTimer = new System.Threading.Timer(UpdateMessages, null, TimeSpan.Zero, updateInterval);

        }

        // when user presses send button, send the message in the chat room and update the message list view
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;

            var chatMessage = new ChatMessage
            {
                MessageText = message,
                MessageType = MessageType.Text
            };
            ChatRoom room = foob.FindPublicChatRoom(roomName);
            foob.SendMessage(chatMessage, roomName, username);
            List<ChatMessage> msgs = room.GetMessage();
            chatRoomListView.ItemsSource = msgs;
        }

        // when user click's leave button, remove user from chat room and close the window and re-open chat room select window 
        private void LeaveRoomButton_Click(object sender, RoutedEventArgs e)
        {
            foob.leaveChatRoom(roomName, username);
            RoomSelectWindow window2 = new RoomSelectWindow(foob, username);
            window2.Show();
            RoomSelectWindow.openWindows.Add(window2);
            RoomSelectWindow.openWindows.Remove(this);
            this.Close();
         }

        // when user click's the upload file button, send selected file as a message text link
        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFile.ShowDialog();

            if (response == true)
            {
                string filepath = openFile.FileName;

                // create file message with filepath as message text
                var chatMessage = new ChatMessage
                {
                    MessageText = filepath,
                    MessageType = MessageType.File
                };
                
                // send message and update message listview
                foob.SendMessage(chatMessage, roomName, username);
                ChatRoom room = foob.FindPublicChatRoom(roomName);
                List<ChatMessage> messages = room.GetMessage();

                chatRoomListView.ItemsSource = messages;
            }
        }

        // when user clicks on file message, display file contents in a new window
        private void FileMessage_Click(object sender, RoutedEventArgs e)
        {
            var textBlock = (ListViewItem)sender;
            var chatMessage = (ChatMessage)textBlock.DataContext;

            if (chatMessage.MessageType == MessageType.File)
            {
                FileDisplayWindow fileContentWindow = new FileDisplayWindow(chatMessage.MessageText);
                fileContentWindow.ShowDialog();
            }
        }

        // aynchronously updates list of user's in chat room
        private async void UpdateUsersList(object state)
        {
            List<string> users = await Task.Run(() => foob.FindPublicChatRoom(roomName).GetUsers());

            Dispatcher.Invoke(() => userListView.ItemsSource = users);
        }

        // asynchronously updates messages listview
        private async void UpdateMessages(object state)
        {
            List<ChatMessage> chat = await Task.Run(() => foob.GetChatRoomMessage(roomName));

            Dispatcher.Invoke(() => chatRoomListView.ItemsSource = chat);
        }

        // when user presses private message button, open a private message with the selected recipient
        private void PrivateMessageButton_Click(object sender, RoutedEventArgs e)
        {
            Button pmButton = (Button)sender;
            string recipient = pmButton.Tag as string;
            PrivateMessageWindow privateMessageWindow = new PrivateMessageWindow(foob, username, recipient); 
            privateMessageWindow.Show();
            RoomSelectWindow.openWindows.Add(privateMessageWindow);
        }
    }
}
