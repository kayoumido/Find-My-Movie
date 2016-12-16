using System.Text.RegularExpressions;

namespace Find_My_Movie {
    class extractfileinfo {

        private Match file_info;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="file_name">name of the file to get info</param>
        public extractfileinfo(string file_name) {
            this.GetData(file_name);
        }

        /// <summary>
        /// Extract information from file name.
        /// e.g. 
        /// After.Earth.2013.TRUEFRENCH.DVDRiP.XViD-AViTECH --> Name : After Earth
        /// </summary>
        /// <param name="file_name">Name of the file to extract information from</param>
        private void GetData(string file_name) {
            
            Regex regex = new Regex(@"(\[.+\]|)(.*?)(dvdrip|byPhilou|TRUEFRENCH|READNFO|avi|\.avi|EDITION|FRENCH|xvid| cd[0-9]|dvdscr|brrip|divx|[\{\(\[]?[0-9]{4}).*", RegexOptions.IgnoreCase);

            // Match file name with regex
            Match match = regex.Match(file_name);

            if (match.Success) {
                string nameMovie = match.Groups[2].Value;
            }

            this.file_info = match;
        }

        /// <summary>
        /// Get movie name
        /// </summary>
        /// <returns>Movie name</returns>
        public string GetMovieName() {
            // return movie name without "." between words
            return this.file_info.Groups[2].ToString().Replace(".", " ");
        }
    }
}
