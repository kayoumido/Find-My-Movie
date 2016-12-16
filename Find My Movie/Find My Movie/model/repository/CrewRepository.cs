using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Find_My_Movie.model.repository {
    class CrewRepository : ICrewRepository {

        private static dbhandler dbh          = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get a list of fmmCrew objects
        /// </summary>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>List of fmmCrew</returns>
        public List<fmmCrew> GetCrews() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a crew
        /// </summary>
        /// <param name="id">ID of the crew to get</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>fmmCrew object of wanted crew</returns>
        public fmmCrew GetCrew(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a crew in DB
        /// </summary>
        /// <param name="crew">fmmCrew object to insert in DB</param>
        /// <param name="movieID">Id of the movie the crew is linked to</param>
        /// <returns>True or false</returns>
        public bool Insert(fmmCrew crew, int movieID) {

            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    crew 
                VALUES (
                    @id, 
                    @creditid, 
                    @name,
                    @image
                );",
                crew
            );

            rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    movie_has_crew 
                VALUES (
                    @fk_movie, 
                    @fk_crew,
                    @department,
                    @job
                );",

                new {
                    fk_movie   = movieID,
                    fk_crew    = crew.id,
                    department = crew.department,
                    job        = crew.job
                }
            );

            dbh.Disconnect(DBConnection);

            return true;
        }

        /// <summary>
        /// Delete a crew
        /// </summary>
        /// <param name="id">ID of the crew to delete</param>
        /// <notes>We didn't have to use of this method so we didn't implement it</notes>
        /// <returns>True or False</returns>
        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
