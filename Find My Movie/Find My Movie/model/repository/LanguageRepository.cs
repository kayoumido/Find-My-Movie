using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    public class LanguageRepository : ILanguageRepository {

        static private dbhandler dbh = new dbhandler();

        private SQLiteConnection DBConnection = dbh.Connect();

        public List<fmmLanguage> GetLanguages(string sort) {
            throw new NotImplementedException();
        }

        public fmmLanguage GetLanguage(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(fmmLanguage language) {
            int rowsAffected = this.DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    language 
                VALUES (
                    @name
                );",
                language
            );

            dbh.Disconnect(DBConnection);

            if (rowsAffected > 0) {
                return true;
            }
            return false;
            throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
