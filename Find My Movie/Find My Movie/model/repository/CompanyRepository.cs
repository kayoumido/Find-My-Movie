using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CompanyRepository {
        static private dbhandler dbh = new dbhandler();

        private SQLiteConnection DBConnection = dbh.Connect();

        public List<fmmCompany> GetCompanies(string sort) {
            throw new NotImplementedException();
        }

        public fmmCompany GetCompany(int id) {
            throw new NotImplementedException();
        }

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

            // check if it was inserted
            if (rowsAffected <= 0) {
                return false;
            }

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
