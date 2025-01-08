# Library System API Documentation

## Base URL
- **IPv4**: http://167.172.105.89

## Authentication
The application uses JWT-based authentication. Each request to a protected endpoint must include a valid JWT token in the Authorization header.

```
Authorization: Bearer <token>
```

## Endpoints

### Users

#### 1. Register a User
- **Endpoint**: `POST /api/users/register`
- **Description**: Registers a new user.
- **Request Body**:
  ```json
  {
    "name": "string",
    "email": "string",
    "password": "string",
    "role": "admin|student",
    "department": "IT|CS|IS"
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "id": "int",
      "name": "string",
      "email": "string",
      "role": "string",
      "department": "string"
    }
    ```

#### 2. Login
- **Endpoint**: `POST /api/users/login`
- **Description**: Authenticates a user and returns a JWT token.
- **Request Body**:
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "token": "string"
    }
    ```

### Books

#### 1. Add a Book
- **Endpoint**: `POST /api/books`
- **Description**: Adds a new book to the library.
- **Request Body**:
  ```json
  {
    "name": "string",
    "author": "string",
    "description": "string",
    "shelf": "string",
    "state": "available|borrowed",
    "department": "IT|CS|IS",
    "year": "int"
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "id": "int",
      "name": "string",
      "author": "string",
      "description": "string",
      "shelf": "string",
      "state": "string",
      "department": "string",
      "year": "int"
    }
    ```

#### 2. Get All Books
- **Endpoint**: `GET /api/books`
- **Description**: Retrieves all books in the library.
- **Response**:
  - **200 OK**:
    ```json
    [
      {
        "id": "int",
        "name": "string",
        "author": "string",
        "description": "string",
        "shelf": "string",
        "state": "string",
        "department": "string",
        "year": "int"
      }
    ]
    ```

### Borrow Requests

#### 1. Create a Borrow Request
- **Endpoint**: `POST /api/borrow-requests`
- **Description**: Submits a borrow request for a book.
- **Request Body**:
  ```json
  {
    "bookId": "int",
    "studentId": "int",
    "days": "int"
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "id": "int",
      "date": "string",
      "bookId": "int",
      "studentId": "int",
      "days": "int"
    }
    ```

#### 2. Get Borrow Requests
- **Endpoint**: `GET /api/borrow-requests`
- **Description**: Retrieves all borrow requests.
- **Response**:
  - **200 OK**:
    ```json
    [
      {
        "id": "int",
        "date": "string",
        "bookId": "int",
        "studentId": "int",
        "days": "int"
      }
    ]
    ```

### Favorites

#### 1. Add a Favorite Book
- **Endpoint**: `POST /api/favorites`
- **Description**: Marks a book as a favorite for a student.
- **Request Body**:
  ```json
  {
    "studentId": "int",
    "bookId": "int"
  }
  ```
- **Response**:
  - **201 Created**:
    ```json
    {
      "studentId": "int",
      "bookId": "int"
    }
    ```

#### 2. Get Favorite Books
- **Endpoint**: `GET /api/favorites/{studentId}`
- **Description**: Retrieves a student's favorite books.
- **Response**:
  - **200 OK**:
    ```json
    [
      {
        "studentId": "int",
        "bookId": "int"
      }
    ]
    ```

## Error Responses

- **400 Bad Request**: Invalid input or request.
  ```json
  {
    "error": "string"
  }
  ```
- **401 Unauthorized**: Authentication failed.
  ```json
  {
    "error": "Unauthorized"
  }
  ```
- **404 Not Found**: Resource not found.
  ```json
  {
    "error": "Not Found"
  }
  ```
- **500 Internal Server Error**: Unexpected error.
  ```json
  {
    "error": "Internal Server Error"
  }
  ```

