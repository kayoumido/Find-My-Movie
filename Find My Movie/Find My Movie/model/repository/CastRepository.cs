using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Find_My_Movie.model.repository {
    public class CastRepository : ICastRepository {

        private static dbhandler dbh          = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get a list of fmmCasts objects
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmCasts</returns>
        public List<fmmCast> GetCasts() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a cast
        /// </summary>
        /// <param name="id">ID of the cast to get</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>fmmCast object of wanted Cast</returns>
        public fmmCast GetCast(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a new Cast in DB
        /// </summary>
        /// <param name="cast">fmmCast object to insert in DB</param>
        /// <param name="movieID">Id of the movie the cast is linked to</param>
        /// <returns>True or false</returns>
        public bool Insert(fmmCast cast, int movieID) {

            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    cast 
                VALUES (
                    @id,
                    @castid,
                    @creditid, 
                    @name,
                    @image
                );",
                cast
            );
            
            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    movie_has_cast 
                VALUES (
                    @fk_movie, 
                    @fk_cast,
                    @character,
                    @aorder
                );",

                new {
                    fk_movie  = movieID,
                    fk_cast   = cast.id,
                    character = cast.character,
                    aorder     = cast.aorder
                }
            );

            // check if info was inserted in db
            if (rowsAffected <= 0) {
                return false;
            }

            dbh.Disconnect(DBConnection);

            return true;
        }

        /// <summary>
        /// Delete a Cast
        /// </summary>
        /// <param name="id">ID of the cast to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>True or False</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
