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
9. Filters
10. Annexe
11. Reference

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

<!--
## 11. Reference
-->
[System configuration]: img/system-configuration.png "System configuration"
