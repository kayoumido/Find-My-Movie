using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Find_My_Movie.model.repository {
    class CompanyRepository {

        private static dbhandler dbh          = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get a list of fmmCompany objects
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmCompany</returns>
        public List<fmmCompany> GetCompanies() {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get a company
        /// </summary>
        /// <param name="id">ID of the company to get</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>fmmCompany object of wanted company</returns>
        public fmmCompany GetCompany(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a company in DB
        /// </summary>
        /// <param name="company">fmmCompany object to insert in DB</param>
        /// <param name="movieID">Id of the movie the company is linked to</param>
        /// <returns>True or false</returns>
        public bool Insert(fmmCompany company, int movieID) {

            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    company 
                VALUES (
                    @id,
                    @name
                );",
                company
            );

            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    production_company
                VALUES (
                    @fk_movie, 
                    @fk_company
                );",

                new {
                    fk_movie   = movieID,
                    fk_company = company.id
                }
            );

            dbh.Disconnect(DBConnection);

            return true;

        }

        /// <summary>
        /// Delete a company
        /// </summary>
        /// <param name="id">ID of the company to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>True or False</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
