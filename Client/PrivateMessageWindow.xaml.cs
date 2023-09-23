using DatabaseLib;
using InterfaceLib;
using System;
using System.Collections.Generic;
using System.IO;
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
            chatRoom = foob.FindPrivateChatRoom(senderName, recipient);
            if (chatRoom == null)
            {
                foob.createPrivateChatRoom(senderName, recipient);
            }
            foob.joinPrivateChatRoom(senderName, recipient);
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

            ChatRoom room = foob.FindPrivateChatRoom(senderName,recipient);
            foob.SendPrivateMessage(chatMessage, senderName, recipient);
            List<ChatMessage> msgs = room.GetMessage();

            chatRoomListView.ItemsSource = msgs;
        }
        private void LeaveRoomButton_Click(object sender, RoutedEventArgs e)
        {
            foob.leavePrivateChatRoom( senderName, recipient, senderName);
            this.Close();
        }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            string sSender = sender.ToString();
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
                ChatRoom room = foob.FindPrivateChatRoom(senderName, recipient);
                foob.SendPrivateMessage(chatMessage, senderName, recipient);
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
        private void PrivateMessageWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
        }
        private async void UpdateMessages(object state)
        {
            List<ChatMessage> chat = await Task.Run(() => foob.FindPrivateChatRoom(senderName, recipient).GetMessage());

            Dispatcher.Invoke(() => chatRoomListView.ItemsSource = chat);
        }
    }
}
