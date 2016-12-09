using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CountryRepository : ICountryRepository {
        static private dbhandler dbh = new dbhandler();

        private SQLiteConnection DBConnection = dbh.Connect();

        public List<fmmCountry> GetCountries(string sort) {
            throw new NotImplementedException();
        }

        public fmmCountry GetCountry(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(fmmCountry country) {

            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    country(
                        name                    
                    )
                VALUES (
                    @name
                );",
                country
            );

            dbh.Disconnect(DBConnection);

            if (rowsAffected > 0) {
                return true;
            }
            return false;
            // throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
