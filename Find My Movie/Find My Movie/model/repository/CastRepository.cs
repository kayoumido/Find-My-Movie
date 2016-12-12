using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    public class CastRepository : ICastRepository {

        static private dbhandler dbh = new dbhandler();

        private SQLiteConnection DBConnection = dbh.Connect();

        public List<fmmCast> GetCasts(string sort) {
            throw new NotImplementedException();
        }

        public fmmCast GetCast(int id) {
            throw new NotImplementedException();
        }

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


            // check if it was inserted
            if (rowsAffected < 0) {
                return false;
            }

            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    movie_has_cast 
                VALUES (
                    @fk_movie, 
                    @fk_cast,
                    @character,
                    @order
                );",

                new {
                    fk_movie  = movieID,
                    fk_cast   = cast.id,
                    character = cast.character,
                    order     = cast.order
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
