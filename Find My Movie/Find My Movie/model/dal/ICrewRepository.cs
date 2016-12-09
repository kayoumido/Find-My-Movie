using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICrewRepository {
        List<Crew> GetCrews(string sort);

        Crew GetCrew(int id);

        bool Insert(Crew crew);

        bool Delete(int id);
    }
}
