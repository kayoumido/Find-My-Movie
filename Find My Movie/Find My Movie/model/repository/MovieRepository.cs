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
                LIMIT 15
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

        public List<fmmMovie> Search(string searchValue, string searchTable) {

            string sql = "";

            switch (searchTable) {

                case "All":
                    sql = @"
                        SELECT DISTINCT 
                            m.id,
                            m.imdbid,
                            m.title,
                            m.ogtitle,
                            m.adult,
                            m.budget,
                            m.homepage,
                            m.runtime,
                            m.tagline,
                            m.voteaverage,
                            m.oglanguage,
                            m.overview,
                            m.popularity,
                            m.poster,
                            m.releasedate,
                            m.fk_collection
                        FROM
                            movie m
                        INNER JOIN
                            movie_has_crew mcr
                        ON
                            m.id = mcr.fk_movie
                        INNER JOIN
                            crew cr
                        ON
                            mcr.fk_crew = cr.id
                        INNER JOIN
                            movie_has_cast mca
                        ON
                            m.id = mca.fk_movie
                        INNER JOIN
                            cast ca
                        ON
                            mca.fk_cast = ca.id
                        WHERE
                            m.title LIKE '%" + searchValue + @"%'
                        OR
                            ca.name LIKE '%" + searchValue + @"%'
                        OR
                            (cr.name LIKE '%" + searchValue + @"%'
                        AND 
                            mcr.job = 'Director')
                        ORDER BY
                            title
                        ASC
                    ;";
                    break;

                case "Title":
                    sql = @"
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
                            title LIKE '%" + searchValue + @"%'
                        ORDER BY
                            title
                        ASC
                    ;";
                    break;

                case "Actor":
                    sql = @"
                        SELECT
                            m.id,
                            m.imdbid,
                            m.title,
                            m.ogtitle,
                            m.adult,
                            m.budget,
                            m.homepage,
                            m.runtime,
                            m.tagline,
                            m.voteaverage,
                            m.oglanguage,
                            m.overview,
                            m.popularity,
                            m.poster,
                            m.releasedate,
                            m.fk_collection
                        FROM
                            movie m
                        INNER JOIN
                            movie_has_cast mca
                        ON
                            m.id = mca.fk_movie
                        INNER JOIN
                            cast c
                        ON
                            mca.fk_cast = c.id
                        WHERE
                            c.name LIKE '%" + searchValue + @"%'
                        ORDER BY
                            title
                        ASC
                    ;";
                    break;

                case "Director":

                    sql = @"
                        SELECT
                            m.id,
                            m.imdbid,
                            m.title,
                            m.ogtitle,
                            m.adult,
                            m.budget,
                            m.homepage,
                            m.runtime,
                            m.tagline,
                            m.voteaverage,
                            m.oglanguage,
                            m.overview,
                            m.popularity,
                            m.poster,
                            m.releasedate,
                            m.fk_collection
                        FROM
                            movie m
                        INNER JOIN
                            movie_has_crew mcr
                        ON
                            m.id = mcr.fk_movie
                        INNER JOIN
                            crew c
                        ON
                            mcr.fk_crew = c.id
                        WHERE
                            c.name LIKE '%" + searchValue + @"%'
                        AND mcr.job = 'Director'
                        ORDER BY
                            title
                        ASC
                    ;";

                    break;

                default:
                    break;
            }

            IEnumerable<fmmMovie> movies = db.Query<fmmMovie>(sql);

            return movies.ToList();
        }

        public int MovieExists(string movieName) {
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
                    ogtitle = '" + movieName + "';"
            ;
            IEnumerable<fmmMovie> movie = db.Query<fmmMovie>(sql);

            if (movie.Count() == 0) {
                return 0;
            }
            else {
                return movie.FirstOrDefault().id;
            }

        }
    }
}
