using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class FileDisplayWindow : Window
    {
        public FileDisplayWindow(string filePath)
        {
            InitializeComponent();

            string fileExtension = null;

            // check if file exsists, if it does then get the file exension
            if (File.Exists(filePath))
            { 
                fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
            }
            else
            {
                MessageBox.Show("The selected file does not exist.");
            }

            // if it's a text file, display text
            if (IsTextFile(fileExtension))
            {
                string fileContent = File.ReadAllText(filePath);
                fileTextBlock.Text = fileContent;
                fileTextBlock.Visibility = Visibility.Visible;
            }
            else if (IsImageFile(fileExtension))
            {
                // if it's an image file, display image
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

            // if it's not an accepted file type
            else
            {
                MessageBox.Show("Unsupported file type.");
            }
        }

        // boolean to check if it's a valid text file
        private bool IsTextFile(string fileExtension)
        {
            // allowed text file types
            string[] textExtensions = { ".txt", ".csv", ".xml", ".html", ".cs", ".cpp", ".c", ".java", ".json" };

            return textExtensions.Contains(fileExtension);
        }

        // boolean to check if it's a valid image file
        private bool IsImageFile(string fileExtension)
        {
            // allowed image file types
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            return imageExtensions.Contains(fileExtension);
        }
    }

}
