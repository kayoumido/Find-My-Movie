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

        public bool Insert(fmmCountry country, int movieID) {

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

            // check if it was inserted
            if (rowsAffected <= 0) {
                return false;
            }

            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    production_country
                VALUES (
                    @fk_movie, 
                    @fk_country
                );",

                new {
                    fk_movie   = movieID,
                    fk_country = country.name
                }
            );

            // check if info was inserted in db
            if (rowsAffected <= 0) {
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
