# EventManagerServerApi

ASP.NET Core Web API Server for managing events, suppliers, and users.

## 🚀 Project Overview

**EventManagerServerApi** is the server-side component of the Event Manager system.
It provides a RESTful API for managing:

- Events
- Suppliers
- Users (including login and authentication)

Main features:
- Create, view, update, and delete (CRUD) operations for events and suppliers.
- Assign multiple suppliers to specific events (many-to-many relationship).
- User registration and login with hashed password security.
- JWT authentication for secured access to protected endpoints.
- Role-based authorization (Event Manager role).
- Connection to a Microsoft SQL Server database.
- Swagger UI for API testing and documentation (development mode).

The server communicates with the React-based client application (**EventManagerClient**).

---

## ⚙️ How to Run Locally

### 1. Clone the repository

```bash
git clone https://github.com/yaron-yunayev/EventMangerServerApi.git
cd EventMangerServerApi
```

### 2. Configure the Local Environment

Create a file named `appsettings.Development.json` in the project root:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EventMangerDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
  },
  "Jwt": {
    "Issuer": "EventManagerApp",
    "Audience": "EventManagerAppClient",
    "SecretKey": "YOUR_SECRET_KEY"
  }
}
```

> ✅ **Note:**
> - Replace `YOUR_SERVER_NAME` with your local SQL Server instance.
> - Replace `YOUR_SECRET_KEY` with a strong random string.

This file is not included in GitHub for security reasons.

---

### 3. Run Database Migrations (if needed)

If the database does not exist yet, run:

```bash
Update-Database
```

(using the Package Manager Console or CLI)

This will create the necessary tables and relationships in the SQL Server database.

---

### 4. Run the Project

```bash
dotnet run
```

Swagger UI will be available at:

```
https://localhost:5001/swagger
```

You can use it to test all API endpoints easily.

---

## 📊 Database Structure

The server is connected to a SQL Server database (`EventMangerDb`) and manages three main entities:

### 1. **User**

- Properties:
  - Id (int)
  - FirstName (string)
  - LastName (string)
  - Email (string)
  - Password (hashed)
  - Role (string) (e.g., EventManager, Admin)
  - IsEventManager (bool)

- Functions:
  - Register new users.
  - Authenticate via login (JWT generation).

### 2. **Supplier**

- Properties:
  - Id (int)
  - Name (string)
  - Email (string)
  - PhoneNumber (string)
  - Address (string)
  - Description (string)
  - Category (enum)
  - ManagerId (user ID who manages it)

- Functions:
  - Full CRUD (Create, Read, Update, Delete).
  - Mark as Favorite for users.

### 3. **Event**

- Properties:
  - Id (int)
  - Name (string)
  - Date (DateTime)
  - Location (string)
  - Description (string)
  - NumberOfGuests (int)
  - ManagerId (user ID who manages it)

- Functions:
  - Full CRUD (Create, Read, Update, Delete).
  - Assign multiple suppliers to a specific event (many-to-many).

There is an `EventSupplier` join table automatically created for many-to-many relationships between events and suppliers.

---

## 🔐 Authentication and Authorization

- Passwords are securely hashed using ASP.NET Core Identity's `PasswordHasher`.
- JWT tokens are generated upon login.
- Role-based authorization ensures that only users with the `EventManager` role can access protected endpoints.

Example of a JWT payload:
```json
{
  "email": "admin@example.com",
  "role": "EventManager",
  "isEventManager": true,
  "IdNumber": 322222222,
}
```

The token must be included in the `Authorization` header as:

```bash
Authorization: Bearer YOUR_JWT_TOKEN
```

---

## 📕 API Documentation (Swagger)

After running the server, access:

```
https://localhost:5001/swagger
```

This provides a full interactive API documentation where you can:
- See available routes.
- Send requests.
- Authenticate with JWT token.
- Test the system easily.

---

## 🌟 Features Summary

- [x] User Registration & Login (Authentication)
- [x] JWT Token Authorization
- [x] CRUD for Events
- [x] CRUD for Suppliers
- [x] Assign Suppliers to Events (Many-to-Many)
- [x] Manage Favorite Suppliers
- [x] API documentation with Swagger

---

## 👩‍💻 Author

Developed by [Yaron Yunayev](https://github.com/yaron-yunayev).

---

## 👍 Notes

- For development, ensure your SQL Server instance is running and accessible.
- Ensure CORS settings are appropriate if connecting the React client locally.
- In production, sensitive data should be stored in safe environment variables (not in plain JSON).

---

