# BANKING-AND-BUDGETING-APPLICATION

A mobile banking and budgeting application built using C#, .NET MAUI and XAML.

The application allows users to manage their balance, record deposits and withdrawals, create savings goals, check affordability, and locate nearby ATMs through an interactive map.

## ✨ Features
User registration and login with Firebase Authentication
Deposit and withdraw money
Automatic balance recalculation
View and manage transaction activity
Create and track multiple savings goals
Display savings progress using completion percentages and progress bars
Affordability checking
Locate nearby ATMs using the user's location
Retrieve ATM data using OpenStreetMap
Multi-page navigation using .NET MAUI Shell
Settings functionality
## ⚙️ Technologies
* C#
* .NET MAUI
* XAML
* Firebase Authentication
* OpenStreetMap
* Overpass API
* REST API integration
* JSON
* Git / GitHub

## ⚒️ How It Works

The application is built using .NET MAUI with XAML used to create the user interface and Shell navigation used to move between different areas of the application.

Users can register and log in using Firebase email and password authentication. Authentication requests are handled asynchronously, with validation and user-friendly error handling included.

Banking

Users can record deposits and withdrawals, with the application automatically recalculating their current balance based on their transactions.

Savings Goals

Savings goals are represented using C# objects and collections.

Users can create and monitor individual goals, with completion percentages and progress bars showing how close they are to reaching each target.

ATM Finder

The application can access the user's location and retrieve nearby ATM information using the OpenStreetMap Overpass API.

ATM data is requested asynchronously and processed from JSON before being displayed to the user. The implementation also includes retry handling for API requests.

Affordability

The application includes an affordability feature designed to help users determine whether a purchase fits within their available balance and budget.

## Project Motivation

This project was developed as a university project to explore mobile application development using C# and .NET MAUI.

It gave me practical experience working with application logic, authentication, APIs, asynchronous programming, JSON data, geolocation, object-oriented programming and multi-page mobile application development.

## Running the Project

Clone the repository:

git clone <repository-url>

Navigate into the project directory:

cd BANKING-AND-BUDGETING-APPLICATION

Restore the project dependencies:

dotnet restore

Build the project:

dotnet build

The application can then be run using a supported .NET MAUI Android development environment.

Configuration

The application uses external services including Firebase and the OpenStreetMap Overpass API.

Any API configuration or credentials required by the application should be stored securely and should not be committed to the repository.

## Project Type

🎓 **University Project**

The project was created to demonstrate mobile application development, banking and budgeting functionality, API integration and authentication using .NET MAUI.

Author

Karim Elmouslemany

Computer Science Graduate
[LinkedIn](https://www.linkedin.com/in/karimelmouslemany/)
