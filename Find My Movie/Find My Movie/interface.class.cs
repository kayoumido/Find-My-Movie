using Find_My_Movie.model.repository;
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
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = appdataPath + "/" + MainWindow.FOLDER_NAME;
            string filePath = folderPath + "/" + MainWindow.CONFIG_FILE_NAME;
            string moviePath = directoryClass.GetPathConfig(filePath, "/config/path_movies");
            string ogFileNamePath = folderPath + "/originalFileName.txt";

            if (!File.Exists(ogFileNamePath)) {
                File.Create(ogFileNamePath);
            }

            //get movie in directory and child directory
            List<string> filePaths = DirectorySearch(moviePath);

            DeleteMovies(filePaths, ogFileNamePath);

            return filePaths.ToArray();

        }//GetAllFilename

        /// <summary>
        /// Delete movies from the database if they are no longer in the folder
        /// </summary>
        /// <param name="filePaths">Name of the file that have just been scanned</param>
        /// <param name="ogFileNamePath">Path to the file with that containes the file names of the previous scan</param>
        private void DeleteMovies(List<string> filePaths, string ogFileNamePath) {

            List<string> originalFileNames = new List<string>(File.ReadAllLines(ogFileNamePath));
            List<string> fileNames = new List<string>();

            foreach (string filePath in filePaths) {
                fileNames.Add(Path.GetFileName(filePath));
            }

            foreach (string originalFileName in originalFileNames) {
                bool inFile = false;
                foreach (string fileName in fileNames) {
                    if (originalFileName == fileName) {
                        inFile = true;
                    }
                }
                if (!inFile) {
                    MovieRepository movieRepo = new MovieRepository();
                    int idMovie = movieRepo.MovieExists("filename", originalFileName);
                    movieRepo.DeleteMovie(idMovie);
                }

            }

            File.WriteAllLines(ogFileNamePath, fileNames);

        }


        /// <summary>
        /// Get all files name from a path 
        /// </summary>
        /// <param name="directoryPath">path to the folder</param>
        /// <returns> All file names </returns>
        private List<string> DirectorySearch(string directoryPath)
        {

            List<string> fileNames = new List<string>();

            // Loop in all the sub-directories
            foreach (string directory in Directory.GetDirectories(directoryPath))
            {

                //try to avoird no read in folder/file
                try {

                    // Loop on all the files
                    foreach (string file in Directory.GetFiles(directory)) {

                        // Add the file name to the list
                        fileNames.Add(file);

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

                fileNames.Add(file);

            }// foreach (string file in Directory.GetFiles(directoryPath))

            return fileNames;

        }// DirectorySearch

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
