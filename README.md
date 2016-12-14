# Find My Movie - C# #
_Décembre 2016_  
_Technicien ES -  CPNV_  
_Struan Forsyth - Antoine Dessauges - Doran Kayoumi_  
## TABLE OF CONTENS
1. Conventions
2. Database

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

#### 1.5.2
Initialy we set that model names and file names were going to be exactly the same as the table it represents, but we had some issues with it. A library we used had exactly the same names as our models, so we decided to change them.
Now our model names and file names start with the initals of the application "fmm" the the name of the table.  
e.g. :

    *Before*
    User { ...
    User.cs

    *Now*
    fmmUser { ...
    fmmUser.cs
