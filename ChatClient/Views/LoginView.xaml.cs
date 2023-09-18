using InterfaceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient.Views
{
    public partial class LoginView : UserControl
    {
        private ChatServerInterface foob;
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginBtnClick(object sender, RoutedEventArgs e)
        {
            if (UsernameTxt.Text.Length == 0)
            {
                ErrorTxt.Text = "Username can not be empty";
            }
            else
            {
                string username = UsernameTxt.Text;

                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (mainWindow != null) { mainWindow.Content = new ChatView(); }
                // ErrorTxt.Text = username;
            }
        }
    }
}
