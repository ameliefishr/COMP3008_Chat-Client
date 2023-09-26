using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // when user presses login button, load login window
        private void BtnUser1_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow newWindow1 = new LoginWindow();
            newWindow1.Show();
            this.Close();
        }
    }
}
