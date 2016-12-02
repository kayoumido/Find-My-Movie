using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;



namespace Find_My_Movie {
    class dbhandler {

        private SQLiteConnection connection;

        /// <summary>
        /// Class constructor, Creates FMM database if it doesn't exist
        /// </summary>
        /// 
        /// <author>Doran Kayoumi</author>
        public dbhandler() {

            // connect to DB
            this.Connect();


            // create tables for db
            string sql = @"
                CREATE TABLE IF NOT EXISTS `collection` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `name` TEXT NOT NULL,
                  `poster` TEXT NOT NULL)
                ;
                CREATE TABLE IF NOT EXISTS `movie` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `imdbid` TEXT NOT NULL,
                  `title` TEXT NOT NULL,
                  `ogtitle` TEXT NOT NULL,
                  `alttitle` TEXT NULL,
                  `adult` INTEGER NOT NULL,
                  `budget` INTEGER NULL,
                  `homepage` TEXT NOT NULL,
                  `runtime` INTEGER NOT NULL,
                  `tagline` TEXT NOT NULL,
                  `voteaverage` REAL NOT NULL,
                  `oglanguage` TEXT NOT NULL,
                  `overview` TEXT NOT NULL,
                  `popularity` REAL NOT NULL,
                  `poster` TEXT NOT NULL,
                  `releasedate` TEXT NOT NULL,
                  `fk_collection` INTEGER NOT NULL,
                  CONSTRAINT `fk_movie_collection1`
                    FOREIGN KEY (`fk_collection`)
                    REFERENCES `collection` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `cast` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `castid` INTEGER NOT NULL,
                  `creditid` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  `image` TEXT NOT NULL,
                  `character` TEXT NOT NULL,
                  `order` INTEGER NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `crew` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `creditid` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  `image` TEXT NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `genre` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `name` TEXT NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `company` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `name` TEXT NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `country` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `name` TEXT NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `language` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `name` TEXT NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `keyword` (
                  `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                  `name` TEXT NOT NULL)
                ;

                CREATE TABLE IF NOT EXISTS `movie_has_keyword` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_keyword` INTEGER NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_keyword`),
                  CONSTRAINT `fk_movie_has_keyword_movie`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_keyword_keyword1`
                    FOREIGN KEY (`fk_keyword`)
                    REFERENCES `keyword` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `spoken_language` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_language` TEXT NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_language`),
                  CONSTRAINT `fk_movie_has_language_movie1`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_language_language1`
                    FOREIGN KEY (`fk_language`)
                    REFERENCES `language` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `movie_has_cast` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_cast` INTEGER NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_cast`),
                  CONSTRAINT `fk_movie_has_cast_movie1`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_cast_cast1`
                    FOREIGN KEY (`fk_cast`)
                    REFERENCES `cast` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `movie_has_crew` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_crew` INTEGER NOT NULL,
                  `department` TEXT NOT NULL,
                  `job` TEXT NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_crew`),
                  CONSTRAINT `fk_movie_has_crew_movie1`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_crew_crew1`
                    FOREIGN KEY (`fk_crew`)
                    REFERENCES `crew` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `movie_has_genre` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_genre` INTEGER NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_genre`),
                  CONSTRAINT `fk_movie_has_genre_movie1`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_genre_genre1`
                    FOREIGN KEY (`fk_genre`)
                    REFERENCES `genre` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `production_country` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_country` TEXT NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_country`),
                  CONSTRAINT `fk_movie_has_country_movie1`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_country_country1`
                    FOREIGN KEY (`fk_country`)
                    REFERENCES `country` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `production_company` (
                  `fk_movie` INTEGER NOT NULL,
                  `fk_company` INTEGER NOT NULL,
                  PRIMARY KEY (`fk_movie`, `fk_company`),
                  CONSTRAINT `fk_movie_has_company_movie1`
                    FOREIGN KEY (`fk_movie`)
                    REFERENCES `movie` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION,
                  CONSTRAINT `fk_movie_has_company_company1`
                    FOREIGN KEY (`fk_company`)
                    REFERENCES `company` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;
            ";
            // prepare new sql command
            SQLiteCommand command = new SQLiteCommand(sql, this.connection);
            // execute command
            command.ExecuteNonQuery();

            // end connection to DB
            this.Disconnect();
        }

        /// <summary>
        /// Initialize connection to DB
        /// </summary>
        /// <returns>SQLiteConnection object</returns>
        /// 
        /// <author>Doran Kayoumi</author>
        private void Connect() {
            // create new SQLiteConnection to database
            this.connection = new SQLiteConnection("Data Source=FMMDb.db;Version=3;");
            // open connection to db, if the db doesn't exists, it will be created
            this.connection.Open();
        }

        /// <summary>
        /// End conection to database
        /// </summary>
        /// <param name="connection">SQLiteConnection that initialized connection</param>
        /// 
        /// <author>Doran Kayoumi</author>
        private void Disconnect() {
            // en connection
            this.connection.Close();

            // empty out variable
            this.connection = null;
        }



        /// REM

        /// <summary>
        /// Execute an insert type request
        /// </summary>
        /// <param name="connection">Connection to DB</param>
        /// <param name="sql">Request to execute</param>
        /// 
        /// <author>Doran Kayoumi</author>
        private void ExecuteInsert(string sql) {
            // prepare new sql command
            SQLiteCommand command = new SQLiteCommand(sql, this.connection);
            // execute command
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute a select type request
        /// </summary>
        /// <param name="connection">Connection to DB</param>
        /// <param name="sql">Request to execute</param>
        /// <returns>Request result</returns>
        /// 
        /// <author>Doran Kayoumi</author>
        private SQLiteDataReader ExecuteSelect(string sql) {
            // prepare new sql command
            SQLiteCommand command = new SQLiteCommand(sql, this.connection);
            // execute command
            return command.ExecuteReader();
        }

        /// <summary>
        /// Insert data in DB
        /// </summary>
        /// <param name="movie">Movie object to insert</param>
        /// <param name="cast">Credit object containing movie cast</param>
        /// 
        /// <author>Doran Kayoumi</author>
        public void InsertInDB(Movie movie, Credits cast) {
            // open connection to DB
            this.Connect();

            if (movie.BelongsToCollection != null) {
                MessageBox.Show(movie.BelongsToCollection.ToString());

                MessageBox.Show("");

                this.InsertCollection(movie.BelongsToCollection);
            }
            this.InsertMovie(movie);

            // end connection to DB
            this.Disconnect();
        }

        /// <summary>
        /// Insert collection in database
        /// </summary>
        /// <param name="collection"></param>
        private void InsertCollection(SearchCollection collection) {

            // search for select in DB
            string sql = "select name from collection where name = '" + collection.Name + "';";

            // execute request
            SQLiteDataReader reader = this.ExecuteSelect(sql);
            // check if select returned anything
            if (!reader.HasRows) {
                // creating SQL request to insert data
                sql = "insert into collection values(null, '" + collection.Name + "', '" + collection.PosterPath + "');";

                this.ExecuteInsert(sql);
            }
        }

        /// <summary>
        /// Insert movie in database
        /// </summary>
        /// <param name="movie">Movie object returned from api</param>
        /// 
        /// <author>Doran Kayoumi</author>
        private void InsertMovie(Movie movie) {
            // check if movie is already in DB
            // create request
            string sql = "select id from movie where id = '" + movie.Id + "';";


            // execute request
            SQLiteDataReader reader = this.ExecuteSelect(sql);

            // check if movie was found
            if (reader.HasRows) {
                // stop execution of method
                return;
            }

            sql = @"insert into movie values("
                + movie.Id + ",'"
                + movie.ImdbId + "','"
                + movie.Title + "','"
                + movie.OriginalTitle + "','"
                + movie.AlternativeTitles + "',"
                + Convert.ToInt32(movie.Adult) + ","
                + movie.Budget + ",'"
                + movie.Homepage + "',"
                + movie.Runtime + ",'"
                + movie.Tagline + "',"
                + movie.VoteAverage + ",'"
                + movie.OriginalLanguage + "','"
                + movie.Overview + "',"
                + movie.Popularity + ",'"
                + movie.PosterPath + "','"
                + movie.ReleaseDate + "',"

            ;

            if (movie.BelongsToCollection != null) {
                // get id of collection linked to movie
                string sqlCollection = "select id from collection where name = '" + movie.BelongsToCollection.Name + "';";
                reader = this.ExecuteSelect(sqlCollection);

                // check if collection was found
                if (reader.HasRows) {
                    sql += reader["id"].ToString() + ");";
                }
            }
            else {
                sql += ");";
            }


            this.ExecuteInsert(sql);

        }
    }
}
