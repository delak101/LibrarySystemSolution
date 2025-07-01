# Library System API Documentation

## Base URLs

- **Production:** `https://fci-library.live`
- **Development:** `https://dev.fci-library.live`

## Authentication
The application uses JWT-based authentication. Each request to a protected endpoint must include a valid JWT token in the Authorization header.

```
Authorization: Bearer <your_token>
```

---

## Users

### Register User
**POST** `/api/user/register`
#### Payload:
```json
{
    "pfp": "base64-profile-image",
    "name": "John Doe",
    "email": "johndoe@example.com",
    "studentEmail": "johndoe@fci.zu.edu",
    "password": "strongpassword",
    "nationalId": "12345678912345",
    "department": "IT",
    "phone": "0123456789",
    "year": 2,
    "termsAccepted": true
}
```
#### Response:
- **200 OK**: `{ "message": "User registered successfully." }`
- **400 Bad Request**: `{ "message": "User already exists." }`
- **404 Not Found**: You're using wrong endpoint
- **500 Internal Server Error**: Server down

---

### Login User
**POST** `/api/user/login`
#### Payload:
```json
{
    "email": "johndoe@example.com",
    "password": "strongpassword"
}
```
#### Response:
- **200 OK**: Login Successful (Returns user data with **TOKEN**) - use token for authorized access
- **400 Bad Request**:
  - `{ "message": "User does not exist." }`
  - `{ "message": "Invalid password." }`

---

### Forgot Password
**POST** `/api/user/forgot-password`
#### Payload:
```json
{
    "email": "johndoe@example.com"
}
```
#### Response:
- **200 OK**: `{ "message": "Password reset initiated. Please check your email." }`
- **400 Bad Request**: `{ "message": "Email not found." }`

---

### Reset Password
**POST** `/api/user/reset-password`
#### Payload:
```json
{
    "token": "token-received-in-email",
    "newPassword": "newstrongpassword"
}
```
#### Response:
- **200 OK**: `{ "message": "Password reset successful." }`
- **400 Bad Request**: `{ "message": "Invalid or expired token." }`

---

### Get User Profile by ID
**GET** `/api/user/profile/searchid/{userid}`
#### Response:
- **200 OK**:
```json
{
    "id": 1,
    "pfp": "base64-profile-image",
    "name": "John Doe",
    "email": "johndoe@example.com",
    "studentEmail": "johndoe@college.edu",
    "nationalId": "12345678912345",
    "department": "IT",
    "phone": "0123456789",
    "year": 2
}
```
- **404 Not Found**: `{ "message": "User not found." }`

### Get User Profile by Email
**GET** `/api/user/profile/search/{email}`
(Same response as search by ID)

### Get User Profile by Name
**GET** `/api/user/profile/search/name/{name}`
(Same response as search by ID)

### Get All Users
**GET** `/api/user/all`
Returns all users in JSON format.

---

### Update User Profile by ID
**PUT** `/api/user/profile/updateid/{userid}`
#### Payload:
```json
{
    "pfp": "base64-profile-image",
    "name": "John Doe",
    "email": "john.doe@gmail.com",
    "studentEmail": "john.doe@fci.zu.edu.eg",
    "nationalId": "1234568912345",
    "department": "CS",
    "phone": "0987654321",
    "year": 3
}
```
#### Response:
- **200 OK**: `{ "message": "User updated successfully." }`
- **404 Not Found**: `{ "message": "User not found or update failed." }`

### Update User Profile by Email
**PUT** `/api/user/profile/update/{email}`
(Same response as update by ID)

---

### Delete User by ID
**DELETE** `/api/user/profile/deleteid/{id}`
#### Response:
- **200 OK**: User deleted
- **400 Bad Request**: User doesn't exist

### Delete User by Email
**DELETE** `/api/user/profile/delete/{email}`
(Same response as delete by ID)

### Bulk Delete Users
**DELETE** `/api/user/profile/deleteyear/{year}`
#### Response: 
- **200 OK**: Users deleted successfully 
- **400 Bad Request**: Some or all users not found

---

## Books

### Add a Book
**POST** `/api/book/add`
#### Payload:
```json
{
    "name": "string",
    "author": "string",
    "description": "string",
    "shelf": "string",
    "isAvailable": true,
    "department": "string",
    "assignedYear": 1,
    "state": true,
    "image": "base64-cover-image",
    "categoryNames": [
        "string"
    ],
    "authorNames": [
        "string"
    ]
}
```
#### Response:
- **200 OK**: `{ "message": "Book added successfully." }`
- **400 Bad Request**: `{ "message": "Failed to add the book." }`

---

### Get Book by ID
**GET** `/api/book/{id}`
#### Response:
- **200 OK**: Returns book details
- **404 Not Found**: `{ "message": "Book not found." }`

### Search Books by Name
**GET** `/api/book/search?name={name}`
#### Response:
- **200 OK**: Returns books matching the search criteria
- **404 Not Found**: `{ "message": "No books found with this name." }`

### Get Books by Genre
**GET** `/api/book/genre?genre={genre}`
#### Response:
- **200 OK**: Returns books matching the genre
- **404 Not Found**: `{ "message": "No books found for this genre." }`

### Get Books by Author
**GET** `/api/book/author?author={author}`
#### Response:
- **200 OK**: Returns books by the specified author
- **404 Not Found**: `{ "message": "No books found for this author." }`

---

### Get Books by Availability
**GET** `/api/book/available/{isAvailable}`
#### Response:
- **200 OK**: Returns available/unavailable books
- **404 Not Found**: `{ "message": "No books available for borrowing." }`

### Get Books by Year
**GET** `/api/book/year/{year}`
#### Response:
- **200 OK**: Returns books for the specified year
- **404 Not Found**: `{ "message": "No books found for this year." }`

### Get Books by Department
**GET** `/api/book/department/{department}`
#### Response:
- **200 OK**: Returns books for the specified department
- **404 Not Found**: `{ "message": "No books found for this department." }`

### Get All Books
**GET** `/api/book`
#### Response:
- **200 OK**: Returns all books in JSON format

### Get Books (Paginated) 🆕
**GET** `/api/book/paged?page={page}&pageSize={pageSize}`
#### Parameters:
- `page` (optional): Page number, defaults to 1
- `pageSize` (optional): Items per page, defaults to 10, max 50

#### Response:
- **200 OK**: Returns paginated books
```json
{
    "items": [...],
    "totalCount": 150,
    "page": 1,
    "pageSize": 10,
    "totalPages": 15,
    "hasNextPage": true,
    "hasPreviousPage": false
}
```

### Get All Authors
**GET** `/api/book/authors`
#### Response:
- **200 OK**: Returns all authors in JSON format

---

### Update a Book
**PUT** `/api/book/update/{id}`
#### Payload:
```json
{
    "name": "string",
    "author": "string",
    "description": "string",
    "shelf": "string",
    "isAvailable": true,
    "department": "string",
    "assignedYear": 1,
    "state": true,
    "image": "base64-cover-image",
    "categoryNames": ["string"],
    "authorNames": ["string"]
}
```
#### Response:
- **200 OK**: `{ "message": "Book updated successfully." }`
- **404 Not Found**: `{ "message": "Book not found or update failed." }`

---

### Delete a Book
**DELETE** `/api/book/delete/{id}`
#### Response:
- **200 OK**: `{ "message": "Book deleted successfully." }`
- **404 Not Found**: `{ "message": "Book not found." }`

---

## Borrow Requests

### Request to Borrow a Book
**POST** `/api/borrow/request?userId={userId}&bookId={bookId}`
#### Parameters:
- `userId`: ID of the user requesting to borrow
- `bookId`: ID of the book to borrow

#### Response:
- **200 OK**: `{ "message": "Borrow request submitted successfully", "borrow": {...} }`
- **400 Bad Request**: `{ "error": "Book not available or other error" }`
- **500 Internal Server Error**: `{ "error": "An unexpected error occurred while processing your request" }`

---

### Get Pending Borrow Requests
**GET** `/api/borrow/pending`
#### Response:
- **200 OK**: Returns a list of pending borrow requests

### Get Borrowed Books
**GET** `/api/borrow/borrowed`
#### Response:
- **200 OK**: Returns a list of currently borrowed books

### Get Overdue Books
**GET** `/api/borrow/overdue`
#### Response:
- **200 OK**: Returns a list of overdue books

### Approve Borrow Request
**POST** `/api/borrow/approve/{borrowId}`
#### Response:
- **200 OK**: `{ "message": "Borrow request approved" }`
- **400 Bad Request**: `{ "error": "Failed to approve request" }`

### Deny Borrow Request
**POST** `/api/borrow/deny/{borrowId}`
#### Response:
- **200 OK**: `{ "message": "Borrow request denied" }`
- **400 Bad Request**: `{ "error": "Failed to deny request" }`

---

### Return a Book
**POST** `/api/borrow/return/{borrowId}`
#### Response:
- **200 OK**: `{ "message": "Book returned successfully" }`
- **400 Bad Request**: `{ "error": "Failed to return book" }`

---

### View Borrow History
**GET** `/api/borrow/history/{userId}`
#### Response:
- **200 OK**: Returns the user's borrow history

---

## Favorites

### Add to Favorites
**POST** `/api/favorite/{userId}/{bookId}`
#### Response: 
- **200 OK**: `{ "message": "Book added to favorites." }`
- **400 Bad Request**: `{ "message": "Book is already favorited." }`

---

### Remove from Favorites
**DELETE** `/api/favorite/{userId}/{bookId}`
#### Response: 
- **200 OK**: `{ "message": "Book removed from favorites." }`
- **404 Not Found**: `{ "message": "Favorite not found." }`

---

### Check if Book is Favorited
**GET** `/api/favorite/{userId}/{bookId}`
#### Response: 
- **200 OK**: `{ "isFavorite": true/false }`

---

### Get User's Favorite Books
**GET** `/api/favorite/{userId}`
#### Response: 
- **200 OK**: Returns a list of favorited books

---

## Analytics 🆕

### Get Analytics Data
**GET** `/api/analytics`
#### Response: 
- **200 OK**: Returns analytics data
```json
{
    "totalBooks": 150,
    "pendingRequests": 12,
    "totalStudents": 300,
    "borrowedBooks": 45
}
```
- **500 Internal Server Error**: `{ "error": "An error occurred while fetching analytics data" }`

---

## Notifications 🆕

### Register Device Token
**POST** `/api/notification/register-token`
*Requires Authentication*
#### Payload:
```json
{
    "deviceToken": "firebase-device-token",
    "deviceType": "iOS/Android"
}
```
#### Response: 
- **200 OK**: `{ "message": "Device token registered successfully" }`
- **400 Bad Request**: `{ "message": "Failed to register device token" }`
- **401 Unauthorized**: Invalid user

### **Register Device Token**
**POST** `/api/notification/register-token`
*Requires Authentication*
#### Payload:
```json
{
  "deviceToken": "firebase-device-token",
  "deviceType": "iOS/Android"
}
```
#### **Response**: 
- **200 OK**: `{ "message": "Device token registered successfully" }`
- **400 Bad Request**: `{ "message": "Failed to register device token" }`
- **401 Unauthorized**: Invalid user
```json
{
    "pfp": "base64-profile-image",
    "name": "John Doe",
    "email": "johndoe@example.com",
    "studentEmail": "johndoe@fci.zu.edu",
    "password": "strongpassword",
    "nationalId": "12345678912345",
    "department": "IT",
    "phone": "0123456789",
    "year": 2,
    "termsAccepted": true
}
```
#### Response:
- **200 OK**: ``{ "message": "User registered successfully." }``
- **400 Bad Request**: ``{ "message": "User already exists." }``
- **404 Not Found**: you're using wrong endpoint.
- **500 Internal Server Error**: server down.

---

### Login User
**POST** `/api/user/login`
#### Payload:
```json
{
    "email": "johndoe@example.com",
    "password": "strongpassword"
}
```
#### Response:
- **200 OK**: Login Successful (Returns user data With **TOKEN**) use token for authorized access
- **400 Bad Request**:
  -``{ "message": "User does not exist." }``
  -``{ "message": "Invalid password." }``

---

### Forgot Password
**POST** `/api/user/forgot-password`
#### Payload:
```json
{
    "email": "johndoe@example.com"
}
```
#### Response:
- **200 OK**: ``{ "message": "Password reset initiated. Please check your email." }``
- **400 Bad Request**: ``{ "message": "Email not found." }``

---

### Reset Password
**POST** `/api/user/reset-password`
#### Payload:
```json
{
    "token": "token-received-in-email",
    "newPassword": "newstrongpassword"
}
```
#### Response:
- **200 OK**: ``{ "message": "Password reset successful." }``
- **400 Bad Request**: ``{ "message": "Invalid or expired token." }``

---
### Get User Profile by ID
**GET** `/api/user/profile/searchid/{userid}`
#### Response:
- **200 OK**:
```json
    {
      "id": 1,
      "pfp": "base64-profile-image",
      "name": "John Doe",
      "email": "johndoe@example.com",
      "studentEmail": "johndoe@college.edu",
      "nationalId": "12345678912345",
      "department": "IT",
      "phone": "0123456789",
      "year": 2
    }
```

- **404 NotFound**: `{ "message": "User not found." }`

### Get User Profile by Email
**GET** `/api/user/profile/search/{email}`
(Same response as search by ID)

### Get User Profile by Name
**GET** `/api/user/profile/search/name/{name}`
(Same response as search by ID)

### Get All Users
**GET** `/api/user/all`
Returns all users in JSON format.

---

### Update User Profile by ID
**PUT** `/api/User/profile/updateid/{userid}`
#### Payload:
```json
{
    "pfp": "base64-profile-image",
    "name": "John Doe",
    "email": "john.doe@gmail.com",
    "studentEmail": "john.doe@fci.zu.edu.eg",
    "nationalId": "1234568912345",
    "department": "CS",
    "phone": "0987654321",
    "year": 3
}
```
#### Response:
- **200 OK**: `{ "message": "User updated successfully." }`
- **404 NotFound**: `{ "message": "User not found or update failed." }`

### Update User Profile by Email
**PUT** `/api/User/profile/update/{email}`
(Same response as update by ID)

---

### Delete User by ID
**DELETE** `/api/user/profile/deleteid/{id}`
#### Response:
- **200 OK**: User deleted
- **400 Bad Request**: User doesn't exist

### Delete User by Email
**DELETE** `/api/user/profile/delete/{email}`
(Same response as delete by ID)


### **Bulk Delete Users**

**DELETE** `/api/user/profile/deleteyear/{year}`
#### **Response**: 
- **200 OK**: Users deleted successfully 
- **400 Bad Request**: Some or all users not found

----

## Books

### Add a Book
**POST** `/api/book/add`
#### Payload:
```json
{
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "isAvailable": true,
  "department": "string",
  "assignedYear": 1,
  "state": true,
  "image": "base64-cover-image",
  "categoryNames": [
    "string"
  ],
  "authorNames": [
    "string"
  ]
}
```
#### Response:
- **200 OK**: Book added successfully

---

### Get Book by ID
**GET** `/api/book/{id}`
#### Response:
- **200 OK**: Returns book details
- **404 Not Found**: Book not found

### Search Books by Name
**GET** `/api/book/search?name={name}`
#### Response:
- **200 OK**: Returns books matching the search criteria
- **404 Not Found**: No books found with this name

### Get Books by Genre
**GET** `/api/book/genre?genre={genre}`
#### Response:
- **200 OK**: Returns books matching the genre
- **404 Not Found**: No books found for this genre

### Get Books by Author
**GET** `/api/book/author?author={author}`
#### Response:
- **200 OK**: Returns books by the specified author
- **404 Not Found**: No books found for this author

---

### Get Books by Availability
**GET** `/api/book/available/{isAvailable}`
#### Response:
- **200 OK**: Returns available/unavailable books
- **404 Not Found**: No books available for borrowing

### Get Books by Year
**GET** `/api/book/year/{year}`
#### Response:
- **200 OK**: Returns books for the specified year
- **404 Not Found**: No books found for this year

### Get Books by Department
**GET** `/api/book/department/{department}`
#### Response:
- **200 OK**: Returns books for the specified department
- **404 Not Found**: No books found for this department

### Get All Books
**GET** `/api/book`
#### Response:
- **200 OK**: Returns all books in JSON format

### Get Books (Paginated) 🆕
**GET** `/api/book/paged?page={page}&pageSize={pageSize}`
#### Parameters:
- `page` (optional): Page number, defaults to 1
- `pageSize` (optional): Items per page, defaults to 10, max 50

#### Response:
- **200 OK**: Returns paginated books
```json
{
  "items": [...],
  "totalCount": 150,
  "page": 1,
  "pageSize": 10,
  "totalPages": 15,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

### Get All Authors
**GET** `/api/book/authors`
#### Response:
- **200 OK**: Returns all authors in JSON format

---

### Update a Book
**PUT** `/api/book/update/{id}`
#### Payload:
```json
{
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "isAvailable": true,
  "department": "string",
  "assignedYear": 1,
  "state": true,
  "image": "base64-profile-image",
  "categoryNames": ["string"],
  "authorNames": ["string"]
}
```
#### Response:
- **200 OK**: Book updated successfully
- **404 Not Found**: Book not found or update failed

---

### Delete a Book
**DELETE** `/api/book/delete/{id}`
#### Response:
- **200 OK**: Book deleted successfully
- **404 Not Found**: Book not found

---

## Borrow Requests

### Request to Borrow a Book
**POST** `/api/borrow/request?userId={userId}&bookId={bookId}`
#### Parameters:
- `userId`: ID of the user requesting to borrow
- `bookId`: ID of the book to borrow

#### Response:
- **200 OK**: `{ "message": "Borrow request submitted successfully", "borrow": {...} }`
- **400 Bad Request**: `{ "error": "Book not available or other error" }`
- **500 Internal Server Error**: Unexpected error occurred

---

### Get Pending Borrow Requests
**GET** `/api/borrow/pending`
#### Response:
- **200 OK**: Returns a list of pending borrow requests

### Get Borrowed Books
**GET** `/api/borrow/borrowed`
#### Response:
- **200 OK**: Returns a list of currently borrowed books

### Get Overdue Books
**GET** `/api/borrow/overdue`
#### Response:
- **200 OK**: Returns a list of overdue books

### Approve Borrow Request
**POST** `/api/borrow/approve/{borrowId}`
#### Response:
- **200 OK**: `{ "message": "Borrow request approved" }`
- **400 Bad Request**: `{ "error": "Failed to approve request" }`

### Deny Borrow Request
**POST** `/api/borrow/deny/{borrowId}`
#### Response:
- **200 OK**: `{ "message": "Borrow request denied" }`
- **400 Bad Request**: `{ "error": "Failed to deny request" }`

---

### Return a Book
**POST** `/api/borrow/return/{borrowId}`
#### Response:
- **200 OK**: `{ "message": "Book returned successfully" }`
- **400 Bad Request**: `{ "error": "Failed to return book" }`

---

### View Borrow History
**GET** `/api/borrow/history/{userId}`
#### Response:
- **200 OK**: Returns the user's borrow history

---

## **Favorites**
 
### **Add to Favorites**

**POST** `/api/favorite/{userId}/{bookId}`
#### **Response**: 
- **200 OK**: `{ "message": "Book added to favorites." }`
- **400 Bad Request**: `{ "message": "Book is already favorited." }`

---

### **Remove from Favorites**
**DELETE** `/api/favorite/{userId}/{bookId}`

#### **Response**: 
- **200 OK**: `{ "message": "Book removed from favorites." }`
- **404 Not Found**: `{ "message": "Favorite not found." }` 

---

### **Get User’s Favorite Books** 
**GET** `/api/favorite/{userId}` 
#### **Response**: 
- **200 OK**: Returns a list of favorited books
 
