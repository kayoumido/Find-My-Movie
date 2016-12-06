using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICastRepository {
        List<Cast> GetCasts(string sort);

        Cast GetCast(int id);

        bool Insert(Cast cast);

        bool Delete(int id);
    }
}
