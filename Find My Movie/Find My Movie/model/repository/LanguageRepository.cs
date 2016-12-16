using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;

namespace Find_My_Movie.model.repository {
    public class LanguageRepository : ILanguageRepository {

        private static dbhandler dbh          = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get a list of fmmLanguage objects
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmLanguage</returns>
        public List<fmmLanguage> GetLanguages() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a language
        /// </summary>
        /// <param name="id">ID of the language to get</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>fmmCrew object of wanted genre</returns>
        public fmmLanguage GetLanguage(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a language in DB
        /// </summary>
        /// <param name="language">fmmLanguage object to insert in DB</param>
        /// <param name="movieID">Id of the movie the language is linked to</param>
        /// <returns>True or false</returns>
        public bool Insert(fmmLanguage language, int movieID) {
            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    language 
                VALUES (
                    @name
                );",
                language
            );

            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    spoken_language
                VALUES (
                    @fk_movie, 
                    @fk_language
                );",

                new {
                    fk_movie    = movieID,
                    fk_language = language.name
                }
            );

            dbh.Disconnect(DBConnection);

            return true;
        }

        /// <summary>
        /// Delete a language
        /// </summary>
        /// <param name="id">ID of the language to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>True or False</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
