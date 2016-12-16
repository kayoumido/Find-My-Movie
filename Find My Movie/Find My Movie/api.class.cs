using System;
using System.Collections.Generic;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Find_My_Movie {
    class api {

        private string key = MainWindow.API_KEY;
        private TMDbClient client;
        private string movieName;
        private int movieId;


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="fileName">name of the file to get info</param>
        public api(string fileName) {

            extractfileinfo extract = new extractfileinfo(fileName);

            this.movieName = extract.GetMovieName();
            this.client    = new TMDbClient(this.key);
            this.movieId   = this.GetMovieID();
            
        }

        /// <summary>
        /// Search for movie in TMDb and returns movie ID
        /// </summary>
        /// <returns>Movie ID or -1 if nothing was found</returns>
        private int GetMovieID() {
            // search for movie in TMDb
            SearchContainer<SearchMovie> res = this.client.SearchMovieAsync(this.movieName).Result;

            // check if any movies were found
            if (res.TotalResults > 0) {
                // normaly first value returned is what we are searching for, so we can get the id of that movie
                return res.Results[0].Id;
            }
            else {
                return -1;
            }
        }

        /// <summary>
        /// Get movie information using movie id
        /// </summary>
        /// <returns>Returns movie info</returns>
        /// <notes>https://image.tmdb.org/t/p/w500/ is the path to the images</notes>
        public Movie GetMovieInfo() {
            return this.client.GetMovieAsync(this.movieId).Result;
        }

        /// <summary>
        /// Get movie Cast info
        /// </summary>
        /// <returns>Returns movie poster</returns>
        public Credits GetMovieCredits() {
            return this.client.GetMovieCreditsAsync(this.movieId).Result;
        }

        /// <summary>
        /// Get movie Name
        /// </summary>
        /// <returns>Movie name</returns>
        public string GetMovieName() {
            return this.movieName;
        }

        /// <summary>
        /// Check if movie was found
        /// </summary>
        /// <returns>Result of request to tm movie db</returns>
        public bool DidItWork() {
            bool res = true;

            // check if movie was found
            if (this.movieId == -1) {
                // if not set result to false
                res = false;
            }
            // return result
            return res;
        }

    }
}
