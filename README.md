# Authentication and User Management System

## Currently, the following features have been implemented
- User Registration 
- Login using JWT

## Features to be added

- User Management
- Complete Security Policies
- Full Implementation of Refresh Token
- Final Api Documentaion

## Features

- Register:Users can register in the system
- Login: Users can log in using their username or email and receive a JWT.
- JWT Tokens: The system uses JWT for authentication.


## Technologies

- ASP.NET Core 8.0
- Entity Framework Core
- JWT Authentication
- Microsoft Identity
- SQL Server for data storage
- Swagger for API documentation

## Prerequisites

Before running the project, ensure the following are installed:
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

## Installation and Running

Follow theses steps to run the project:
1. Clone the repository
    git clone https://github.com/MGonjishke/Auth.Api.git
    cd Auth.Api

2. Create a file called **appsettings.json** in Auth.Api and enter and configure the following code:
           
           {
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          },
          "ConnectionStrings": {
            "DefaultConnection": "Server=YOUR_SERVER;Database=AuthDb;Trusted_Connection=True;"
          },
          "JWT": {
            "Audience": "Audience Url",
            "Issuer": "Issuer Url",
            "SignInKey": "Secret Key"
          }
        } 
   


4. Apply Migrations: Run the migration to set up the database schema.

5. Run the project


