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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using System.IO;
using System.Diagnostics;

namespace Find_My_Movie {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        // CONSTANTES
        public const string FOLDER_NAME = "FindMyMovie",
                            CONFIG_FILE_NAME = "FindMyMovie.config",
                            JSON_DATA_FILE_NAME = "movie_data.json";

        @interface interfaceClass = new @interface();

        public MainWindow() {
            InitializeComponent();
        }

        private void btnLeftMenuHide_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void btnLeftMenuShow_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void MetroWindow_Closed (object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void MetroWindow_SizeChanged (object sender, SizeChangedEventArgs e) {

            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            double maxWidth = interfaceClass.getWidthMovie(containerMovies.ActualWidth);
            single.Width = containerMovies.ActualWidth;
            foreach (Image child in covers) {
                child.MaxWidth = maxWidth;
            }//foreach

        }//MetroWindow_SizeChanged

        private void ShowHideMenu(string Storyboard, Button btnHide, Button btnShow, StackPanel pnl) {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);

            if (Storyboard.Contains("Show")) {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (Storyboard.Contains("Hide")) {
                btnHide.Visibility = System.Windows.Visibility.Hidden;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            choose_directory directoryClass = new choose_directory();

            // get path movie in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = "";

            if (File.Exists(file_path))
                movie_path = directoryClass.GetPathConfig(file_path, "/config/path_movies");

            // Open the second form if it's the first launch
            if (movie_path == "")
                directoryClass.ShowDialog();
            else
                directoryClass.Close();

            var allMovies = interfaceClass.GetAllFilename();
            foreach (var movie in allMovies) {
                
            }

            //Hard Display
            string[] moviesCover = new string[] {
                                            "https://s-media-cache-ak0.pinimg.com/236x/82/f0/15/82f01596145820a6f8ab76f191ae346d.jpg",
                                            "https://jacobboombar.files.wordpress.com/2014/03/oblivion-dvd-cover-55.jpg",
                                            "http://violentworldofparker.com/wordpress/wp-content/uploads/2012/10/Flashfire2013.jpg",
                                            "https://s-media-cache-ak0.pinimg.com/236x/82/f0/15/82f01596145820a6f8ab76f191ae346d.jpg",
                                            "https://jacobboombar.files.wordpress.com/2014/03/oblivion-dvd-cover-55.jpg",
                                            "https://s-media-cache-ak0.pinimg.com/236x/82/f0/15/82f01596145820a6f8ab76f191ae346d.jpg",
                                            "https://jacobboombar.files.wordpress.com/2014/03/oblivion-dvd-cover-55.jpg",
                                            "http://violentworldofparker.com/wordpress/wp-content/uploads/2012/10/Flashfire2013.jpg",
                                            "https://s-media-cache-ak0.pinimg.com/236x/82/f0/15/82f01596145820a6f8ab76f191ae346d.jpg",
                                            "https://jacobboombar.files.wordpress.com/2014/03/oblivion-dvd-cover-55.jpg",
                                            "https://s-media-cache-ak0.pinimg.com/236x/82/f0/15/82f01596145820a6f8ab76f191ae346d.jpg",
                                            "https://jacobboombar.files.wordpress.com/2014/03/oblivion-dvd-cover-55.jpg",
                                            "http://violentworldofparker.com/wordpress/wp-content/uploads/2012/10/Flashfire2013.jpg",
                                            "https://s-media-cache-ak0.pinimg.com/236x/82/f0/15/82f01596145820a6f8ab76f191ae346d.jpg",
                                            "https://jacobboombar.files.wordpress.com/2014/03/oblivion-dvd-cover-55.jpg",
                                            "http://violentworldofparker.com/wordpress/wp-content/uploads/2012/10/Flashfire2013.jpg"
            };

            //display cover
            int i = 0;
            double maxWidth = interfaceClass.getWidthMovie(containerMovies.ActualWidth);
            single.Width = containerMovies.ActualWidth;
            foreach (var cover in moviesCover) {
                var webImage = new BitmapImage(new Uri(cover));
                var imageControl = new Image();
                imageControl.Name = "id_" + i;
                imageControl.Source = webImage;
                imageControl.MaxWidth =  maxWidth;
                imageControl.MouseUp += new MouseButtonEventHandler(displaySingleMovie);
                gridMovies.Children.Add(imageControl);
                i++;
            }

        }//MetroWindow_Loaded

        private void play_MouseUp (object sender, MouseButtonEventArgs e) {
           
            string pathMovie = "C:\\Users\\Antoine.DESSAUGES\\Documents\\Projets\\Wildlife.wmv";
            if (File.Exists(pathMovie))
                Process.Start(pathMovie);
            else
                MessageBox.Show("Erreur : Chemin vers le fichier incorecte !");
        }

        void displaySingleMovie (object sender, MouseEventArgs e) {


            var mouseWasDownOn = e.Source as FrameworkElement;
            if (mouseWasDownOn != null) {
                string elementName = mouseWasDownOn.Name; //MessageBox.Show(elementName);
            }

            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                child.Visibility = Visibility.Collapsed;
            }//foreach
            single.Visibility = Visibility.Visible;

        }//displaySingleMovie

        private void closeSingle_MouseUp (object sender, MouseButtonEventArgs e) {
            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                child.Visibility = Visibility.Visible;
            }//foreach
            single.Visibility = Visibility.Collapsed;
        }//closeSingle_MouseUp


    }
}
