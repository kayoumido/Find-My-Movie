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
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;
using System.Threading;

namespace Find_My_Movie
{
    /// <summary>
    /// Logique d'interaction pour choose_directory.xaml
    /// </summary>
    public partial class choose_directory : MetroWindow
    {

        public choose_directory()
        {
            InitializeComponent();
        }

        private string selected_path;

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (dialog.SelectedPath != "") {
                path.Text = dialog.SelectedPath;
                selected_path = dialog.SelectedPath;
                button.IsEnabled = true;
                button.Background = new SolidColorBrush(Colors.Transparent);
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Generate path for folder, file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;

            //create directory and file (close the file cause the processus doesn't stop himself)
            Directory.CreateDirectory(folder_path);
            var file = File.Create(file_path);
            file.Close();

            // add config in findMyMovies.config
            new XDocument(
                new XElement("config",
                    new XElement("path_movies", selected_path)
                )
            ).Save(file_path);

            this.Close();
        }

        public string GetPathConfig(string file_path, string config_name)
        {

            XmlDocument document = new XmlDocument();
            document.Load(file_path);
            XmlNode node = document.DocumentElement.SelectSingleNode(config_name); // config_name exemple : "/config/path_movies"
            return node.InnerText;

        }// GetPathConfig

        private void MetroWindow_Loaded (object sender, RoutedEventArgs e) {

            // get path movie in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = "";

            if (File.Exists(file_path))
                movie_path = GetPathConfig(file_path, "/config/path_movies");

            if (movie_path != "") {
                path.Text = movie_path;
                this.IsCloseButtonEnabled = true;
            }

        }
    }
}
