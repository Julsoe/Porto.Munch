# Porto.U IN MNCH

## What is U in MNCH?

This project is made by eight students at Høyskolen Kristiania spring '23. The following people has participated in this project:

- Tonje Karin Nordquist
- Christopher Glommen Andresen
- Thea Brenna
- Maiken Jørgensen Edvardsen
- Anikken Thøring Sivertsen
- Julie Søbo
- Siri Dale
- Ida Urszula Krech

### Description

The solution we have created is based on one of the cases given by MUNCH. We were presented with two different cases from product owner, and we chose the following case: How can we make young adults (18-25) more engaged in Munch, through innovative use of new technologies, forms of experience and interaction that argue for the physical art experience + How can we measure and visualize the effect of the engagement we create through innovative use of technology?

### Why?

U in MNCH is the solution we made, and is an application built as a game, to inspire and engage young adults to become interested in art. The game consists of clues and tasks to solve a mystery, which is based on a true story about some of Munch's paintings during WW2. The goal of the game is to find out what happened to the missing painting, and the true story will be revealed when the last clue is solved. The user will get small pieces of information about the story along the way from Munchgyver. He is the detective who has been working on the case and needs help from the user to solve the tasks and connect the pieces of the clues. By looking at the progress of the user, and how many of the users that complete the mystery, we can track the effect of the engagement we are creating.

## Get started

# Installation list

You need to install a couple of things to get the app up and running:

- [Azure Data Studio & MS SQL Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16)
- [Visual Studio Code](https://dotnet.microsoft.com/en-us/download)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Android Studio](https://developer.android.com/studio)

**The prject is coded to work on MacOS and/or Linux. If you're using Windows you can skip the following installation(s):**

- [Docker for MacOS](https://docs.docker.com/desktop/install/mac-install/)
- [Docker for Linux](https://docs.docker.com/desktop/install/linux-install/)

**If the environment is to be configured on Mac or Linux**, you would have to install Docker. In Docker we create a container for us to be able to login with SQL credentials. The credentials will be set when creating and running the container for the chosen Image.

**If the environment is to be configured on Windows**, you can use your Microsoft credentials to login to Azure Data Studio/MS SQL Server. That means that there is no need to install Docker to get the environment up and running correctly.

# Setup dev environment

- If on MacOS or Linux: Install and open Docker Desktop.
  - Choose to install docker with the regular setting and skip the roles and uses.
  - Open Docker and increase RAM allocation for docker. We’ve used 4GB which is enough for our project.
  - Open a terminal and insert the following command to create and run a container for the Image:

```
docker run -e "ACCEPT_EULA=1" -e "MSSQL_USER=SA" -e "MSSQL_SA_PASSWORD=Porto_Munch9" -e "MSSQL_PID=Developer" -p 1433:1433 -d --name=sql_connect mcr.microsoft.com/azure-sql-edge
```

- Check if the container is running:

```
docker container ls -a
```

**If the status of the container is “Exited” the container is not running and needs to be started to connect to Azure Data Studio/MS SQL Server. See the command below to start the container.**

- Start the container called sql_connect:

```
docker start sql_connect
```

- Stop the container called sql_connect – **Please note that the container will run even if you close the terminal/desktop application:**

```
docker stop sql_connect
```

- Install and open Azure Data Studio & MS SQL Server.

Connect to localhost where you change the parameters in red squares below. Remember that you’d need to have the Docker container running for this to work. The username is “SA” and the password is “Porto_Munch9” unless this was changed in the steps above.

**If on MacOS or Linux:**

- Server = localhost
- Authentication type = SQL Login
- Username = SA
- Password = Porto_Munch9
- Trust server certificate = True

**If on Windows:**

- Server = localhost
- Authentication type = Windows Authentication
- Trust server certificate = True

When connected you should run the SQL seed by copying the following text and insert in Azure Data Studio after connection is established:

```
-- Create the MUNCH database
CREATE DATABASE MUNCH;
GO

-- Use the MUNCH database
USE MUNCH;
GO

-- Create the MUNCH schema
CREATE SCHEMA MUNCH;
GO

-- Create the Users table in the MUNCH schema
CREATE TABLE MUNCH.Users (
	UserId INT IDENTITY(1,1) PRIMARY KEY,
	Email VARCHAR(50),
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	Password VARCHAR(50),
	FullName AS (FirstName + ' ' + LastName),
	DateOfBirth DATE,
	CreatedAt DATETIME
);
GO

-- Create the Infinite_v1 table in the MUNCH schema
CREATE TABLE MUNCH.Infinite_v1 (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	userID INT,
	task1 BIT,
	task2 BIT,
	task3 BIT,
	task4 BIT,
	task5 BIT,
	task6 BIT,
	FOREIGN KEY (userID) REFERENCES MUNCH.Users(UserId)
);
GO

-- Insert data into the Users table
INSERT INTO MUNCH.Users (Email, FirstName, LastName, Password, DateOfBirth, CreatedAt)
VALUES
	('test@gmail.com', 'Test', 'Testsson', 'ProgrammingIsC00L', '1995-06-09', GETDATE());
GO

-- Insert data into the Infinite_v1 table
INSERT INTO MUNCH.Infinite_v1 (userID, task1, task2, task3, task4, task5, task6)
VALUES (1, 0, 0, 0, 0, 0, 0);
GO
```

- Install both Visual Studio Code and the .NET SDK. Make sure to include the “.NET desktop development” and “ASP.NET and web development”. This will ensure that you have all the necessary tools and templates to create web APIs with C#.

As packages aren’t automatically installed on other systems, you’d have to add the following packages in the WebAPI directory through the terminal:

```
Dotnet add package dapper
Dotnet add package Microsoft.Data.SqlClient
```

**If you’re running on a Windows**, you’d need to change the ConnectionStrings inside appsettings.json in the WebAPI to the following string:

```
"ConnectionStrings": {
	"DefaultConnection" : "Server=localhost; Database=MUNCH; Trusted_Connection=true;TrustServerCertificate=True;
  },
```

The only thing missing now is downloading Android Studio and running the app through Android Virtual Device (AVD) to test out our project!
