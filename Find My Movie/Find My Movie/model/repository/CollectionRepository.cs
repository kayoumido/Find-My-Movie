using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CollectionRepository : ICollectionRepository {
        public List<Collection> GetCollections(string sort) {
            throw new NotImplementedException();
        }

        public Collection GetCollection(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(Collection collection) {
            throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
