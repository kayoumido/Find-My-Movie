using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using System.IO;
using System.Text.RegularExpressions;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using Find_My_Movie.model;
using Find_My_Movie.model.repository;
using System.Threading;
using System.Net;

namespace Find_My_Movie {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        // CONSTANTES
        public const string FOLDER_NAME         = "FindMyMovie",
                            CONFIG_FILE_NAME    = "FindMyMovie.config",
                            API_KEY             = "88cf1d08f60e20cf9f7d3f49e82e7c8f"; 

        // ATTRIBUTES
        @interface interfaceClass       = new @interface();
        Thread childThread;
        double scrollPositionY          = 0;
        bool internetConected           = true;
        private bool searchClicked      = false;

        public MainWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// Click event to hide sidebar when button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void btnLeftMenuHide_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        /// <summary>
        /// Click event to show sidebar when button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void btnLeftMenuShow_Click(object sender, RoutedEventArgs e) {
            ShowHideMenu("sbShowLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);
        }

        /// <summary>
        /// Close all proccess when the windows is closed
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void MetroWindow_Closed(object sender, EventArgs e) {
            Environment.Exit(0);
        }
        
        /// <summary>
        /// Change image grid with when the windows is resized
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e) {

            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            double maxWidth = interfaceClass.GetWidthMovie(containerMovies.ActualWidth);
            single.Width = containerMovies.ActualWidth;
            foreach (Image child in covers) {
                child.MaxWidth = maxWidth;
            }

        }

        /// <summary>
        /// Show/Hide sidebar
        /// </summary>
        /// <param name="Storyboard">Element to hide</param>
        /// <param name="btnHide">Button performing hide action</param>
        /// <param name="btnShow">Button performing show action</param>
        /// <param name="pnl">Panel in which the sidebar is in</param>
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

        /// <summary>
        /// Launch choose folder windows it it's the first launch and initialize DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

            // get movie path in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = "";

            if (!File.Exists(folder_path)) {
                Directory.CreateDirectory(folder_path);
            }

            choosedirectory directoryClass = new choosedirectory();

            if (File.Exists(file_path)) {
                movie_path = directoryClass.GetPathConfig(file_path, "/config/path_movies");
            }

            // open the second form if it's the first launch
            if (movie_path == "") {
                directoryClass.ShowDialog();
                directoryClass.Close();
            }

            dbhandler FMDb = new dbhandler();

        }

        /// <summary>
        /// Event on click button "btnFolder"
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnFolder_Click (object sender, RoutedEventArgs e) {

            //kill the thread
            childThread.Abort();

            //open choose folder windows
            choosedirectory directoryClass = new choosedirectory();
            directoryClass.ShowDialog();
            directoryClass.Close();

            //delete all displayed movies
            List<UIElement> delItems = new List<UIElement>();
            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                    delItems.Add(child);
            }
            foreach (UIElement delitem in delItems) {
                gridMovies.Children.Remove(delitem);
            }

            //show loading
            loading.Visibility = Visibility.Visible;

            //restart thread to display movie from the new path
            childThread = new Thread(displayMovies);
            childThread.SetApartmentState(ApartmentState.STA);
            childThread.Start();
        }

        /// <summary>
        /// Check if internet is connected
        /// </summary>
        /// <returns></returns>
        private bool CheckConnection() {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.ch/");
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Test if movie is in db or not and display it from DB or API
        /// </summary>
        private void displayMovies() {

            MovieRepository movieRepository = new MovieRepository();

            //test internet connected
            if (!CheckConnection()) {
                internetConected = false;
            }

            var allMovies = interfaceClass.GetAllFilename();
            bool zeroMovieFound = true;

            List<int> alreadyDisplay = new List<int>(); //to avoid doublon

            foreach (var movie in allMovies) {

                bool displayMovie = false;

                //get movie name
                extractfileinfo extract = new extractfileinfo(movie);
                string movieName = extract.GetMovieName().Trim();

                //check if in db
                int idMovie = movieRepository.MovieExists(movieName);

                string urlImg = "";

                //if in DB
                if (idMovie != 0) {

                    //get data from db
                    fmmMovie infos = movieRepository.GetMovie(idMovie);

                    //get cover
                    if (infos.poster != null && internetConected)
                        urlImg = "https://image.tmdb.org/t/p/w500" + infos.poster;

                    displayMovie = true;

                }//if
                else if (internetConected) {

                    //Thread.Sleep(200) are here for avoid the maximum request impose per The movie database (40 requets per second)

                    Thread.Sleep(200);

                    // init new api object
                    api api = new api(movie);

                    // check if request to api worked
                    if (api.DidItWork()) {

                        //get data from api

                        Thread.Sleep(200);
                        Movie infos = api.GetMovieInfo();

                        if (infos.PosterPath != null)
                            urlImg = "https://image.tmdb.org/t/p/w500" + infos.PosterPath;

                        Thread.Sleep(200);
                        Credits credit = api.GetMovieCredits();

                        idMovie = infos.Id;

                        //add in DB
                        PopulateDB(infos, credit, movieName);

                        displayMovie = true;

                    }

                }//else


                //display cover
                if (displayMovie && !alreadyDisplay.Contains(idMovie)) {
                    this.Dispatcher.BeginInvoke(new Action(() => addMovieGrid(urlImg, idMovie)), System.Windows.Threading.DispatcherPriority.Background, null);
                    alreadyDisplay.Add(idMovie);
                    zeroMovieFound = false;
                }

            }//foreach

            if (zeroMovieFound)
                MessageBox.Show("No movies found in your folder !", "Find My Movie", MessageBoxButton.OK, MessageBoxImage.Information);
            

        }

        /// <summary>
        /// Display one movie in the grid
        /// </summary>
        /// <param name="urlImg">url cover </param>
        /// <param name="id">id movies (from the movie Database)</param>
        /// <param name="tag"> tag to know which elements are added by filter or search </param>
        /// <returns></returns>
        private void addMovieGrid(string urlImg, int id, string tag = null) {

            //hide loading
            loading.Visibility = Visibility.Collapsed;

            double maxWidth = interfaceClass.GetWidthMovie(containerMovies.ActualWidth);
            single.Width = containerMovies.ActualWidth;

            var imageControl = new Image();

            if (urlImg == "") {
                imageControl.Source = new BitmapImage(new Uri(@"assets/img/notFound.png", UriKind.Relative));
            }
            else {
                var webImage = new BitmapImage(new Uri(urlImg));
                imageControl.Source = webImage;
            }

            imageControl.Name = "id_" + id;
            imageControl.MaxWidth = maxWidth;
            imageControl.Stretch = Stretch.Fill;

            imageControl.Tag = "";
            if (tag != null) {
                imageControl.Tag = tag;
            }
            imageControl.MouseUp += new MouseButtonEventHandler(displaySingleMovie);

            if (single.Visibility == Visibility.Visible || searchClicked)
                imageControl.Visibility = Visibility.Collapsed;

            gridMovies.Children.Add(imageControl);
            
        }

        /// <summary>
        /// Lauch thread to display movie at the lauch app
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void MetroWindow_ContentRendered (object sender, EventArgs e) {

            childThread = new Thread(displayMovies);
            childThread.SetApartmentState(ApartmentState.STA);
            childThread.Start();

            ListBox objGenreList = lstbGenre;

            GenreRepository genreRepo = new GenreRepository();
            List<fmmGenre> genres = genreRepo.GetGenres();

            foreach (fmmGenre genre in genres) {
                ListBoxItem item = new ListBoxItem();
                item.Content = genre.name;
                item.Tag = genre.id;

                objGenreList.Items.Add(item);
            }

        }

        /// <summary>
        /// Event on click button "btnBack"
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnBack_Click(object sender, RoutedEventArgs e) {
            
            //get and show all images
            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                if (!searchClicked || child.Tag.ToString() == "search") {
                    child.Visibility = Visibility.Visible;
                }
            }

            //hide single
            single.Visibility = Visibility.Collapsed;

            //replace the scroll in the correct position
            containerMovies.ScrollToVerticalOffset(scrollPositionY);
            btnBack.Visibility = Visibility.Hidden;

            if (searchClicked) {
                btnBackSearch.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Event for when the search button is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnSearch_Click(object sender, RoutedEventArgs e) {

            TextBox objTextSearch = txtSearch;
            string searchText = objTextSearch.Text.Trim();

            ComboBox objSearchType = lstSearchType;
            string searchType = ((ComboBoxItem)objSearchType.SelectedItem).Tag.ToString();

            if (searchText != "") {

                // This is to help with the sql request
                searchText = searchText.Replace(" ", "%");

                btnBackSearch.Visibility = Visibility.Visible;
                MovieRepository movieRepo = new MovieRepository();
                List<fmmMovie> movies = movieRepo.Search(searchText, searchType);

                if (movies.Count > 0) {

                    // Hide the side bar that containes the search and filters
                    ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);

                    List<UIElement> delItems = new List<UIElement>();

                    IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
                    foreach (Image child in covers) {
                        // Get the objects that were added to the display by the search or filter function
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
                else {
                    MessageBox.Show("No movies were found!", "Find My Movie", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        /// <summary>
        /// Event for when the button to clear a search or filter is clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnBackSearch_Click(object sender, RoutedEventArgs e) {

            List<UIElement> delItems = new List<UIElement>();

            IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
            foreach (Image child in covers) {
                // Get the objects that were added to the display by the search or filter function
                // These elements will be deleted
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

        /// <summary>
        /// Event for when the filter button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilter_Click(object sender, RoutedEventArgs e) {

            bool isYearFromOK = true;
            bool isYearToOK = true;
            bool isYearFromEmpty = false;
            bool isYearToEmpty = false;
            string errorMessage = "";

            // Filter for the "from year"
            TextBox objYearFrom = txtYearFrom;
            string yearFrom = objYearFrom.Text.Trim();

            if (yearFrom == "") {
                isYearFromOK = false;
                isYearFromEmpty = true;
            }
            else if (!(Regex.IsMatch(yearFrom, @"^\d+$"))) {
                isYearFromOK = false;
                errorMessage += "The \"from year\" needs to be a number!";
            }
            else if (yearFrom.Length != 4) {
                isYearFromOK = false;
                errorMessage += "The \"from year\" needs to be YYYY!";
            }


            // Filter for the "to year"
            TextBox objYearTo = txtYearTo;
            string yearTo = objYearTo.Text.Trim();

            if (yearTo == "") {
                isYearToOK = false;
                isYearToEmpty = true;
            }
            else if (!(Regex.IsMatch(yearTo, @"^\d+$"))) {
                isYearToOK = false;
                if (errorMessage != "") {
                    errorMessage += "\n";
                }
                errorMessage += "The \"to year\" needs to be a number!";
            }
            else if (yearTo.Length != 4) {
                isYearToOK = false;
                if (errorMessage != "") {
                    errorMessage += "\n";
                }
                errorMessage += "The \"to year\" needs to be YYYY!";
            }

            if (isYearFromOK && isYearToOK) {
                if (Int32.Parse(yearFrom) > Int32.Parse(yearTo)) {
                    isYearFromOK = isYearToOK = false;
                    errorMessage += "The filter \"from year\" needs to be smaller than the \"to year\"!";
                }
            }

            // Filter for the genres
            ListBox objGenreList = lstbGenre;
            List<int> genres = new List<int>();

            foreach (ListBoxItem genre in objGenreList.Items) {
                if (genre.IsSelected) {
                    genres.Add(Int32.Parse(genre.Tag.ToString()));
                }
            }

            // Main area of the filter
            if ((isYearFromOK && isYearToOK) || (isYearFromEmpty && isYearToEmpty && genres.Count > 0)) {
                btnBackSearch.Visibility = Visibility.Visible;
                MovieRepository movieRepo = new MovieRepository();
                List<fmmMovie> movies;
                if (isYearFromOK && isYearToOK) {
                    movies = movieRepo.Filter(genres, new int[] { Int32.Parse(yearFrom), Int32.Parse(yearTo) });
                }
                else {
                    movies = movieRepo.Filter(genres);
                }


                if (movies.Count > 0) {

                    ShowHideMenu("sbHideLeftMenu", btnLeftMenuHide, btnLeftMenuShow, pnlLeftMenu);

                    List<UIElement> delItems = new List<UIElement>();

                    IEnumerable<Image> covers = gridMovies.Children.OfType<Image>();
                    foreach (Image child in covers) {
                        // Get the objects that were added to the display by the search or filter function
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
                else {
                    MessageBox.Show("No movies were found!", "Find My Movie", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }

            if (errorMessage != "") {
                MessageBox.Show(errorMessage, "Filter warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        /// <summary>
        /// Display single movie
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        void displaySingleMovie (object sender, MouseEventArgs e) {

            MovieRepository movieRepository = new MovieRepository();

            //change visibility button
            btnBack.Visibility = Visibility.Visible;
            btnBackSearch.Visibility = Visibility.Hidden;
            scrollPositionY = containerMovies.VerticalOffset;
            
            //get the clicked element in grid
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

        }

        /// <summary>
        /// Populate DB with movie returned by API
        /// </summary>
        /// <param name="infos">Movie information</param>
        /// <param name="credit">Cast and crew linked to movie</param>
        /// <param name="originalName">Orignial Name of the movie</param>
        private void PopulateDB(Movie infos, Credits credit, string originalName) {
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
