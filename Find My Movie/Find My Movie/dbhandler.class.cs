using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie {
    class dbhandler {

        private SQLiteConnection connection;

        public dbhandler() {

            this.Connect();

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

            SQLiteCommand command = new SQLiteCommand(sql, this.connection);
            command.ExecuteNonQuery();

            this.Disconnect();
        }

        /// <summary>
        /// Initialize connection to DB
        /// </summary>
        /// <returns>SQLiteConnection object</returns>
        /// 
        /// <author>Doran Kayoumi</author>
        public void Connect() {
            
            this.connection = new SQLiteConnection("Data Source=FMMDb.db;Version=3;");
            
            this.connection.Open();
        }

        /// <summary>
        /// End conection to database
        /// </summary>
        /// <param name="connection">SQLiteConnection that initialized connection</param>
        /// 
        /// <author>Doran Kayoumi</author>
        public void Disconnect() {
            
            this.connection.Close();

            this.connection = null;
        }
    }
}
