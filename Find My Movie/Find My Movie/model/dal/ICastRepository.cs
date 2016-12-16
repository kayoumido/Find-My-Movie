using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface ICastRepository {
        List<fmmCast> GetCasts();

        fmmCast GetCast(int id);

        bool Insert(fmmCast cast, int movieID);

        bool Delete(int id);
    }
}
