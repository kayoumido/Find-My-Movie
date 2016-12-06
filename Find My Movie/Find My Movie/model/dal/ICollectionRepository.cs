using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICollectionRepository {
        List<Collection> GetCollections(string sort);

        Collection GetCollection(int id);

        bool Insert(Collection collection);

        bool Delete(int id);
    }
}
