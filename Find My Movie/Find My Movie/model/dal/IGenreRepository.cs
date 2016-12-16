using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface IGenreRepository {
        List<fmmGenre> GetGenres();

        fmmGenre GetGenre(int id);

        bool Insert(fmmGenre genre, int movieID);

        bool Delete(int id);
    }
}
