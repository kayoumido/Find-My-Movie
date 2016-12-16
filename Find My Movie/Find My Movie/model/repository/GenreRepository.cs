using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace Find_My_Movie.model.repository {
    class GenreRepository : IGenreRepository {

        private static dbhandler dbh          = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get a list of fmmGenre objects
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmGenre</returns>
        public List<fmmGenre> GetGenres() {
            string sql = @"
                SELECT
                    id,
                    name
                FROM
                    genre
                ORDER BY
                    name
                ASC
            ;";
            IEnumerable<fmmGenre> genres = DBConnection.Query<fmmGenre>(sql);

            return genres.ToList();
        }

        /// <summary>
        /// Get a genre
        /// </summary>
        /// <param name="id">ID of the genre to get</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>fmmCrew object of wanted genre</returns>
        public fmmGenre GetGenre(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a genre in DB
        /// </summary>
        /// <param name="genre">fmmGenre object to insert in DB</param>
        /// <param name="movieID">Id of the movie the genre is linked to</param>
        /// <returns>True or false</returns>
        public bool Insert(fmmGenre genre, int movieID) {
            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    genre 
                VALUES (
                    @id, 
                    @name
                );",
                genre
            );

            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    movie_has_genre 
                VALUES (
                    @fk_movie, 
                    @fk_genre
                );",

                new {
                    fk_movie = movieID,
                    fk_genre = genre.id
                }
            );

            dbh.Disconnect(DBConnection);

            return true;
        }

        /// <summary>
        /// Delete a genre
        /// </summary>
        /// <param name="id">ID of the genre to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>True or False</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
