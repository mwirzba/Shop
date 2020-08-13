# SHOP
Web API created in ASP.NET Core 3.0

This project has been created to learn ASP.NET Core API.
While creating this project I learned best coding practices and many useful libraries.

### GENERIC REPOSITORY PATTERN
In this project I used generic repository pattern.
All calls to database using *Entity Framework* has been placed in repositories.
* Thanks to this pattern code in controllers is more readable and I do not have to repeat same code.
* Thanks to repositories interfaces it is also easier to write unit tests.

### DOMAIN TRANSFER OBJECT (DTO)
To transfer data beetween API and Client I use DTO
 * It allows to seperate models used in database from client 
 * It also allows to flatten complicated objects

### AUTOMAPPER
* Using automapper is the best way to make good use of DTO pattern and make code shorter and less complicated.


### DATA VALIDATION USING FLUENT VALIDATION LIBRARY
 * This library allows to create readable validation rules

### AUTHORIZATION USING JWT TOKENS

### UNIT TESTS 
 * Good unit tests should be short,easy to read and should check one thing
 * I wrote unit tests for each controller using *NUnit*  framework and *Moq* Framework
 * I used *Fluent Assertions* Framework: 
     > “With Fluent Assertions, the assertions look beautiful, natural and, most importantly, extremely readable”
 * Additionally I decided to implement builder pattern to create even more readable unit tests 

### SWAGGER 
* Good tool during Web API creation
* Great documentation for client




