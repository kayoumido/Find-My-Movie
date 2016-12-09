using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CollectionRepository : ICollectionRepository {

        static private SQLiteConnection db = new dbhandler().Connect();

        public List<fmmCollection> GetCollections(string sort) {
            throw new NotImplementedException();
        }

        public fmmCollection GetCollection(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(fmmCollection collection) {
            int rowsAffected = db.Execute(@"
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

        public bool Delete(int id) {
            int rowsAffected = db.Execute(@"
                DELETE FROM
                    collection
                WHERE
                    id = " + id + ";"
            );

            if (rowsAffected > 0) {
                return true;
            }
            return false;
        }
    }
}
