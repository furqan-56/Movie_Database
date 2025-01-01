# Movie Management System

## Overview
The **Movie Management System** is a database-driven system designed to manage and organize data related to movies, their production details, genres, ratings, reviews, and user information. This system is primarily aimed at stakeholders such as production companies, directors, and users who want to interact with movie data. The system provides functionalities for storing information about movies, categorizing them by genre, tracking ratings and reviews, and managing user-related data.

## Features
- **Movie Information**: Store and retrieve movie details such as name, release year, plot, industry, and length.
- **Categorization**: Movies can be categorized by genre and mood type.
- **Production Details**: Track directors, producers, and production companies involved in a movie.
- **Ratings & Reviews**: Users can rate movies and leave reviews.
- **Multi-language Support**: Movies can be associated with different languages.
- **Cast Information**: Track the lead actors in movies.

## Database Design

### Entity-Relationship Diagram (ERD)
The ERD represents the database structure with its entities, attributes, and relationships. Key entities include **Movie**, **Director**, **Producer**, **Genre**, **Rating**, **User**, **Language**, **MoodType**, **ProductionCompany**, and **MovieCast**.

### Key Relationships
- **Movie ↔ Rating**: Movies are rated by users.
- **Movie ↔ Director**: Movies are directed by a director.
- **Movie ↔ Producer**: Movies are produced by producers.
- **Movie ↔ Genre**: Movies belong to one or more genres.
- **Movie ↔ Language**: Movies can be in one or more languages.
- **Movie ↔ MoodType**: Movies are categorized by mood types.
- **Movie ↔ MovieCast**: Tracks the lead hero in the cast.
- **User ↔ Rating**: Users can rate movies.

For more detailed information about the entities and their relationships, please refer to the **Entity-Relationship Diagram (ERD)** section in the documentation.

### Relational Data Model (RDM)
The system's database schema is based on the following tables:

- **Movie**: MovieID (PK), MovieName, ReleaseYear, OriginalTitle, MovieIndustry, Length, Plot
- **Director**: DirectorID (PK), DirectorName
- **Producer**: ProducerID (PK), ProducerName
- **Genre**: GenreID (PK), GenreName
- **Language**: LanguageID (PK), LanguageName
- **MoodType**: MoodID (PK), MoodName
- **MovieCast**: CastID (PK), LeadHeroName
- **User**: UserID (PK), FirstName, LastName, Username, Email, Password, UserType
- **ProductionCompany**: CompanyID (PK), CompanyName
- **Rating**: RatingID (PK), RatingValue

### ERD Constructs and Symbols
- **Rectangles**: Represent entities.
- **Ellipses**: Represent attributes.
- **Diamonds**: Represent relationships.
- **Lines with Cardinality**: Indicate the type of relationship (1:1, 1:N, M:N).

### Implementation Details
- One-to-many relationships (e.g., Movie and Rating) were implemented using foreign keys.
- Many-to-many relationships (e.g., IsRatedBy) were resolved using associative tables.

## How to Use
1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-username/movie-management-system.git
   ```

2. **Set up the database**:
   Import the schema into your database management system (e.g., MySQL, PostgreSQL).

3. **Add Movie Data**:
   You can add movies, directors, producers, and other related entities through the database tables.

4. **Rate Movies**:
   Users can rate movies by inserting ratings into the `Rating` table.

## Future Enhancements
- **User Functionalities**: Additional features such as user profile management, watchlist, and notifications.
- **Advanced Reporting**: Implement advanced reporting capabilities such as top-rated movies, movies by genre, and more.
- **API Integration**: Expose the system’s functionality via a RESTful API.
