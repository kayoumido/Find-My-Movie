using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using TMDbLib.Objects.Movies;

namespace Find_My_Movie {
    class dbhandler {
        /// <summary>
        /// Class constructor, Creates FMM database if it doesn't exist
        /// </summary>
        /// 
        /// <author>Doran Kayoumi</author>
        public dbhandler() {


            // create new SQLiteConnection to database
            SQLiteConnection connection = new SQLiteConnection("Data Source=FMMDb.db;Version=3;");
            // open connection to db, if the db doesn't exists, it will be created
            connection.Open();

            
            // create tables for db
            string sql = @"
                CREATE TABLE IF NOT EXISTS `collection` (
                  `id` INT NOT NULL,
                  `name` TEXT NOT NULL,
                  `poster` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;
                CREATE TABLE IF NOT EXISTS `movie` (
                  `id` INT NOT NULL,
                  `imdbid` TEXT NOT NULL,
                  `title` TEXT NOT NULL,
                  `ogtitle` TEXT NOT NULL,
                  `alttitle` TEXT NULL,
                  `adult` INT NOT NULL,
                  `budget` INT NULL,
                  `homepage` TEXT NOT NULL,
                  `runtime` INT NOT NULL,
                  `tagline` TEXT NOT NULL,
                  `voteaverage` REAL NOT NULL,
                  `oglanguage` TEXT NOT NULL,
                  `overview` TEXT NOT NULL,
                  `popularity` REAL NOT NULL,
                  `poster` TEXT NOT NULL,
                  `releasedate` TEXT NOT NULL,
                  `fk_collection` INT NOT NULL,
                  PRIMARY KEY (`id`),
                  CONSTRAINT `fk_movie_collection1`
                    FOREIGN KEY (`fk_collection`)
                    REFERENCES `collection` (`id`)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION)
                ;

                CREATE TABLE IF NOT EXISTS `cast` (
                  `id` INT NOT NULL,
                  `castid` INT NOT NULL,
                  `creditid` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  `image` TEXT NOT NULL,
                  `character` TEXT NOT NULL,
                  `order` INT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `crew` (
                  `id` INT NOT NULL,
                  `creditid` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  `image` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `genre` (
                  `id` INT NOT NULL,
                  `name` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `company` (
                  `id` INT NOT NULL,
                  `name` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `country` (
                  `id` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `language` (
                  `id` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `keyword` (
                  `id` INT NOT NULL,
                  `name` TEXT NOT NULL,
                  PRIMARY KEY (`id`))
                ;

                CREATE TABLE IF NOT EXISTS `movie_has_keyword` (
                  `fk_movie` INT NOT NULL,
                  `fk_keyword` INT NOT NULL,
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
                  `fk_movie` INT NOT NULL,
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
                  `fk_movie` INT NOT NULL,
                  `fk_cast` INT NOT NULL,
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
                  `fk_movie` INT NOT NULL,
                  `fk_crew` INT NOT NULL,
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
                  `fk_movie` INT NOT NULL,
                  `fk_genre` INT NOT NULL,
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
                  `fk_movie` INT NOT NULL,
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
                  `fk_movie` INT NOT NULL,
                  `fk_company` INT NOT NULL,
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
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            // execute command
            command.ExecuteNonQuery();
           
            // end connection to DB
            connection.Close();
        }

        public void InsertInDB(Movie movie, Credits cast) {

        }

        private void InsertCollection() {

        }
    }
}
