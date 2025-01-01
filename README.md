# Movie Management System

A comprehensive Movie Management System that organizes and manages movie-related data, including genres, production companies, directors, producers, actors, and more. The system also features a graphical user interface (GUI) to simplify interaction with the database.


## 📚 Project Overview

This project implements a robust database for managing movies and related information such as genres, production companies, directors, producers, actors, and users. The database schema ensures data integrity and provides features like triggers to enforce constraints and cascade rules.

The database is accompanied by a GUI for user-friendly interaction, making it easier to manage data and perform CRUD (Create, Read, Update, Delete) operations.


## 📂 Features

1. **Database Tables:**
   - `Genres`: Manage movie genres.
   - `ProductionCompanies`: Store production company details.
   - `Producers`: Manage producer information.
   - `Directors`: Store director details.
   - `Movies`: Store and manage movies with metadata such as rating, runtime, and plot.
   - `Actors`: Manage actor profiles.
   - `MovieCast`: Track actor roles in movies.
   - `Users`: Manage user accounts.
   - `Admins`: Handle admin accounts.

2. **Triggers:**
   - Ensure valid `DateOfBirth` for actors.
   - Handle dependent records when deleting a movie.

3. **Data Integrity:**
   - Validation using `CHECK` constraints.
   - Relationships using `FOREIGN KEY` constraints.

4. **GUI:**
   - User-friendly interface for managing database entries.
   - Simplifies CRUD operations.
   - Real-time interaction with the database.

## 🛠️ Technologies Used

- **Database**: MySQL
- **GUI Framework**: (Specify the framework used, e.g., Java Swing, PyQt, Tkinter, etc.)
- **Programming Language**: SQL, (Specify the language used for GUI, e.g., Python, Java, etc.)
- **Triggers and Constraints**: Enforce data integrity.


## 🚀 Getting Started

### Prerequisites

- MySQL Server (version 8.0 or later)
- GUI Framework dependencies (install relevant dependencies based on the framework used for the GUI)

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
