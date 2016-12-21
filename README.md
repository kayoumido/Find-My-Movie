# Find My Movie - C# #
_Décembre 2016_  
_Technicien ES -  CPNV_  
_Struan Forsyth - Antoine Dessauges - Doran Kayoumi_  
## TABLE OF CONTENTS
1. Conventions
2. Work environment
3. Configuration
4. Dependencies
5. Database
6. API
7. Interface
8. Search
9. Filter
10. Annexe

## 1. Convention

To keep everything as consistent as possible, we took time to set conventions for variable, functions, etc...

### 1.1 Variables
Variable names are written in lowercase. If it's composed of multiple words, each word, expected the first one, will start with a capital letter.  
e.g. :

    string title = "Find My Movie - C#";
    string authorName = "Doran Kayoumi";

### 1.2 Constants
Constants are declared in uppercase. If the name is composed of multiple words, each word is seperated by an underscore.  
e.g. :  

    const string TITLE = "FindMyMovie";
    const string AUTHOR_NAME = "Doran Kayoumi"

### 1.3 Opening brackets
Opening brackets are blaced on the same line as the declaration (class, function, loop, condition, etc).  
e.g. :  

    Function() { ...
    Class() { ...

### 1.4 Functions
Function names start with a capital letter and the rest is written in lowercase. If it's composed of multiple word, each word is separated by a capital letter.  
e.g. :  

    Title ()
    GetAuthor()

### 1.5 Classes
#### 1.5.1 General
In most cases classes will follow these conventions :
Class names are written in lowercase (even if it's composed with multiple words).  
e.g. :  

    dbhandler { ...
    extractfileinfo { ...

 The class file names contain the name of the class (kind of necessary to know th nature of the file :D), then it will be followed by a *.class* and finished by the file extension.  
e.g. :  

    dbhandler.class.cs
    extractfileinfo.class.cs

Attributes and methods follow the same conventions as for variables and functions.

#### 1.5.2 Models
Initialy we set that model names and file names were going to be exactly the same as the table it represents, but we had some issues with it. A library we used had exactly the same names as our models, so we decided to change them.
Now our model names and file names start with the initals of the application "fmm" the the name of the table.  
e.g. :

    *Before*
    User { ...
    User.cs

    *Now*
    fmmUser { ...
    fmmUser.cs


## 2. Work environment
This project was realised on computer, provided by the CPNV, with the following characteristics :
    * OS : Windows 7 Entreprise - Service pack 1
    * CPU : Intel Core i7-6700 @ 3.40GHZ
    * RAM : 16Gb
    * System type : 64bits

![alt text][System configuration]

### 2.1 Tools
For the development of the project, we used [Microsoft Visual Studio Entreprise 2015]. And for the versioning of the project, git was used with [GitKraken] and the code was hosted on [Bitbucket].


## 3. Configuration


## 4. Database
Initialy the project was using JSON files and not a database, but after a few changes, we had the change and implement a database.

Before implementing a database, we had to choose a database managment system (DBSM). There's a wide array of DBMS to choose from (MySQL, SQL, NoSQL, SQLite, etc..).
To avoid using an external server to host the database, we decided to use a local database, since Struan and Antoine had some experience with SQLite, we went with it.

### 4.1 Creating the database
To keep it as simple as possible, we created tables for the elements that were returned by the API.

    movie
    cast
    crew
    collection
    company
    country
    genre
    language


For the sake of simplicity, some of the information stored aren't used. Even though they aren't used, we found they were still usefull to have and that they could easily be added afterwards in later versions of the appication.

The SQL script will be annexed.

## 7. Interface
The inital project was create in windows form but to simplify the creation of the design we choose to change to a WPF project.

The interface has two window :
* __choosedirectory.xaml__
* __MainWindow.xaml__

Each file contains XAML code to create the design. It's basicaly like HTML, every tag may has attributs and you can interact with them in C#.

Each window has a .cs file that contains the code/functions that will be used by the window (e.g. : MainWindow.xaml.cs).

### 7.1 choosedirectory.xaml
This window is use to selecting the folder which contains the movies. It is opened the first time the user launches the application and when they click on the change folder icon.

### 7.2 MainWindow.xaml
This is the main window of the application. It contains the XAML that displayes the grid of movies and the details of a movie.

The orange bar is an external ressources/library known as [Mahapps].

The filter and search functions are also contained in this file.

To overwrite the default style of the elements, we use `<style>` tag in the top of the file and link it to the wanted elements.

### 7.3 choosedirectory.xaml.cs
This file manages all the events comming from the window "choosedirectory.xaml", its main purpose is to store the movie folder path in a configuration file.

### 7.4 MainWindow.xaml.cs
This file manages the display of the movies and the display of the details of a movie. It launches the API that collects the movie data and addes the movie to the database. It's also the conenxion point between all the classes and the functionalities of the application.

### 7.5 Internet
The application should work without internet. A pop-up will informe the user if they are not connected, the pictures won't be displayed and the new movies won't have their data collected from the API.
A "picture not found" image will be display if there is no connexion. This is because the movie covers aren't downloaded and saved on the users machine but are loaded from a url each time.

### 7.6 Regex
The regex is located in the "extractfileinfo.class.cs" file. It extracts the suposed movie title from the file name.

Full regex : `(\[.+\]|)(.*?)(dvdrip|byPhilou|TRUEFRENCH|READNFO|avi|\.avi|EDITION|FRENCH|xvid| cd[0-9]|dvdscr|brrip|divx|[\{\(\[]?[0-9]{4}).*`

The regex splits the file name into different groups :
* First group `(\[.+\]|)`:  
If the name containt a [...] with a web site (e.g. : [www.Cestpasbien.fr])
* Second group `(.*?)` :  
 The movie title
* Third group `(dvdrip|byPhilou|TRUEFRENCH|READNFO|avi|\.avi|EDITION|FRENCH|xvid| cd[0-9]|dvdscr|brrip|divx|[\{\(\[]?[0-9]{4})` :  
 The year the movie was released or another special string like "DVDRIP" OR "FRENCH"

### 7.7 How movies are display
At first we get the file names from the folder. After the file names are put through the regex to get the movie title which is used to test if it's in the database.

* If the movie is found in the database, we get the data and display it.
* If the movie is not found in the database, we use the API to get the data, add it to the display and add it to the database (only if the machine is connected to the internet, the API doesn't work without it).

All this code is executed in a thread, doing this allows the user to use the application while movies are being loaded.

### 7.8 Responsive
The application is responsive, it calculates the size of the elements depending on the window size.
For the display of the details of a movie it is defined in the XAML file. The elements are set with a width of `0.8*`.
For the movie grid a function was create in "Interface.class.cs" (getWidthMovie) which returns the width of a movie cover.

## 8. Search
The search function allows the user to search for a title, actor or directory of a movie, as well as search all three at the same time. The searched movies are displayed like normal so the usual actions work can be used.
When the user wants to remove the search they can click the back arrow.

The search function is located in the main file (MainWindow.xaml.cs). This function gets the data from the form, does a check on the data, sends the data to a repository (MovieRepository.cs) which will query the database for the movies. The last thing it does is send the list of movie object to a function that displayes the movies (addMovieGrid).

## 9. Filter
The filter function allows the user to filter on a year, by inputing two identical year in the input, or on a rang of years. There is also the possibility to filter on the genre. The user can select one genre to all of them. The filter on the genres is cumulative, so if the user selects "Action" and "Fantasy" the movies returned will have both those genres.
When the user wants to remove the filter they can click the back arrow.

The filter function is located in the main file (MainWindow.xaml.cs). This function gets the data from the form, does a check on the data, sends the data to a repository (MovieRepository.cs) which will query the database for the movies. The last thing it does is send the list of movie object to a function that displayes the movies (addMovieGrid).

## 10. Functionality
The application contains the following features :
* Possibility to select a folder that contains the movies
* Extract the movie title from the original file name
* Possibility to change the folder where the movie are located
* Get the movie data (e.g. : Title, Description, Actors, etc...) from FMMDB
* Save the movie data in a local database
* Display all the movies
* Display a detailed view of a movie
* Play the movie
* Search a movie per title, actor or director
* Filter the movie per year or genre

## 11. State of the project
There are some features which need to be improved and some rare bugs that need fixed.
But we are fairly please with what we have acomplished considering we couoldn't remember much of C#

### 11.1 Bugs
Here is a lit of bugs which need to be fixed

* If the folder selected is located on a network/external drive (e.g. : K://COMMUN) and the network/external drive is disconnected the application will crash.
* If the internet connexion is lost after the start of the application, the movies won't be display or the application will crash (depending of the moment internet is lost).

### 11.2 Improvements
* Create a thread for the statuts of the internet connexion  (this will fix the bug with the internet).
* Download the movie's cover to the users machine to allow the application to work without internet after the first launch.
* Improve the way the details of a movie are displayed.
* Improve the search and filter functions to that they work together (e.g. : search for a movie containing "war" in the title and then wants to filter for only those that came out in 2016).
* There are certain bits of code that are repeted and should be moved into functions.

[11. Reference]:

[System configuration]:                    img/system-configuration.png     "System configuration"  
[Bitbucket]:                               https://bitbucket.org/           "Bitbucket"
[Mahapps]:                                 http://mahapps.com/           "Mahapps"  
[GitKraken]:                               https://www.gitkraken.com/       "GitKraken"
[Microsoft Visual Studio Entreprise 2015]: https://www.visualstudio.com/vs/ "Microsoft Visual Studio Entreprise 2015"  
