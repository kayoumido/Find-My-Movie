using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    internal interface IMovieRepository {

        List<fmmMovie> GetMovies();

        fmmMovie GetMovie(int id);

        bool Insert(fmmMovie movie);

        bool Delete(int id);

        List<fmmCast> GetMovieCasts(int movieId);

        List<fmmCrew> GetMovieCrews(int movieId);

        List<fmmGenre> GetMovieGenres(int movieId);

        int MovieExists(string column, string value);

        List<fmmMovie> Search(string searchValue, string searchTable);

        List<fmmMovie> Filter(List<int> genres, int[] years = null);


    }
}
