## **README for Bookstore Repository**

### **Overview**
The **Bookstore** project is a modern web application designed for managing a catalog of books with features like CRUD operations, caching using Redis, and an API-driven architecture. It is built using .NET Aspire and incorporates containerized services for scalability and performance.

### **Key Features**
- **API-Driven Design**: A RESTful API that powers the web frontend and any third-party integrations.
- **Redis Caching**: Improves response times by caching frequently accessed data.
- **Containerized Architecture**: Services are isolated and run in Docker containers, ensuring scalability and easy deployment.
- **Extensibility**: Modular design allows for easy addition of new features or microservices.

### **Technologies Used**
- **.NET Aspire**: Used to build and orchestrate the application components.
- **Redis**: For caching data.
- **Docker**: Containerizes services for consistency across environments.
- **StackExchange.Redis**: Library for connecting to Redis from .NET.

---

### **Project Structure**
Here is an overview of the folder structure:

```
Bookstore
│
├── Bookstore.ApiService
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Interfaces
│   └── Program.cs
│
├── Bookstore.Web
│   ├── Pages
│   ├── wwwroot
│   ├── Models
│   └── Program.cs
│
├── Bookstore.AppHost
│   └── Program.cs
│
├── Bookstore.ServiceDefaults
├── Bookstore.Tests
│   ├── UnitTests
│   ├── IntegrationTests
│   └── BookstoreAppHostTests.cs
│
└── README.md
```

---

### **Components**

#### **1. Bookstore.ApiService**
- **Purpose**: Acts as the backend for the application, providing RESTful endpoints for managing books.
- **Key Files**:
  - **Controllers/BooksController.cs**: Defines the API endpoints for CRUD operations on books.
  - **Services/BookService.cs**: Handles business logic for books and integrates with Redis for caching.
  - **Models/Book.cs**: Represents the data structure for a book.

---

#### **2. Bookstore.Web**
- **Purpose**: The frontend project that interacts with the API and provides a user-friendly interface for managing the book catalog.
- **Key Files**:
  - **Pages**: Contains Razor Pages for rendering UI components.
  - **wwwroot**: Static files such as CSS, JavaScript, and images.

---

#### **3. Bookstore.AppHost**
- **Purpose**: Orchestrates the different services using `.NET Aspire` and configures dependencies like Redis and the API service.
- **Key Files**:
  - **Program.cs**: Sets up the application dependencies and starts the services.

---

#### **4. Bookstore.Tests**
- **Purpose**: Contains unit and integration tests to ensure the functionality and reliability of the application.
- **Key Files**:
  - **UnitTests**: Validates individual components in isolation.
  - **IntegrationTests**: Tests how the components work together.
  - **BookstoreAppHostTests.cs**: Validates the behavior of the application host.

---

### **Quick Start**

#### **Prerequisites**
- Install Docker
- Install .NET SDK
- Clone the repository:
  ```bash
  git clone https://github.com/Shawn-Bellazan-jr/Bookstore.git
  ```

#### **Steps to Run**
1. **Start Redis Container**:
   ```bash
   docker run -d --name cache -p 6379:6379 redis
   ```
2. **Build the Application**:
   ```bash
   dotnet build
   ```
3. **Run the Application**:
   Navigate to the `Bookstore.AppHost` directory and run:
   ```bash
   dotnet run
   ```

4. **Access the Application**:
   - API Service: [http://localhost:7574](http://localhost:7574)
   - Web Frontend: [http://localhost:7276](http://localhost:7276)

---

### **Documentation Articles**

#### **1. How Caching with Redis Works**
Redis is used to cache frequently accessed data like book details. Here’s a quick breakdown:
- When a book is requested, the service first checks Redis.
- If the book exists in the cache, it is returned immediately.
- If not, the service fetches the book from the database, stores it in Redis, and then returns it.

#### **2. Understanding `.NET Aspire` in the Bookstore Project**
`.NET Aspire` simplifies the orchestration of multiple services by providing:
- Dependency injection for Redis and other services.
- Containerized environments for running services in isolation.
- Automatic health checks for service dependencies.

#### **3. Writing Tests for Bookstore**
Testing ensures the application behaves as expected. Example tests include:
- **Unit Tests**: Validate the logic of `BookService`.
- **Integration Tests**: Test the interaction between `BooksController` and `BookService`.

---

### **Contributing**

1. Fork the repository.
2. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature
   ```
3. Make changes and commit them:
   ```bash
   git commit -m "Add your message"
   ```
4. Push to your fork and create a pull request.

