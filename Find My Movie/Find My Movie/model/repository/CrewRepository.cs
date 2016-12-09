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
        public bool Insert(fmmCrew crew) {

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
