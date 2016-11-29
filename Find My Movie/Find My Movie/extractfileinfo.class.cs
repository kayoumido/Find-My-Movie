using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;

namespace Find_My_Movie {
    class extractfileinfo {

        private Match file_info;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="file_name">name of the file to get info</param>
        /// 
        /// <author>Doran Kayoumi</author>
        public extractfileinfo(string file_name) {
            this.GetData(file_name);
        }

        /// <summary>
        /// Extract information from file name.
        /// e.g. 
        /// After.Earth.2013.TRUEFRENCH.DVDRiP.XViD-AViTECH --> Name : After Earth , Date : 2013
        /// </summary>
        /// <param name="file_name">Name of the file to extract information from</param>
        /// 
        /// <author>Doran Kayoumi</author>
        private void GetData(string file_name) {
            // building new regex object
            // regex found : http://stackoverflow.com/questions/34712335/get-title-and-year-from-file-name-using-regex
            // regex extracts movie title and date from a file name like : Big.Hero.6.2014.FRENCH.BRRip.XviD-DesTroY.avi
            // files without a date in them won't work
            // @FIXME This regex doesn't match all file names
            
            // OLD Regex regex = new Regex(@"^(.+?)[.( \t]*(?:(19\d{2}|20(?:0\d|1[0-6])).*|(?:(?=bluray|\d+p|brrip)..*)?[.](mkv|avi|mpe?g|divx)$)i");
            Regex regex = new Regex(@"(\[.+\]|)(.*?)(dvdrip|byPhilou|TRUEFRENCH|READNFO|avi|\.avi|EDITION|FRENCH|xvid| cd[0-9]|dvdscr|brrip|divx|[\{\(\[]?[0-9]{4}).*", RegexOptions.IgnoreCase);

            // Match file name with regex
            Match match = regex.Match(file_name);

            if (match.Success) {
                string nameMovie = match.Groups[2].Value;
            }

            // store match in attribut
            this.file_info = match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetMovieName() {
            // return movie name without "." between words
            return this.file_info.Groups[2].ToString().Replace(".", " ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetMovieDate() {

            // return movie date
            return this.file_info.Groups[3].ToString();
        }
    }
}
