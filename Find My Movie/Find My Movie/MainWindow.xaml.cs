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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {

            dbhandler FMDb                      = new dbhandler();
            MovieRepository movieRepo           = new MovieRepository();
            CollectionRepository collectionRepo = new CollectionRepository();

            // get movie path in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path   = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path     = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path    = "";

            choose_directory directoryClass = new choose_directory();

            if (File.Exists(file_path)) { 
                movie_path = directoryClass.GetPathConfig(file_path, "/config/path_movies");
            }

            // open the second form if it's the first launch
            if (movie_path == "") {
                directoryClass.ShowDialog();
                directoryClass.Close();
            }

            var allMovies = interfaceClass.GetAllFilename();
            int not_found = 0;
            int foo = 0; // temporary counter to limit number of films displayed, otherwise we will loose a lot of time waiting for the app to launch.

            foreach (var movieName in allMovies) {
                
                // init new api object
                api api = new api(movieName);

                // check if request to api worked
                if (api.DidItWork()) {
                    // increment number of films found
                    foo++;

                    Movie infos = api.GetMovieInfo();

                    bool collectionAdded = false; 

                    if (infos.BelongsToCollection != null) {

                        fmmCollection collection = new fmmCollection {
                            id     = infos.BelongsToCollection.Id,
                            name   = infos.BelongsToCollection.Name,
                            poster = infos.BelongsToCollection.PosterPath
                        };
                        collectionRepo.Insert(collection);

                        collectionAdded = true;

                    }

                    fmmMovie movie = new fmmMovie {
                        id          = infos.Id,
                        imdbid      = infos.ImdbId,
                        title       = infos.Title,
                        ogtitle     = infos.OriginalTitle,
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
                        releasedate = infos.ReleaseDate.ToString().Substring(0,10)
                    };

                    if (collectionAdded) {
                        movie.fk_collection = infos.BelongsToCollection.Id;
                    }

                    bool movieAdded = movieRepo.Insert(movie);

                    Credits credit  = api.GetMovieCredits();

                    CrewRepository     _crewrepo     = new CrewRepository();
                    CastRepository     _castrepo     = new CastRepository();
                    CompanyRepository  _companyrepo  = new CompanyRepository();
                    CountryRepository  _countryrepo  = new CountryRepository();
                    GenreRepository    _genrerepo    = new GenreRepository();
                    LanguageRepository _languagerepo = new LanguageRepository();

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
                            aorder     = cast.Order
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

                    // check number of film founds
                    if (foo == 10) {                                                                                                                                                                             
                        // break out of loop
                        // this is emporary code
                        break;
                    }
                }
                else {
                    not_found++;
                }
            }
            //MessageBox.Show(not_found + " movies wern't found");

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

        private void btnFolder_Click (object sender, RoutedEventArgs e) {
            choose_directory directoryClass = new choose_directory();
            directoryClass.ShowDialog();
            directoryClass.Close();
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
