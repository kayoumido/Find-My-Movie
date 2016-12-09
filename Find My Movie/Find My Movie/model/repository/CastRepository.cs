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

        public bool Insert(fmmCast cast) {

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
