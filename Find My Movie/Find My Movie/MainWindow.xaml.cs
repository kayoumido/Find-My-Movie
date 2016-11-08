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
using System.Text.RegularExpressions;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Find_My_Movie {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        // CONSTANTES
        public const string FOLDER_NAME         = "FindMyMovie",
                            CONFIG_FILE_NAME    = "FindMyMovie.config",
                            JSON_DATA_FILE_NAME = "movie_data.json"; // @rem

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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

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

            @interface interfaceClass = new @interface();
            var allMovies = interfaceClass.GetAllFilename();
            foreach (var movie in allMovies) {

                // init new api object
                api api = new api(allMovies[3]);
                // get movie name
                string name = api.GetMovieName();

                MessageBox.Show(name);
                // Instantiate a new TMDb Client, an API key is needed
                TMDbClient client = new TMDbClient("88cf1d08f60e20cf9f7d3f49e82e7c8f");

                SearchContainer<SearchMovie> res = client.SearchMovieAsync(name).Result;
                MessageBox.Show(res.TotalResults.ToString());
                foreach (SearchMovie result in res.Results.Take(3)) {
                    MessageBox.Show(result.Id + ": " + result.Title);
                    MessageBox.Show(result.OriginalTitle);
                    MessageBox.Show(result.ReleaseDate.ToString());
                    MessageBox.Show(result.Popularity.ToString());
                    MessageBox.Show(result.VoteCount.ToString());

                    // Print out each hit
                    /*
                    Console.WriteLine(result.Id + ": " + result.Title);
                    Console.WriteLine("\t Original Title: " + result.OriginalTitle);
                    Console.WriteLine("\t Release date  : " + result.ReleaseDate);
                    Console.WriteLine("\t Popularity    : " + result.Popularity);
                    Console.WriteLine("\t Vote Average  : " + result.VoteAverage);
                    Console.WriteLine("\t Vote Count    : " + result.VoteCount);
                    Console.WriteLine();
                    Console.WriteLine("\t Backdrop Path : " + result.BackdropPath);
                    Console.WriteLine("\t Poster Path   : " + result.PosterPath);

                    Console.WriteLine();
                    */
                }
                break;
            }


        }
    }
}
