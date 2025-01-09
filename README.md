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
- **Endpoint**: `POST /api/user/register`
- **Description**: Registers a new user.
- **Request Body**:
  ```json
  {
    "name": "string",
    "email": "string@text.com",
    "password": "string",
    "role": 0,                  #( 0 admin/ 1 student )
    "department": "IT|CS|IS",
    "phone": "string",
    "year": 3                     #(1,2,3,4)
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
    "message": "User registered successfully."
    }
    ```
    
  - **400 BAD**:
    ```json
    {
    "message": "User already exists or registration failed."
    }
    ```
    ps: todo -> phone number has to be unique as well. if phone number exist could get response like:
      - **400 BAD**:
        ```json
        {
            "hint": "problem here",
            "message": "A user with this email already exists. also problem with AddAsync or savingchanges"
        }       
        ```


#### 2. Login
- **Endpoint**: `POST /api/user/login`
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
    "user": {
        "id": 1,
        "name": "test",
        "email": "test@test.com",
        "role": 0,
        "department": "IT",
        "year": 4,
        "phone": "8934819"
    },
    "token": "very secret token"
    }
    ```
  - **401 Unauthorized**:
    ```json
    {
    "message": "User does not exist."
    }
    ```
    ```json
    {
        "message": "Invalid password."
    }
    ```

#### 3. Get a User
  ##### .1 ID
  - **Endpoint**: `GET /api/user/profile/searchid/{id}`
  - **Description**: Get a user by Id.
  - **Response**:
    - **200 OK**:
      ```json
      {
          "id": 2,
          "name": "test2",
          "email": "test2@test.com",
          "role": 0,
          "department": "IT",
          "year": 4,
          "phone": "854519"
      }
      ```
      
    - **404 Not Found**:
      - User not found
      
  ##### .2 Email
  - **Endpoint**: `GET /api/user/profile/search/{email}`
  - **Description**: Get a user by Email.
  - **Response**:
    - **200 OK**:
      ```json
      {
          "id": 2,
          "name": "test2",
          "email": "test2@test.com",
          "role": 0,
          "department": "IT",
          "year": 4,
          "phone": "854519"
      }
      ```
      
    - **404 Not Found**:
      - User not found
      
#### 4. Get All Users
- **Endpoint**: `GET /api/user/all`
- **Description**: Authenticates a user and returns a JWT token.
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "id": 1,
            "name": "test",
            "email": "test@test.com",
            "role": 0,
            "department": "IT",
            "year": 4,
            "phone": "8934819"
        },
        {
            "id": 2,
            "name": "test2",
            "email": "test2@test.com",
            "role": 0,
            "department": "IT",
            "year": 4,
            "phone": "854519"
        }
    ]
    ```
    
#### 5. Update a User
  ##### .1 ID
  - **Endpoint**: `PUT /api/user/profile/updateid/{id}`
  - **Description**: Update existing user by id.
  - **Request Body**:
    ```json
    {
      "name": "test4",
      "email": "test@test.com",
      "department": "CS",
      "phone": "654-4520",
      "year": 4
    }
    ```
  - **Response**:
    - **200 OK**:
      User updated successfully.

    - **404 Not Found**:
      User not found or update failed.
      
    - **500 Internal Server Error**:
        Error message indicating user with email exist (not handled yet)
      
  ##### .2 Email
  - **Endpoint**: `PUT /api/user/profile/update/{email}`
  - **Description**: Update existing user by email.
  - **Request Body**:
    ```json
    {
      "name": "test4",
      "email": "test@test.com",
      "department": "CS",
      "phone": "654-4520",
      "year": 4
    }
    ```
  - **Response**:
    - **200 OK**:
        User updated successfully.

    - **404 Not Found**:
      User not found or update failed.
      
    - **500 Internal Server Error**:
        Error message indicating user with email exist (not handled yet)
      
#### 6. Delete a User
  ##### ID
  - **Endpoint**: `DELETE /api/user/profile/deleteid/{id}`
  - **Description**: Delete existing user by id.
  - **Response**:
    - **200 OK**:
      User not found or update failed
      
    - **500 Internal Server Error**:
      System.InvalidOperationException: User not found.
      
  ##### Email
  - **Endpoint**: `PUT /api/user/profile/update/{email}`
  - **Description**: Update existing user by email.
  - **Request Body**:
    ```json
    {
      "name": "test4",
      "email": "test@test.com",
      "department": "CS",
      "phone": "654-4520",
      "year": 4
    }
    ```
  - **Response**:
    - **200 OK**:
      User not found or update failed
      
    - **404 Not Found**:
      User not found.

    - **500 Internal Server Error**:
        Error message indicating user with email exist (not handled yet)
  

---

#### **IN PROGRESS**

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

