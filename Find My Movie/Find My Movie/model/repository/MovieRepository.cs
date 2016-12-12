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
                    movie
                ORDER BY
                    title
                ASC
            ;";
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

            string sql = @"
                SELECT
                    c.id,
                    c.castid,
                    c.creditid,
                    c.name,
                    c.image,
                    mca.character,
                    mca.aorder
                FROM
                    cast c
                INNER JOIN
                    movie_has_cast mca
                ON
                    c.id = mca.fk_cast
                INNER JOIN
                    movie m
                ON
                    mca.fk_movie = m.id
                WHERE
                    m.id = " + movieId + @"
                ORDER BY
                    mca.aorder
                ASC
            ;";
            IEnumerable<fmmCast> casts = db.Query<fmmCast>(sql);

            return casts.ToList();
        }

        public List<fmmGenre> GetMovieGenres(int movieId) {

            string sql = @"
                SELECT
                    g.id,
                    g.name
                FROM
                    genre g
                INNER JOIN
                    movie_has_genre mg
                ON
                    g.id = mg.fk_genre
                INNER JOIN
                    movie m
                ON
                    mg.fk_movie = m.id
                WHERE
                    m.id = " + movieId + ";";

            IEnumerable<fmmGenre> genres = db.Query<fmmGenre>(sql);

            return genres.ToList();

        }

        public List<fmmCrew> GetMovieCrews(int movieId) {
            string sql = @"
                SELECT
                    c.id,
                    c.creditid,
                    c.name,
                    c.image,
                    mcr.department,
                    mcr.job
                FROM
                    crew c
                INNER JOIN
                    movie_has_crew mcr
                ON
                    c.id = mcr.fk_crew
                INNER JOIN
                    movie m
                ON
                    mcr.fk_movie = m.id
                WHERE
                    m.id = " + movieId + ";";

            IEnumerable<fmmCrew> crews = db.Query<fmmCrew>(sql);

            return crews.ToList();
        }
    }
}
