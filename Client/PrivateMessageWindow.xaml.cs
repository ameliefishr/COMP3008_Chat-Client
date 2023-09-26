using DatabaseLib;
using InterfaceLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for PrivateMessageWindow.xaml
    /// </summary>
    public partial class PrivateMessageWindow : Window
    {
        ChatServerInterface foob;
        ChatRoom chatRoom;
        private string senderName;
        private string recipient;
        private System.Threading.Timer messageUpdateTimer;
        private TimeSpan updateInterval = TimeSpan.FromSeconds(0.5);
        private CancellationTokenSource cancellationTokenSource;
        public PrivateMessageWindow(ChatServerInterface foobFromWindow1, string pSenderName, string pRecipient)
        {
            InitializeComponent();
            foob = foobFromWindow1;
            senderName = pSenderName;
            recipient = pRecipient;
            ChatRoomNameTextBlock.Text = (recipient);
            // check if a private message already exists between users
            chatRoom = foob.FindPrivateChatRoom(senderName, recipient);
            // if it doesn't make one
            if (chatRoom == null)
            {
                foob.createPrivateChatRoom(senderName, recipient);
            }
            // join the private message
            foob.joinPrivateChatRoom(senderName, recipient);
            messageUpdateTimer = new System.Threading.Timer(UpdateMessages, null, TimeSpan.Zero, updateInterval);

        }

        // when user presses the send button, send the message and update the message listview
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            
            var chatMessage = new ChatMessage
            {
                MessageText = message,
                MessageType = MessageType.Text
            };

            ChatRoom room = foob.FindPrivateChatRoom(senderName,recipient);
            foob.SendPrivateMessage(chatMessage, senderName, recipient);
            List<ChatMessage> msgs = room.GetMessage();

            chatRoomListView.ItemsSource = msgs;
        }

        // when a user leaves the room, remove the user from the room and close the window
        private void LeaveRoomButton_Click(object sender, RoutedEventArgs e)
        {
            foob.leavePrivateChatRoom(senderName, recipient, senderName);
            this.Close();
        }

        // when a user selects upload file, convert the file to a clickable link and send as a message
        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            // open file select window
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            string sSender = sender.ToString();
            bool? response = openFile.ShowDialog();

            if (response == true)
            {
                string filepath = openFile.FileName;

                // creates a file message with filepath as text
                var chatMessage = new ChatMessage
                {
                    MessageText = filepath,
                    MessageType = MessageType.File
                };
                ChatRoom room = foob.FindPrivateChatRoom(senderName, recipient);
                foob.SendPrivateMessage(chatMessage, senderName, recipient);
                List<ChatMessage> messages = room.GetMessage(); 

                chatRoomListView.ItemsSource = messages;
            }
        }

        // when a user clicks on a file message, display the file in a new window
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

        // cancel the task when the private message window is closed
        private void PrivateMessageWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
        }

        // update listview of messages
        private async void UpdateMessages(object state)
        {
            List<ChatMessage> chat = await Task.Run(() => foob.FindPrivateChatRoom(senderName, recipient).GetMessage());

            Dispatcher.Invoke(() => chatRoomListView.ItemsSource = chat);
        }
    }
}
