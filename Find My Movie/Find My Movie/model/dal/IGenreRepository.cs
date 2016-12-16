using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface IGenreRepository {
        List<fmmGenre> GetGenres();

        fmmGenre GetGenre(int id);

        bool Insert(fmmGenre genre, int movieID);

        bool Delete(int id);
    }
}
