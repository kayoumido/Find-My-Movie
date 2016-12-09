using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CollectionRepository : ICollectionRepository {
        public List<fmmCollection> GetCollections(string sort) {
            throw new NotImplementedException();
        }

        public fmmCollection GetCollection(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(fmmCollection collection) {
            throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
