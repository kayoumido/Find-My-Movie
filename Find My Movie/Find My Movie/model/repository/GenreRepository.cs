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

        public bool Insert(fmmGenre genre) {
            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    genre 
                VALUES (
                    @id, 
                    @name
                );",
                genre
            );

            dbh.Disconnect(DBConnection);

            if (rowsAffected > 0) {
                return true;
            }
            return false;
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
