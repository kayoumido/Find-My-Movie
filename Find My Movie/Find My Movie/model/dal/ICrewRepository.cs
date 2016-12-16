using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface ICrewRepository {
        List<fmmCrew> GetCrews();

        fmmCrew GetCrew(int id);

        bool Insert(fmmCrew crew, int movieID);

        bool Delete(int id);
    }
}
