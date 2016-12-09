using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    internal interface IMovieRepository {

        List<fmmMovie> GetMovies();

        fmmMovie GetMovie(int id);

        bool InsertMovie(fmmMovie movie);

        bool DeleteMovie(int id);

        List<fmmCast> GetMovieCasts(int movieId);

        List<fmmGenre> GetMovieGenres(int movieId);
        
    }
}
