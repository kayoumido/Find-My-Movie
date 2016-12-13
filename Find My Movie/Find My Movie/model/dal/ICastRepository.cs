using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICastRepository {
        List<fmmCast> GetCasts(string sort);

        fmmCast GetCast(int id);

        bool Insert(fmmCast cast, int movieID);

        bool Delete(int id);
    }
}
