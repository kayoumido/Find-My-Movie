using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Find_My_Movie{

    class @interface {

        /// <summary>
        /// Get path folder in config file and lauch directorySearch
        /// </summary>
        /// <returns>Files name</returns>
        public string[] GetAllFilename()
        {

            //get the choose directory form
            choosedirectory directoryClass = new choosedirectory();

            //get path movie in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = directoryClass.GetPathConfig(file_path, "/config/path_movies");

            //get movie in directory and child directory
            List<string> file_names = DirectorySearch(movie_path);

            return file_names.ToArray();

        }//GetAllFilename


        /// <summary>
        /// Get all files name from a path 
        /// </summary>
        /// <param name="directoryPath">path to the folder</param>
        /// <returns> All file names </returns>
        private List<string> DirectorySearch(string directoryPath)
        {

            List<string> file_names = new List<string>();

            // Loop in all the sub-directories
            foreach (string directory in Directory.GetDirectories(directoryPath))
            {

                //try to avoird no read in folder/file
                try {

                    // Loop on all the files
                    foreach (string file in Directory.GetFiles(directory)) {

                        // Add the file name to the list
                        file_names.Add(Path.GetFileName(file));

                    }// foreach (string file in Directory.GetFiles(directory))

                    // If there are any more directories in the directory
                    DirectorySearch(directory);

                }
                catch (Exception ) {

                }


            }// foreach (string directory in Directory.GetDirectories(directoryPath))

            // Loop in the main directory that was selected
            foreach (string file in Directory.GetFiles(directoryPath))
            {

                file_names.Add(Path.GetFileName(file));

            }// foreach (string file in Directory.GetFiles(directoryPath))

            return file_names;

        }// directorySearch

        /// <summary>
        /// Get thw with of movie for the display in grid (depending windows width)
        /// </summary>
        /// <param name="widthForm">Windows width</param>
        /// <returns> Width of one movie</returns>
        public double GetWidthMovie(double widthForm) {
            
            double moviePerLine = 4;

            //number of movie per line for the max width of one movie is 300 px
            while (widthForm / moviePerLine > 300) {
                moviePerLine++;
            }//while

            // width movie without the scrollbar width
            return widthForm / moviePerLine - SystemInformation.VerticalScrollBarWidth / moviePerLine;

        }//getWidthMovie

    }//@interface
}
