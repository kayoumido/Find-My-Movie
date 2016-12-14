using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CrewRepository : ICrewRepository {

        static private dbhandler dbh = new dbhandler();

        private SQLiteConnection DBConnection = dbh.Connect();
        

        public List<fmmCrew> GetCrews(string sort) {
            throw new NotImplementedException();
        }

        public fmmCrew GetCrew(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a Crew in database
        /// </summary>
        /// <param name="crew">fmmCrew object to insert in db</param>
        /// <returns>True if insert was successful otherwise false</returns>
        /// 
        /// <author>Doran Kayoumi</author>
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


            // check if it was inserted
            /*if (rowsAffected <= 0) {
                return false;
            }*/

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
