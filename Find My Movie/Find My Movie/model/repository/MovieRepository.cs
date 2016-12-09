using Dapper;
using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {

    class MovieRepository : IMovieRepository {

        private SQLiteConnection db = new dbhandler().Connect();

        public fmmMovie GetMovie(int id) {
            throw new NotImplementedException();
        }

        public List<fmmCast> GetMovieCasts(int movieId) {
            throw new NotImplementedException();
        }

        public List<fmmGenre> GetMovieGenres(int movieId) {
            throw new NotImplementedException();
        }

        public List<fmmMovie> GetMovies() {
            throw new NotImplementedException();
        }

        public bool InsertMovie(fmmMovie movie) {

            db.Execute(@"INSERT INTO movie VALUES (@id,
                                                    @imdbid,
                                                    @title,
                                                    @ogtitle,
                                                    @alttitle,
                                                    @adult,
                                                    @budget,
                                                    @homepage,
                                                    @runtime,
                                                    @tagline,
                                                    @voteaverage,
                                                    @oglanguage,
                                                    @overview,
                                                    @popularity,
                                                    @poster,
                                                    @releasedate,
                                                    @fk_collection);", movie);

            return true;

        }

        public bool DeleteMovie(int id) {
            throw new NotImplementedException();
        }
    }
}
