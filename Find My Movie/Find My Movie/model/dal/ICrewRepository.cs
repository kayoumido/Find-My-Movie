using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICrewRepository {
        List<fmmCrew> GetCrews(string sort);

        fmmCrew GetCrew(int id);

        bool Insert(fmmCrew crew);

        bool Delete(int id);
    }
}
