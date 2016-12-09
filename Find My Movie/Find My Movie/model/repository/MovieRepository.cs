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

        static private SQLiteConnection db = new dbhandler().Connect();

        public fmmMovie GetMovie(int id) {

            string sql = @"
                SELECT
                    id,
                    imdbid,
                    title,
                    ogtitle,
                    adult,
                    budget,
                    homepage,
                    runtime,
                    tagline,
                    voteaverage,
                    oglanguage,
                    overview,
                    popularity,
                    poster,
                    releasedate,
                    fk_collection
                FROM
                    movie
                WHERE
                    id = " + id + ";"
            ;
            IEnumerable<fmmMovie> movie = db.Query<fmmMovie>(sql);
            
            return movie.FirstOrDefault();
        }

        public List<fmmMovie> GetMovies() {
            string sql = @"
                SELECT
                    id,
                    imdbid,
                    title,
                    ogtitle,
                    adult,
                    budget,
                    homepage,
                    runtime,
                    tagline,
                    voteaverage,
                    oglanguage,
                    overview,
                    popularity,
                    poster,
                    releasedate,
                    fk_collection
                FROM
                    movie;"
            ;
            IEnumerable<fmmMovie> movies = db.Query<fmmMovie>(sql);

            return movies.ToList();

        }

        public bool Insert(fmmMovie movie) {

             int rowsAffected = db.Execute(@"
                INSERT OR IGNORE INTO
                    movie
                VALUES (
                    @id,
                    @imdbid,
                    @title,
                    @ogtitle,
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
                    @fk_collection
                );",
            movie
            );

            if (rowsAffected > 0) {
                return true;
            }
            return false;

        }

        public bool Delete(int id) {

            int rowsAffected = db.Execute(@"
                DELETE FROM
                    movie
                WHERE
                    id = " + id + ";"
            );

            if (rowsAffected > 0) {
                return true;
            }
            return false;

        }

        public List<fmmCast> GetMovieCasts(int movieId) {
            throw new NotImplementedException();
        }

        public List<fmmGenre> GetMovieGenres(int movieId) {
            throw new NotImplementedException();
        }

    }
}
