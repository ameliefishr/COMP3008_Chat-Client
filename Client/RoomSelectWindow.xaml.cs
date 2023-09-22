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
        public RoomSelectWindow(ChatServerInterface foobFromWindow1, String pUsername)
        {
            InitializeComponent();
            username = pUsername;
            foob = foobFromWindow1;
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> chatrooms = foob.GetChatRoomNamesList();
            chatRoomListView.ItemsSource = chatrooms;
        }
        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            Button joinButton = (Button)sender;
            string selectedChatRoomName = joinButton.Tag as string;
            foob.joinChatRoom(selectedChatRoomName, username);
            ChatroomWindow window3 = new ChatroomWindow(foob, username, selectedChatRoomName);
            window3.Show();
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
            //User currentUser = foob.getCurrentUser();
            //foob.logout(currentUser);
            Close();
        }
    }
}
