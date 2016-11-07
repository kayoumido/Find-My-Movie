using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Find_My_Movie{

    class @interface {

        public string[] GetAllFilename()
        {


            //get the choose directory form
            choose_directory directoryClass = new choose_directory();

            //get path movie in config file
            string app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder_path = app_data_path + "/" + MainWindow.FOLDER_NAME;
            string file_path = folder_path + "/" + MainWindow.CONFIG_FILE_NAME;
            string movie_path = directoryClass.GetPathConfig(file_path, "/config/path_movies");

            //get movie in directory and child directory

            List<string> file_names = directorySearch(movie_path);

            File.WriteAllLines(folder_path + "/filenamesOriginal.txt", file_names.ToArray());

            return file_names.ToArray();

        }//GetAllFilename



        private List<string> directorySearch(string directoryPath)
        {

            List<string> file_names = new List<string>();

            // Loop in all the sub-directories
            foreach (string directory in Directory.GetDirectories(directoryPath))
            {

                // Loop on all the files
                foreach (string file in Directory.GetFiles(directory))
                {

                    // Add the file name to the list
                    file_names.Add(Path.GetFileName(file));

                }// foreach (string file in Directory.GetFiles(directory))

                // If there are any more directories in the directory
                directorySearch(directory);

            }// foreach (string directory in Directory.GetDirectories(directoryPath))

            // Loop in the main directory that was selected
            foreach (string file in Directory.GetFiles(directoryPath))
            {

                file_names.Add(Path.GetFileName(file));

            }// foreach (string file in Directory.GetFiles(directoryPath))

            return file_names;

        }// directorySearch


        public double getWidthMovie(double widthForm) {
            

            int moviePerLine = 4;
            while (widthForm / moviePerLine > 300) {
                moviePerLine++;
            }//while

            return widthForm / moviePerLine;

        }//getWidthMovie

    }//@interface
}
