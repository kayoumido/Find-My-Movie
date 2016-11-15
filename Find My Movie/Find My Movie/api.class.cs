using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Find_My_Movie {
    class api {
        // the Movie DB api key
        private string key = "88cf1d08f60e20cf9f7d3f49e82e7c8f";
        // TMDbClient object
        TMDbClient client;

        // movie name extracted from file name
        private string movie_name;
        // movie date extracted from file name
        private string movie_date;
        // movie id
        private int movie_id;


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="file_name">name of the file to get info</param>
        /// 
        /// <author>Doran Kayoumi</author>
        public api(string file_name) {
            // instantiate new extractfileinfo object instance
            extractfileinfo extract = new extractfileinfo(file_name);

            // get movie name and release date and store them in attribute
            this.movie_name = extract.GetMovieName();
            this.movie_date = extract.GetMovieDate();

            // Instantiate a new TMDb Client, an API key is needed
            this.client = new TMDbClient(this.key);

            // get movie ID
            this.movie_id = 198287;// this.GetMovieID();
        }

        /// <summary>
        /// Search for movie in TMDb and returns movie ID
        /// </summary>
        /// 
        /// <author>Doran Kayoumi</author>
        private int GetMovieID() {
            // search for movie in TMDb
            SearchContainer<SearchMovie> res = this.client.SearchMovieAsync(this.movie_name).Result;

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
        /// 
        /// <author>Doran Kayoumi</author>
        public Movie GetMovieInfo() {
            return this.client.GetMovieAsync(this.movie_id).Result;
        }

        /// <summary>
        /// Get movie Cast info
        /// </summary>
        /// <returns>Returns movie poster</returns>
        /// 
        /// <author>Doran Kayoumi</author>
        public Credits GetMovieCast() {
            return this.client.GetMovieCreditsAsync(this.movie_id).Result;
        }

        /// <summary>
        /// Get movie Poster
        /// </summary>
        /// <returns>Returns movie poster</returns>
        /// <notes>https://image.tmdb.org/t/p/w500/ is the path to the images</notes>
        /// 
        /// <author>Doran Kayoumi</author>
        public Images GetMoviePoster() {
            return this.client.GetMovieImagesAsync(this.movie_id).Result;
        }

        public string GetMovieName() {
            return this.movie_name;
        }

    }
}
