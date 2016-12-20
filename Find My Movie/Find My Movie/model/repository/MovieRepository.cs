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

        private static dbhandler dbh = new dbhandler();
        private SQLiteConnection DBConnection = dbh.Connect();

        /// <summary>
        /// Get all the movies from the database
        /// </summary>
        /// <returns>A list of movie objects containing the information of a movie</returns>
        public List<fmmMovie> GetMovies() {
            string sql = @"
                SELECT
                    id,
                    imdbid,
                    title,
                    ogtitle,
                    filename,
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
            IEnumerable<fmmMovie> movies = DBConnection.Query<fmmMovie>(sql);

            return movies.ToList();

        }

        /// <summary>
        /// Get one movie from the database
        /// </summary>
        /// <param name="id">id of the movie we want to get</param>
        /// <returns>A movie object containing the information of the movie</returns>
        public fmmMovie GetMovie(int id) {

            string sql = @"
                SELECT
                    id,
                    imdbid,
                    title,
                    ogtitle,
                    filename,
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
            IEnumerable<fmmMovie> movie = DBConnection.Query<fmmMovie>(sql);

            return movie.FirstOrDefault();
        }

        /// <summary>
        /// Add a new entry into the database only if it doesn't exist
        /// </summary>
        /// <param name="movie">A movie object containing the information of the movie</param>
        /// <returns>A boolean to inform if anything was added</returns>
        public bool Insert(fmmMovie movie) {

             int rowsAffected = DBConnection.Execute(@"
                INSERT OR IGNORE INTO
                    movie
                VALUES (
                    @id,
                    @imdbid,
                    @title,
                    @ogtitle,
                    @filename,
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

        /// <summary>
        /// Remove the movie from the database
        /// </summary>
        /// <param name="id">id of the movie we want to delete</param>
        /// <returns>A boolean to inform if anything was deleted</returns>
        public bool Delete(int id) {
            int rowsAffected = DBConnection.Execute(@" 
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

        /// <summary>
        /// Get all the casts for a movie
        /// </summary>
        /// <param name="movieId">id of the movie for which we want the casts</param>
        /// <returns>A list of cast objects containing the information of a cast</returns>
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
            IEnumerable<fmmCast> casts = DBConnection.Query<fmmCast>(sql);

            return casts.ToList();
        }

        /// <summary>
        /// Get all the genres for a movie
        /// </summary>
        /// <param name="movieId">id of the movie for which we want the genres</param>
        /// <returns>A list of genre objects containing the information of a genre</returns>
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

            IEnumerable<fmmGenre> genres = DBConnection.Query<fmmGenre>(sql);

            return genres.ToList();

        }

        /// <summary>
        /// Get all the crew for a movie
        /// </summary>
        /// <param name="movieId">id of the movie for which we want the crew</param>
        /// <returns>A list of crew objects containing the information of a crew</returns>
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

            IEnumerable<fmmCrew> crews = DBConnection.Query<fmmCrew>(sql);

            return crews.ToList();
        }

        /// <summary>
        /// Check if a movie is in the database
        /// </summary>
        /// <param name="value">Name of the movie after going through the RegEx</param>
        /// <returns>0 to indicate that there isn't a movie or the id of the movie</returns>
        public int MovieExists(string column, string value) {
            string sql = @"
                SELECT
                    id,
                    imdbid,
                    title,
                    ogtitle,
                    filename,
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
                    " + column + @" = '" + value + @"'
            ;";

            IEnumerable<fmmMovie> movie = DBConnection.Query<fmmMovie>(sql);

            if (movie.Count() == 0) {
                return 0;
            }
            else {
                return movie.FirstOrDefault().id;
            }

        }

        /// <summary>
        /// Get the movies that meet the search criteria
        /// </summary>
        /// <param name="searchValue">The string to search in the database</param>
        /// <param name="searchTable">Indicates the table that needs to be searched</param>
        /// <returns>A list of movie objects containing the information of a movie</returns>
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
                            m.filename,
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
                            m.ogtitle LIKE '%" + searchValue + @"%'
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
                            filename,
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
                        OR
                            ogtitle LIKE '%" + searchValue + @"%'
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
                            m.filename,
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
                            m.filename,
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

            IEnumerable<fmmMovie> movies = DBConnection.Query<fmmMovie>(sql);

            return movies.ToList();
        }

        /// <summary>
        /// Get the movies that meet the filter criteria
        /// </summary>
        /// <param name="genres">A list of genre ids</param>
        /// <param name="years">An array for the year from and to (this attribut doesn't need to be passed)</param>
        /// <returns>A list of movie objects containing the information of a movie</returns>
        public List<fmmMovie> Filter(List<int> genres, int[] years = null) {

            string sql = @"
                SELECT
                    m.id,
                    m.imdbid,
                    m.title,
                    m.ogtitle,
                    m.filename,
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
					movie_has_genre mg
				ON
					m.id = mg.fk_movie
				INNER JOIN
					genre g
				ON
					mg.fk_genre = g.id
            ";

            if (genres.Count > 0) {
                sql += @"
                    WHERE
                        g.id
                            IN
                        (";
                foreach (int id in genres) {
                    if (!id.Equals(genres.Last())) {
                        sql += id + ",";
                    }
                    else {
                        sql += id;
                    }
                }
                sql += @")";
            }

            if (years != null) {
                if (genres.Count > 0) {
                    sql += @"
                        AND
                    ";
                }
                else {
                    sql += @"
                        WHERE
                    ";
                }
                sql += @"
                    substr(m.releasedate, 7, 4)
                        BETWEEN
                    '" + years[0] + @"'
                        AND
                    '" + years[1] + @"'";
            }

            sql += @"
                GROUP BY
					m.id
				HAVING
					count(g.id) >= " + genres.Count + @"
                ORDER BY
                    m.title
                ASC
            ;";

            IEnumerable<fmmMovie> movies = DBConnection.Query<fmmMovie>(sql);

            return movies.ToList();
        }
    }
}
