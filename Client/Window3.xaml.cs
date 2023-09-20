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
        public Window3(ChatServerInterface foobFromWindow1, String pUsername)
        {
            InitializeComponent();
            foob = foobFromWindow1;
            username = pUsername;
        }
        private void PrivateMessageButton_Click (object sender, RoutedEventArgs e)
        {
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;

            foob.SendMessage(message, "chat1", username);
            ChatRoom room = foob.FindChatRoom("chat1");
            List<String> msgs = room.GetMessage();

            chatRoomListView.ItemsSource = msgs;
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

                // Create a new window to display the file content
                Window4 fileContentWindow = new Window4(filepath);
                fileContentWindow.ShowDialog();
            }
        }
    }
}
