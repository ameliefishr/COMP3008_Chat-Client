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

namespace Client
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private ChatServerInterface foob;
        public Window2(ChatServerInterface foobFromWindow1)
        {
            InitializeComponent();
            foob = foobFromWindow1;
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> chatrooms = foob.GetChatRooms();
            chatRoomListView.ItemsSource = chatrooms;
        }
        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            Window3 window3 = new Window3(foob);
            window3.Show();
            this.Close();
        }
        private void CreateChatRoomButton_Click(object sender, RoutedEventArgs e)
        {
            foob.createChatRoom(createChatRoomTextBox.Text);
        }
    }
}
