﻿using System;
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
using System.Text.RegularExpressions;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Find_My_Movie.model;
using Find_My_Movie.model.repository;
using System.Threading;
using System.ComponentModel;
using System.Net;

namespace Find_My_Movie {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        // CONSTANTES
        public const string FOLDER_NAME         = "FindMyMovie",
                            CONFIG_FILE_NAME    = "FindMyMovie.config",
                            JSON_DATA_FILE_NAME = "movie_data.json"; // @rem

        @interface interfaceClass = new @interface();
        MovieRepository movieRepository = new MovieRepository();


        Thread childThread;

        double scrollPositionY = 0;
        bool internetConected = true;

        private bool searchClicked = false;

        public MainWindow() {
            InitializeComponent();
        }

        private void btnLeftMenuHide_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void btnLeftMenuShow_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        private void MetroWindow_Closed(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e) {

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
                btnHide.Visibility = Visibility.Visible;
                btnShow.Visibility = Visibility.Hidden;
            }
            else if (Storyboard.Contains("Hide")) {
                btnHide.Visibility = Visibility.Hidden;
                btnShow.Visibility = Visibility.Visible;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

            dbhandler FMDb = new dbhandler();

            // get movie path in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = "";

            choose_directory directoryClass = new choose_directory();

            if (File.Exists(file_path)) {
                movie_path = directoryClass.GetPathConfig(file_path, "/config/path_movies");
            }

            // open the second form if it's the first launch
            if (movie_path == "") {
                directoryClass.ShowDialog();
                directoryClass.Close();
            }

        }//MetroWindow_Loaded

        private void btnFolder_Click (object sender, RoutedEventArgs e) {
            childThread.Abort();
            choose_directory directoryClass = new choose_directory();
            directoryClass.ShowDialog();
            directoryClass.Close();

            List<UIElement> delItems = new List<UIElement>();
            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                    delItems.Add(child);
            }
            foreach (UIElement delitem in delItems) {
                gridMovies.Children.Remove(delitem);
            }

            childThread = new Thread(displayMovies);
            childThread.SetApartmentState(ApartmentState.STA);
            childThread.Start();
        }

        private bool CheckConnection(String URL) {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK) return true;
                else return false;
            }
            catch {
                return false;
            }
        }

        private void displayMovies() {

            if (!CheckConnection("https://www.google.ch/")) {
                internetConected = false;
            }

            var allMovies = interfaceClass.GetAllFilename();
            int not_found = 0;
            int i = 0;

            List<int> alreadyDisplay = new List<int>(); //to avoid doublon

            foreach (var movie in allMovies) {

                bool displayMovie = false;

                extractfileinfo extract = new extractfileinfo(movie);
                string movieName = extract.GetMovieName().Trim();

                int idMovie = movieRepository.MovieExists(movieName);

                string urlImg = "";

                if (idMovie != 0) {

                    fmmMovie infos = movieRepository.GetMovie(idMovie);

                    if (infos.poster != null && internetConected)
                        urlImg = "https://image.tmdb.org/t/p/w500" + infos.poster;

                    displayMovie = true;

                }//if
                else if (internetConected) {

                    Thread.Sleep(200);
                    // init new api object
                    api api = new api(movie);

                    // check if request to api worked
                    if (api.DidItWork()) {

                        Thread.Sleep(200);
                        Movie infos = api.GetMovieInfo();

                        if (infos.PosterPath != null)
                            urlImg = "https://image.tmdb.org/t/p/w500" + infos.PosterPath;

                        Thread.Sleep(200);
                        Credits credit = api.GetMovieCredits();

                        idMovie = infos.Id;
                        PopulateDB(infos, credit, movieName);

                        displayMovie = true;

                    }
                    else {
                        not_found++;
                    }

                }//else


                //display cover
                if (displayMovie && !alreadyDisplay.Contains(idMovie)) {
                    this.Dispatcher.BeginInvoke(new Action(() => i = addMovieGrid(urlImg, idMovie)), System.Windows.Threading.DispatcherPriority.Background, null);
                    alreadyDisplay.Add(idMovie);
                }

            }//foreach

            //MessageBox.Show(not_found + " movies wern't found");
        }

        private int addMovieGrid(string urlImg, int i, string tag = null) {
            double maxWidth = interfaceClass.getWidthMovie(containerMovies.ActualWidth);
            single.Width = containerMovies.ActualWidth;

            var imageControl = new Image();

            if (urlImg == "") {
                imageControl.Source = new BitmapImage(new Uri(@"assets/img/notFound.png", UriKind.Relative));

            }
            else {
                var webImage = new BitmapImage(new Uri(urlImg));
                imageControl.Source = webImage;
            }

            imageControl.Name = "id_" + i;
            imageControl.MaxWidth = maxWidth;
            imageControl.Stretch = Stretch.UniformToFill;

            imageControl.Tag = "";
            if (tag != null) {
                imageControl.Tag = tag;
            }
            imageControl.MouseUp += new MouseButtonEventHandler(displaySingleMovie);

            if (single.Visibility == Visibility.Visible || searchClicked)
                imageControl.Visibility = Visibility.Collapsed;

            gridMovies.Children.Add(imageControl);
            i++;
            return i;
        }


        private void MetroWindow_ContentRendered (object sender, EventArgs e) {

            childThread = new Thread(displayMovies);
            childThread.SetApartmentState(ApartmentState.STA);
            childThread.Start();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                if (!searchClicked || child.Tag.ToString() == "search") {
                    child.Visibility = Visibility.Visible;
                }
            }
            single.Visibility = Visibility.Collapsed;
            containerMovies.ScrollToVerticalOffset(scrollPositionY);
            btnBack.Visibility = Visibility.Hidden;

            if (searchClicked) {
                btnBackSearch.Visibility = Visibility.Visible;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) {

            TextBox objTextSearch = txtSearch;
            string searchText = objTextSearch.Text.Trim();

            ComboBox objSearchType = lstSearchType;
            string searchType = ((ComboBoxItem)objSearchType.SelectedItem).Tag.ToString();

            if (searchText != "") {

                searchText = searchText.Replace(" ", "%");

                btnBackSearch.Visibility = Visibility.Visible;
                MovieRepository movieRepo = new MovieRepository();
                List<fmmMovie> movies = movieRepo.Search(searchText, searchType);

                List<UIElement> delItems = new List<UIElement>();

                IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
                foreach (Image child in covers) {
                    // Get the objects that were added to the display by the search function
                    // These elements will be deleted
                    if (child.Tag.ToString() == "search") {
                        delItems.Add(child);
                    }
                    else {
                        child.Visibility = Visibility.Collapsed;
                    }
                }

                foreach (UIElement delitem in delItems) {
                    gridMovies.Children.Remove(delitem);
                }

                searchClicked = false;

                foreach (fmmMovie movie in movies) {

                    string urlImg = "https://az853139.vo.msecnd.net/static/images/not-found.png";
                    if (movie.poster != null) {
                        urlImg = "https://image.tmdb.org/t/p/w500" + movie.poster;
                    }

                    addMovieGrid(urlImg, movie.id, "search");

                }

                searchClicked = true;
            }
        }

        private void btnBackSearch_Click(object sender, RoutedEventArgs e) {

            List<UIElement> delItems = new List<UIElement>();

            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                if (child.Tag.ToString() == "search") {
                    delItems.Add(child);
                }
                else {
                    child.Visibility = Visibility.Visible;
                }
            }

            foreach (UIElement delitem in delItems) {
                gridMovies.Children.Remove(delitem);
            }

            searchClicked = false;
            btnBackSearch.Visibility = Visibility.Hidden;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e) {

            TextBox objYearFrom = txtYearFrom;
            string yearFrom = objYearFrom.Text.Trim();

            TextBox objYearTo = txtYearTo;
            string yearTo = objYearTo.Text.Trim();

            string errorMessage = "";

            if (yearFrom != "" && yearTo != "") {
                if ((Regex.IsMatch(yearFrom, @"^\d+$") && Regex.IsMatch(yearTo, @"^\d+$")) && (yearFrom.Length == 4 && yearTo.Length == 4)) {

                    if (Int32.Parse(yearFrom) <= Int32.Parse(yearTo)) {
                        btnBackSearch.Visibility = Visibility.Visible;
                        MovieRepository movieRepo = new MovieRepository();
                        List<fmmMovie> movies = movieRepo.Filter(new int[] { Int32.Parse(yearFrom), Int32.Parse(yearTo) });

                        List<UIElement> delItems = new List<UIElement>();

                        IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
                        foreach (Image child in covers) {
                            if (child.Tag.ToString() == "search") {
                                delItems.Add(child);
                            }
                            else {
                                child.Visibility = Visibility.Collapsed;
                            }
                        }

                        foreach (UIElement delitem in delItems) {
                            gridMovies.Children.Remove(delitem);
                        }

                        searchClicked = false;

                        foreach (fmmMovie movie in movies) {

                            string urlImg = "https://az853139.vo.msecnd.net/static/images/not-found.png";
                            if (movie.poster != null) {
                                urlImg = "https://image.tmdb.org/t/p/w500" + movie.poster;
                            }

                            addMovieGrid(urlImg, movie.id, "search");

                        }

                        searchClicked = true;
                    }
                    else {
                        errorMessage += "The filter \"from year\" needs to be smaller than the \"to year\"!";
                    }
                }
                else {
                    errorMessage += "The filter for the year needs to be a number (YYYY)!";
                }
            }

            if (errorMessage != "") {
                MessageBox.Show(errorMessage, "Filter warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        void displaySingleMovie (object sender, MouseEventArgs e) {

            btnBack.Visibility = Visibility.Visible;
            btnBackSearch.Visibility = Visibility.Hidden;
            scrollPositionY = containerMovies.VerticalOffset;


            var mouseWasDownOn = e.Source as FrameworkElement;
            if (mouseWasDownOn != null) {

                //get infos
                string elementName = mouseWasDownOn.Name;
                elementName = elementName.Replace("id_", "");
                fmmMovie infos = movieRepository.GetMovie(Convert.ToInt32(elementName));
                List<fmmCast> casts = movieRepository.GetMovieCasts(Convert.ToInt32(elementName));
                List<fmmCrew> crews = movieRepository.GetMovieCrews(Convert.ToInt32(elementName));
                List<fmmGenre> genres = movieRepository.GetMovieGenres(Convert.ToInt32(elementName));

                //cover
                if (infos.poster == null || !internetConected) {
                    coverSingle.Source = new BitmapImage(new Uri(@"assets/img/notFound.png", UriKind.Relative));
                }
                else {
                    coverSingle.Source = new BitmapImage(new Uri("https://image.tmdb.org/t/p/w500" + infos.poster));
                }

                //reset field
                genresSingle.Text = "";
                authorSingle.Text = "";

                //title
                titleSingle.Text = infos.title + " (" + infos.releasedate.Substring(6, 4) + ")";

                //duration
                durationSingle.Text = infos.runtime + " min";

                //gender
                foreach (var genre in genres) {
                    if (!(genres.First() == genre))
                        genresSingle.Text += ", ";

                    genresSingle.Text += genre.name;
                }

                //rated
               if (infos.adult)
                    ratedSingle.Text = "Adult";
                else
                    ratedSingle.Text = "All public";

                //director
                foreach (var crew in crews) {
                    if (crew.job == "Director") {
                        directorSingle.Text = crew.name;
                        break;
                    }

                }
                

                //actors
               foreach (var cast in casts) {
                    if (!(casts.First() == cast))
                        authorSingle.Text += ", ";

                    authorSingle.Text += cast.name + " (" + cast.character.Replace(" (voice)","") + ")" ;

                }

                //description
                descSingle.Text = infos.overview;
            }

            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                child.Visibility = Visibility.Collapsed;
            }//foreach
            single.Visibility = Visibility.Visible;

        }//displaySingleMovie


        private void PopulateDB(Movie infos, Credits credit, String originalName) {
            MovieRepository      _movieRepo      = new MovieRepository();
            CollectionRepository _collectionRepo = new CollectionRepository();
            CrewRepository       _crewrepo       = new CrewRepository();
            CastRepository       _castrepo       = new CastRepository();
            CompanyRepository    _companyrepo    = new CompanyRepository();
            CountryRepository    _countryrepo    = new CountryRepository();
            GenreRepository      _genrerepo      = new GenreRepository();
            LanguageRepository   _languagerepo   = new LanguageRepository();


            bool collectionAdded = false;

            if (infos.BelongsToCollection != null) {

                fmmCollection collection = new fmmCollection {
                    id = infos.BelongsToCollection.Id,
                    name = infos.BelongsToCollection.Name,
                    poster = infos.BelongsToCollection.PosterPath
                };
                _collectionRepo.Insert(collection);

                collectionAdded = true;

            }

            fmmMovie movie = new fmmMovie {
                id          = infos.Id,
                imdbid      = infos.ImdbId,
                title       = infos.Title,
                ogtitle     = originalName,
                adult       = infos.Adult,
                budget      = infos.Budget,
                homepage    = infos.Homepage,
                runtime     = infos.Runtime,
                tagline     = infos.Tagline,
                voteaverage = infos.VoteAverage,
                oglanguage  = infos.OriginalLanguage,
                overview    = infos.Overview,
                popularity  = infos.Popularity,
                poster      = infos.PosterPath,
                releasedate = infos.ReleaseDate.ToString().Substring(0, 10)
            };

            if (collectionAdded) {
                movie.fk_collection = infos.BelongsToCollection.Id;
            }

            bool movieAdded = _movieRepo.Insert(movie);

            foreach (Crew crew in credit.Crew) {
                var ncrew = new fmmCrew {
                    id         = crew.Id,
                    creditid   = crew.CreditId,
                    name       = crew.Name,
                    image      = crew.ProfilePath,
                    department = crew.Department,
                    job        = crew.Job
                };

                _crewrepo.Insert(ncrew, movie.id);
            }

            foreach (Cast cast in credit.Cast) {
                var ncast = new fmmCast {
                    id        = cast.Id,
                    castid    = cast.CastId,
                    creditid  = cast.CreditId,
                    name      = cast.Name,
                    image     = cast.ProfilePath,
                    character = cast.Character,
                    aorder    = cast.Order
                };

                _castrepo.Insert(ncast, movie.id);
            }

            foreach (ProductionCompany company in infos.ProductionCompanies) {
                var ncompany = new fmmCompany {
                    id   = company.Id,
                    name = company.Name,
                };

                _companyrepo.Insert(ncompany, movie.id);
            }

            foreach (ProductionCountry country in infos.ProductionCountries) {
                var ncountry = new fmmCountry {
                    name = country.Name,
                };

                _countryrepo.Insert(ncountry, movie.id);
            }

            foreach (Genre genre in infos.Genres) {
                var ngenre = new fmmGenre {
                    id   = genre.Id,
                    name = genre.Name,
                };

                _genrerepo.Insert(ngenre, movie.id);
            }


            foreach (SpokenLanguage language in infos.SpokenLanguages) {
                var nlanguage = new fmmLanguage {
                    name = language.Name,
                };

                _languagerepo.Insert(nlanguage, movie.id);
            }
        }
    }
}
