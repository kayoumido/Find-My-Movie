using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Find_My_Movie.model.repository {
    class CollectionRepository : ICollectionRepository {

        private static dbhandler dbh = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get all the collections from the database
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmCasts</returns>
        public List<fmmCollection> GetCollections() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get one collection from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public fmmCollection GetCollection(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a new entry into the database only if it doesn't exist
        /// </summary>
        /// <param name="collection">A collection object containing the information of the movie</param>
        /// <returns>A boolean to inform if anything was added</returns>
        public bool Insert(fmmCollection collection) {
            int rowsAffected = DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    collection
                VALUES (
                    @id,
                    @name,
                    @poster
                );",
            collection
            );

            if (rowsAffected > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the collection from the database
        /// </summary>
        /// <param name="id">id of the collection we want to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>A boolean to inform if anything was added</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
