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
using InterfaceLib;
using DatabaseLib;

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
        private void CreateChatRoomButton_Click(object sender, RoutedEventArgs e)
        {
            foob.createChatRoom(createChatRoomTextBox.Text);

            List<string> chatrooms = foob.GetChatRoomNamesList();
            chatRoomListView.ItemsSource = chatrooms;
        }

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

        private async void UpdateChatroomList(object state)
        {
            List<string> chatrooms = await Task.Run(() => foob.GetChatRoomNamesList());

            Dispatcher.Invoke(() => chatRoomListView.ItemsSource = chatrooms);
        }
    }
}
