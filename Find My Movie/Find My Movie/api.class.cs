using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Find_My_Movie {
    class api {
        // The Movie DB api key
        private string key        = "88cf1d08f60e20cf9f7d3f49e82e7c8f";



        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="file_name">name of the file to get info</param>
        /// 
        /// <author>Doran Kayoumi</author>
        public api(string file_name) {
            // set class attribute with file name
            this.file_name = file_name;
        }

        /// <summary>
        /// Extract information from file name.
        /// e.g. 
        /// After.Earth.2013.TRUEFRENCH.DVDRiP.XViD-AViTECH --> Name : After Earth , Date : 2013
        /// </summary>
        /// <param name="file_name">Name of the file to extract information from</param>
        /// <returns>Returns a match object</returns>
        /// 
        /// <author>Doran Kayoumi</author>
        private Match ExtractDataFromFileName(string file_name) {
            // building new regex object
            // regex found : http://stackoverflow.com/questions/34712335/get-title-and-year-from-file-name-using-regex
            // regex extracts movie title and date from a file name like : Big.Hero.6.2014.FRENCH.BRRip.XviD-DesTroY.avi
            // files without a date in them won't work
            // @FIXME
            Regex regex = new Regex(@"^(.+?)[.( \t]*(?:(19\d{2}|20(?:0\d|1[0-6])).*|(?:(?=bluray|\d+p|brrip)..*)?[.](mkv|avi|mpe?g|divx)$)i");

            // Match file name with regex
            Match match = regex.Match(this.file_name);

            // return result
            return match;
        }

        public string GetMovieName() {
            // get movie info from file name
            Match info = this.ExtractDataFromFileName();

            // return movie name without "." between words
            return info.Groups[1].ToString().Replace(".", " ");
        }

        public string GetMovieDate() {
            // get movie info from file name
            Match info = this.ExtractDataFromFileName();

            // return movie date
            return info.Groups[2].ToString();
        }
 
    }
}
