using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICollectionRepository {
        List<fmmCollection> GetCollections(string sort);

        fmmCollection GetCollection(int id);

        bool Insert(fmmCollection collection);

        bool Delete(int id);
    }
}
