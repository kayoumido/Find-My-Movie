using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;

namespace Find_My_Movie.model.repository {
    class CountryRepository : ICountryRepository {

        private static dbhandler dbh          = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get a list of fmmCountry objects
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmCountry</returns>
        public List<fmmCountry> GetCountries() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a country
        /// </summary>
        /// <param name="id">ID of the country to get</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>fmmCountry object of wanted country</returns>
        public fmmCountry GetCountry(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a country in DB
        /// </summary>
        /// <param name="country">fmmCountry object to insert in DB</param>
        /// <param name="movieID">Id of the movie the country is linked to</param>
        /// <returns>True or false</returns>
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

        /// <summary>
        /// Delete a country
        /// </summary>
        /// <param name="id">ID of the country to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>True or False</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
