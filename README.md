# Library System API Documentation

## Base URL: https://fci-library.me

## Authentication
The application uses JWT-based authentication. Each request to a protected endpoint must include a valid JWT token in the Authorization header.

```
Authorization: Bearer <your_token>
```

---

## Users

### Register User
**POST** `/api/User/register`
#### Payload:
```json
{
    "name": "string",
    "email": "string",  
    "password": "string",
    "department": "string",  
    "phone": "string",
    "year": 1-4
}
```
#### Response:
- **200 OK**: User Registered Successfully
- **400 Bad Request**: Email or phone number taken / User exists
- **404 Not Found**: Server down

---

### Login User
**POST** `/api/user/login`
#### Payload:
```json
{
    "email": "string",
    "password": "string"
}
```
#### Response:
- **200 OK**: Login Successful (Returns user data)
- **400 Bad Request**: Doesn't Exist / Wrong Password

---

### Get User Profile by ID
**GET** `/api/user/profile/searchid/{userid}`
#### Response:
- **200 OK**: Returns user data
- **400 Bad Request**: User doesn't exist

### Get User Profile by Email
**GET** `/api/user/profile/search/{email}`
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
  "name": "string",
  "email": "string",
  "department": "string",
  "phone": "string",
  "year": 1-4
}
```
#### Response:
- **200 OK**: User updated
- **400 Bad Request**: User doesn't exist

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

---

## Books

### Add a Book
**POST** `/api/Book/add`
#### Payload:
```json
{
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "isAvailable": true,
  "department": "string",
  "assignedYear": int,
  "state": true,
  "image": "string",
  "categoryNames": ["string"],
  "authorNames": ["string"]
}
```
#### Response:
- **200 OK**: Book added successfully

---

### Get Book by ID
**GET** `/api/book/{id}`
#### Response:
- **200 OK**: Returns book details

### Search Book by Name
**GET** `/api/book/search?name={name}`
(Same response format as getting book by ID)

### Search Book by Genre
**GET** `/api/book/genre?genre={genre}`

### Search Book by Author
**GET** `/api/book/author?author={author}`

### Get Books by Availability
**GET** `/api/Book/?isAvailable=true`

### Get Books by Year
**GET** `/api/book/year/{id}`

### Get Books by Department
**GET** `/api/Book/?department={department}`

### Get All Books
**GET** `/api/Book`
Returns all books in list format.

---

### Update a Book
**PUT** `/api/Book/update/{id}`
#### Payload:
```json
{
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "isAvailable": true,
  "department": "string",
  "assignedYear": int,
  "state": true,
  "image": "string",
  "categoryNames": ["string"],
  "authorNames": ["string"]
}
```
#### Response:
- **200 OK**: Book updated successfully
- **404 Not Found**: Book not found or update failed

---

### Delete a Book
**DELETE** `/api/Book/delete/{id}`
#### Response:
- **200 OK**: Book deleted

