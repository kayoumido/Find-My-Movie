using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    internal interface IMovieRepository {

        List<Movie> GetMovies();

        Movie GetMovie(int id);

        bool InsertMovie(Movie movie);

        bool DeleteMovie(int id);

        List<Cast> GetMovieCasts(int movieId);
        
    }
}
