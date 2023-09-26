using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InterfaceLib;

namespace Client
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class RoomSelectWindow : Window
    {
        private ChatServerInterface foob;
        private String username;
        private System.Threading.Timer chatRoomListUpdateTimer;
        private TimeSpan updateInterval = TimeSpan.FromSeconds(0.5);
        public static List<Window> openWindows = new List<Window>();
        public RoomSelectWindow(ChatServerInterface foobFromWindow1, String pUsername)
        {
            InitializeComponent();
            username = pUsername;
            foob = foobFromWindow1;

            chatRoomListUpdateTimer = new System.Threading.Timer(UpdateChatroomList, null, TimeSpan.Zero, updateInterval);
        }

        // when a user presses the join button, add them to the selected room and close the room select window
        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            Button joinButton = (Button)sender;
            string selectedChatRoomName = joinButton.Tag as string;
            foob.joinChatRoom(selectedChatRoomName, username);
            ChatroomWindow window3 = new ChatroomWindow(foob, username, selectedChatRoomName);
            window3.Show();
            openWindows.Add(window3);
            this.Close();
        }

        // when a user presses the create chat room button, create a chat room with the entered name
        private void CreateChatRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (createChatRoomTextBox.Text == null || createChatRoomTextBox.Text.Equals(""))
            {
                MessageBox.Show("Chat room name cannot be empty");
            }
            else
            {
                foob.createPublicChatRoom(createChatRoomTextBox.Text);
                List<string> chatrooms = foob.GetChatRoomNamesList();
                chatRoomListView.ItemsSource = chatrooms;
            }
        }

        // when a user presses the logout button, log them out and close all of the windows that user has open
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            foob.logout(username);
            foreach (var window in openWindows)
            {
                window.Close();
            }

            openWindows.Clear();
            Close();
        }

        // update list of chat room asynchronously
        private async void UpdateChatroomList(object state)
        {
            List<string> chatrooms = await Task.Run(() => foob.GetChatRoomNamesList());

            Dispatcher.Invoke(() => chatRoomListView.ItemsSource = chatrooms);
        }
    }
}
