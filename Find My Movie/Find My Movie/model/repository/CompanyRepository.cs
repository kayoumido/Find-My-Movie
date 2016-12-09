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

        public bool Insert(fmmCompany company) {

            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    company 
                VALUES (
                    @id,
                    @name
                );",
                company
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
