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
8. Filters
9. Annexe

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
The interface part consist to manage the style of the application. Two windows were created :
* choosedirectory.xaml
* MainWindow.xaml

Each .xaml file contains XAML code to create the design. It's basicaly like HTML, every tag may have some attribut and you can interact with him in C#.

Each windows has a .cs file to contain C# functions concerning the windows ( ex: MainWindow.xaml.cs).

### 7.1 choosedirectory.xaml
This windows are use to selecting the folder who contains the movies. She is open the first time you launch the application and when the user click on the change folder icon.

### 7.2 MainWindow.xaml
This is the principal windows of the application. She contains the XAML to display the grid of movies and the single of a movie.

The orange bar are an external ressources comming from [Mahapps].

The filter and the search are also contains in this file.

To overwrite the default style of form elements we use `<style>` tag in the top of the file and linked this stlye to the correct elements.

### 7.3 choosedirectory.xaml.cs
This file manage events with the windows "choosedirectory.xaml", basically she just manage the save of the folder in the config file and the form.

### 7.4 MainWindow.xaml.cs
This file manage the display of the movies in the grid or the single view of a movie, the launch of the api for collecting movie's data, add movie in the DB. She is the conenxion between all the class and functionality of the application.

### 7.5 Internet
The application should work without internet. A pop-up informe the user if internet is not connected. However the picture won't be display and the new movie can't be get from the api. A "picture not found" image will be display. This append beacause the picture are not save in folder and are from a url.

### 7.6 Regex
The regex is in the "extractfileinfo.class.cs" file. She extract the file name from the original file name. She detect different group :
* First group, if the name containt a [] with the site web (ex: [www.Cestpasbien.fr])
* Secong group, the year of the movie or another special string like "DVDRIP" OR "FRENCH"
* Third group, the movie name
* Fouth group, the extension, year or special string

### 7.7 How the movies are display
In first time we get all filename from the folder choose by the user. After use the regex to get the correct name of the movie we try to find it in the DB.

* If the movie is found in the DB, we get the data and display it in the movie grid.
* If the movie is not found in the DB, we use the api to get all datas, display it in the movie grid and add it in the DB (only if internet is active, api doesn't work without).

All this code is executed in a thread to allow user to use the application while it run.

### 7.8 Responsive
The application is responsive, she calcul the size of the elements depending of the windows size. For the single is make in the XAML with the width="0.8*" of the elements.
For the movie grid a function was create in "Interface.class.cs" (getWidthMovie) who return the width of a movie.

A rajouter :
Descriptions des fonctionnalités claires
Etat des lieux final et projection sur le futur

[11. Reference]:

[System configuration]:                    img/system-configuration.png     "System configuration"  
[Bitbucket]:                               https://bitbucket.org/           "Bitbucket"
[Mahapps]:                                 http://mahapps.com/           "Mahapps"  
[GitKraken]:                               https://www.gitkraken.com/       "GitKraken"
[Microsoft Visual Studio Entreprise 2015]: https://www.visualstudio.com/vs/ "Microsoft Visual Studio Entreprise 2015"  
