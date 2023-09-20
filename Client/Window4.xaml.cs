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
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4(string filePath)
        {
            InitializeComponent();

            // Check the file type based on its extension
            string fileExtension = null;

            if (File.Exists(filePath))
            { 
                fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
                // Rest of your code to handle the file extension
            }
            else
            {
                MessageBox.Show("The selected file does not exist.");
            }

            if (IsTextFile(fileExtension))
            {
                // Display text content
                string fileContent = File.ReadAllText(filePath);
                fileTextBlock.Text = fileContent;
                fileTextBlock.Visibility = Visibility.Visible;
            }
            else if (IsImageFile(fileExtension))
            {
                // Display image
                try
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(filePath));
                    fileImage.Source = bitmapImage;
                    fileImage.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Unsupported file type.");
            }
        }

        private bool IsTextFile(string fileExtension)
        {
            // Define a list of text file extensions you want to support
            string[] textExtensions = { ".txt", ".csv", ".xml", ".html", ".cs", ".cpp", ".c", ".java", ".json" };

            return textExtensions.Contains(fileExtension);
        }

        private bool IsImageFile(string fileExtension)
        {
            // Define a list of image file extensions you want to support
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            return imageExtensions.Contains(fileExtension);
        }
    }

}
