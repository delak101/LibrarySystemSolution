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
- **200 OK**: Login Successful (Returns user data With **TOKEN**) use token for authorized access
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


### **Bulk Delete Users**
 
**DELETE** `/api/user/deleteyear/{year}`  
#### **Response**: 
- **200 OK**: Users deleted successfully 
- **400 Bad Request**: Some or all users not found

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
  "assignedYear": 1-4,
  "state": true,
  "image": "string",
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

### Search Books
**GET** `/api/book/search?name={name}`  
**GET** `/api/book/genre?genre={genre}`  
**GET** `/api/book/author?author={author}`  

#### Response:
- **200 OK**: Returns books matching the search criteria.

---

### Get Books by Availability
**GET** `/api/book?isAvailable=true`

### Get Books by Year
**GET** `/api/book/year/{year}`

### Get Books by Department
**GET** `/api/book?department={department}`

### Get All Books
**GET** `/api/Book`

#### Response:
- **200 OK**: Returns all books in JSON format.

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
  "assignedYear": 1-4,
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

---

## Borrow Requests

### Request to Borrow a Book
**POST** `/api/borrow/request`
#### Payload:
```json
{
  "userId": 0,
  "bookId": 0,
  "dueDate": "$DateTime [yyyy/mm/dd]"
}
```
#### Response:
- **200 OK**: Request submitted.
- **400 Bad Request**: Book not available.

---

### View Borrow Requests
**GET** `/api/borrow/requests`
#### Response:
- **200 OK**: Returns a list of pending borrow requests.

### Approve Borrow Request
**PUT** `/api/borrow/approve/{requestId}`
#### Response:
- **200 OK**: Request approved, book marked as borrowed.

### Deny Borrow Request
**DELETE** `/api/borrow/deny/{requestId}`
#### Response:
- **200 OK**: Request denied.

---

### Return a Book
**PUT** `/api/borrow/return/{bookId}`
#### Response:
- **200 OK**: Book returned successfully.

---

### View Borrow History
**GET** `/api/borrow/history/{userId}`
#### Response:
- **200 OK**: Returns the user's borrow history.

### Get Overdue Books
**GET** `/api/borrow/overdue`
#### Response:
- **200 OK**: Returns a list of overdue books.

---

## **Favorites**
 
### **Add to Favorites**
 
**POST** `/api/favorite/{userId}/{bookId}`
#### **Response**: 
- **200 OK**: Book added to favorites
- **400 Bad Request**: Book already favorited
 
---

 
### **Remove from Favorites**
**DELETE** `/api/favorite/{userId}/{bookId}`
 
#### **Response**: 
- **200 OK**: Book removed from favorites 
- **404 Not Found**: Favorite not found 

---

### **Get User’s Favorite Books** 
**GET** `/api/favorite/{userId}` 
#### **Response**: 
- **200 OK**: Returns a list of favorited books
 
