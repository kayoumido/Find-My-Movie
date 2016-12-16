using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface ICollectionRepository {
        List<fmmCollection> GetCollections();

        fmmCollection GetCollection(int id);

        bool Insert(fmmCollection collection);

        bool Delete(int id);
    }
}
