# Library Management System

## README — How to Install and Run

### 1. About the Project

Library Management System is a C# project that helps a librarian manage books, categories, members, borrowing, returns, and borrowing history.

After logging in, the librarian can choose a function from the Main Menu. When finished, the system returns to the Main Menu.

### 2. Main Features

* **Login:** Log in to the system. If the login details are wrong, an error appears and the user can try again.
* **Books:** Add, edit, delete, and search for books.
* **Categories:** Add, edit, delete, and search for categories.
* **Members:** Register, edit, deactivate, and search for members.
* **Borrow:** Choose a member and available books, set the borrow and due dates, and save the borrowing record. The available book quantity is reduced.
* **Return:** Find a borrowing record, choose a book to return, and enter the return date. If it is late, the system calculates a fine. The book quantity is increased.
* **History:** View all borrowing records or search by member, book, date, or status. This page does not change borrowing records.

### 3. What You Need

Install the following tools:

* Windows
* Visual Studio
* .NET or .NET Framework: **[Enter the version used]**
* PostgreSQL
* pgAdmin 4 (recommended for managing the database)

### 4. How to Install

#### Step 1: Open the Project

1. Extract the project ZIP file.
2. Find the `.sln` or `.csproj` file.
3. Open it in Visual Studio.

#### Step 2: Set Up the Database

1. Open **pgAdmin 4**.
2. Connect to your PostgreSQL server.
3. Create a database named:

```text
library_management
```

4. Open the provided `.sql` file.
5. Run the SQL script on the `library_management` database.
6. Check that all required tables have been created.

#### Step 3: Set the Database Connection

1. Find the database connection settings in the project.
2. Update the PostgreSQL connection string with your PostgreSQL details.
3. Make sure the following information is correct:

```text
Host=localhost;
Port=5432;
Database=library_management;
Username=postgres;
Password=your_password;
```

#### Step 4: Restore Packages

1. Open the project in Visual Studio.
2. Restore NuGet packages if the project uses them.
3. Wait until the package restore is complete.

#### Step 5: Run the Project

1. Set the correct project as the startup project.
2. Build the project.
3. If there are no errors, run the project.
4. Log in to start using the system.

**Login Information**

```text
Username: admin
Password: 1234
```

### 5. How to Use

1. Log in.
2. Choose a function from the Main Menu.
3. Complete the task.
4. Return to the Main Menu to choose another function.

### 6. Common Problems

* **Cannot connect to PostgreSQL:** Check that PostgreSQL is running and the connection details are correct.
* **Database not found:** Make sure the `library_management` database has been created.
* **Tables are missing:** Run the provided `.sql` file on the correct database.
* **Login does not work:** Check the login details and make sure the admin account is set up.
* **Packages are missing:** Restore NuGet packages and build the project again.
* **Project will not run:** Check that the required .NET version is installed and the correct startup project is selected.

### 7. Project Details

* **Project name:** Library Management System
* **Language:** C#
* **Database:** PostgreSQL
* **Version:** 1.00
* **Developers:** Tith Cholna, Soeng Yanut, Chhay Tha, Khon Somphors, Ben Thona
