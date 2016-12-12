using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class GenreRepository : IGenreRepository {

        static private dbhandler dbh = new dbhandler();

        private SQLiteConnection DBConnection = dbh.Connect();

        public List<fmmGenre> GetGenres(string sort) {
            throw new NotImplementedException();
        }

        public fmmGenre GetGenre(int id) {
            throw new NotImplementedException();
        }

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

            // check if it was inserted
            if (rowsAffected < 0) {
                return false;
            }

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

            // check if info was inserted in db
            if (rowsAffected < 0) {
                return false;
            }

            dbh.Disconnect(DBConnection);

            return true;
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
