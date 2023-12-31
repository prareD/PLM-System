# PLM_System (Parking Lot Management System)

PLM_System is a Parking Lot Management System that simplifies the management of a parking lot. This browser application allows you to check in and checkout vehicles, keeping track of their entry and exit times.

## Installation

Before running the application, make sure you have the following prerequisites installed:

- .NET 7
-  Microsoft SQL Server 16.0.10

## Built With

- .NET 7
- Microsoft SQL Server 2022 (RTM) - 16.0.1000.6 (X64)
- CSS3
- HTML5
- JavaScript
- jQuery
- Visual Studio 2022

## NuGet Packages
- NUnit 3.13.3
- Moq 4.18.4

## Setting Up the Database

1. Post the installation of .Net 7, sql server create a new database in your MySQL server.
2. Open the `DataBase/PLM_SystemDB.sql` file located in the resources folder of the code base.
3. Run the SQL commands present in the `DataBase/PLM_SystemDB.sql` file to set up the necessary tables and data in the database.

## Conditions

1.vehicle number can be max of 8 digits and make sure to change connection string path in appsettings.json as per requirment.

## Running the Application

1. Clone the code repository to your local machine.
2. Import the code into Visual Studio IDE.
3. Build and run the project to launch the application.

Please note that you might need to configure the database connection string in the application settings to ensure it connects to your MySQL database.

Feel free to explore the application and manage your parking lot efficiently with PLM_System.

For any issues or questions, please refer to the documentation or reach out to the project maintainers.
