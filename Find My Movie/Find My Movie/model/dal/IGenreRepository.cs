using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface IGenreRepository {
        List<Genre> GetGenres(string sort);

        Genre GetGenre(int id);

        bool Insert(Genre genre);

        bool Delete(int id);
    }
}
