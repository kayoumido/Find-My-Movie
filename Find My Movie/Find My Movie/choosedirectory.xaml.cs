using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Find_My_Movie
{
    /// <summary>
    /// Logique d'interaction pour choose_directory.xaml
    /// </summary>
    public partial class choosedirectory : MetroWindow
    {

        public choosedirectory()
        {
            InitializeComponent();
        }

        private string selected_path;

        /// <summary>
        /// Event on click button "btnOpenFile"
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //open a select path windows
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            //enable the save button
            if (dialog.SelectedPath != "") {
                path.Text = dialog.SelectedPath;
                selected_path = dialog.SelectedPath;
                button.IsEnabled = true;
                button.Background = new SolidColorBrush(Colors.Transparent);
            }

        }

        /// <summary>
        /// Event on click button "button"
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
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

        /// <summary>
        /// Get config option in the config file
        /// </summary>
        /// <param name="file_path"> path to the config file</param>
        /// <param name="config_name">name of the config you need in config value</param>
        /// <returns></returns>
        public string GetPathConfig(string file_path, string config_name)
        {

            XmlDocument document = new XmlDocument();
            document.Load(file_path);
            XmlNode node = document.DocumentElement.SelectSingleNode(config_name); // config_name exemple : "/config/path_movies"
            return node.InnerText;

        }// GetPathConfig

        /// <summary>
        /// Event when windows is loaded
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void MetroWindow_Loaded (object sender, RoutedEventArgs e) {

            // get path movie in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = "";

            //get folder path if exist
            if (File.Exists(file_path))
                movie_path = GetPathConfig(file_path, "/config/path_movies");

            if (movie_path != "") {
                path.Text = movie_path;
                this.IsCloseButtonEnabled = true;
            }

        }
    }
}
