# **Library System API Documentation**  

## **Base URL**  
`https://app-librarysystem-centralus-dev-001.azurewebsites.net`  

---

## **Endpoints**  

### **1. User APIs**  

#### **Register User**  
- **Endpoint:** `/api/User/register`  
- **Method:** POST  
- **Payload:**  
```json
{
  "name": "string",
  "email": "string",
  "password": "string",
  "role": "string",
  "department": "string",
  "phone": "string",
  "year": 0
}
```  
- **Description:** Registers a new user with the provided details.  

#### **Login User**  
- **Endpoint:** `/api/User/login`  
- **Method:** POST  
- **Payload:**  
```json
{
  "email": "string",
  "password": "string"
}
```  
- **Description:** Authenticates the user and returns a token upon successful login.  

#### **Search User Profile**  
- **Endpoint:** `/api/User/profile/search/{userId}`  
- **Method:** GET  
- **Response:**  
```json
{
  "id": 0,
  "name": "string",
  "email": "string",
  "role": "string",
  "department": "string",
  "token": "string"
}
```  
- **Description:** Retrieves the profile details for a specific user by ID.  

#### **Update User Profile**  
- **Endpoint:** `/api/User/profile/update/{userId}`  
- **Method:** PUT  
- **Payload:**  
```json
{
  "name": "string",
  "email": "string",
  "department": "string",
  "phone": 0,
  "year": 0
}
```  
- **Description:** Updates the profile details for the specified user ID.  

#### **Delete User Profile**  
- **Endpoint:** `/api/User/profile/delete/{userId}`  
- **Method:** DELETE  
- **Description:** Deletes the profile for the specified user ID.  

---

### **2. Book APIs**  

#### **Add Book**  
- **Endpoint:** `/api/Book/addBook`  
- **Method:** POST  
- **Payload:**  
```json
{
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "state": true,
  "department": "string",
  "year": 0
}
```  
- **Description:** Adds a new book to the library system.  

#### **Search Book by ID**  
- **Endpoint:** `/api/Book/search/{bookId}`  
- **Method:** GET  
- **Response:**  
```json
{
  "id": 0,
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "state": true,
  "department": "string",
  "year": 0
}
```  
- **Description:** Retrieves the details of a specific book by ID.  

#### **Search All Books**  
- **Endpoint:** `/api/Book/search`  
- **Method:** GET  
- **Response:**  
```json
[
  {
    "id": 0,
    "name": "string",
    "author": "string",
    "description": "string",
    "shelf": "string",
    "state": true,
    "department": "string",
    "year": 0
  }
]
```  
- **Description:** Retrieves a list of all books in the library system.  

#### **Update Book Details**  
- **Endpoint:** `/api/Book/update/{bookId}`  
- **Method:** PUT  
- **Payload:**  
```json
{
  "name": "string",
  "author": "string",
  "description": "string",
  "shelf": "string",
  "state": true,
  "department": "string",
  "year": 0
}
```  
- **Description:** Updates the details of a specific book by ID.  

#### **Delete Book**  
- **Endpoint:** `/api/Book/delete/{bookId}`  
- **Method:** DELETE  
- **Description:** Deletes a specific book from the library system by ID.  

---

TODO
- **Authentication:** Include necessary authentication headers, e.g., `Authorization: Bearer <token>`, if applicable.  
- **Error Handling:** Ensure to handle common HTTP errors like `400 Bad Request`, `401 Unauthorized`, and `404 Not Found`.  
- **Content-Type:** Use `Content-Type: application/json` for all POST/PUT requests.  
