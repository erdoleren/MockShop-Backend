#  MockShop - Scalable E-Commerce Backend

A robust, scalable e-commerce backend system built with **.NET 10**, implementing **Onion Architecture** and **Event-Driven Design**.

This project demonstrates the integration of modern backend technologies to handle high-load scenarios, asynchronous processing, and third-party service integrations.

## 🚀 Key Features

*   **Onion Architecture:** Strict separation of concerns (Domain, Application, Infrastructure, API).
*   **Event-Driven Architecture:** Asynchronous communication between microservices using **RabbitMQ**.
*   **Background Processing:** Dedicated **Worker Service (Windows Service)** for handling order fulfillment logic without blocking the API.
*   **Security:** **JWT Authentication** & Role-Based Authorization.
*   **Database:** **PostgreSQL** with Entity Framework Core (Code First & Snake Case Naming Convention).
*   **Performance:** **Redis** implementation for high-performance data caching.
*   **Integration:** Mocked **SOAP Service** adapter for simulating banking transactions.
*   **DevOps:** **Docker Compose** support for instant infrastructure setup.

## 🛠️ Tech Stack

*   **Framework:** .NET 10 (Core)
*   **Language:** C#
*   **Database:** PostgreSQL
*   **Message Broker:** RabbitMQ
*   **Caching:** Redis
*   **ORM:** Entity Framework Core
*   **Documentation:** Swagger / OpenAPI
*   **Containerization:** Docker

## 🏗️ System Architecture & Flow

1.  **Client:** Sends an order request via REST API (secured with JWT).
2.  **API Layer:**
    *   Validates the request.
    *   Checks cache (Redis) for product availability.
    *   Calls the **Mock SOAP Payment Service** to verify funds.
    *   Saves the order to **PostgreSQL** with status "Paid".
    *   Publishes an `OrderCreated` event to **RabbitMQ**.
3.  **Messaging Layer:** RabbitMQ queues the message, decoupling the user interface from heavy processing.
4.  **Background Worker:**
    *   Consumes the event asynchronously.
    *   Simulates shipping logic (creating labels, notifying logistics).
    *   Updates the order status to "Shipped" in the database using a separate **Service Scope**.

## ⚙️ How to Run

### 1. Prerequisites
*   .NET 10 SDK
*   Docker Desktop

### 2. Infrastructure Setup (Docker)
* Run the following command in the project root to start PostgreSQL, RabbitMQ, and Redis:
```
docker-compose up -d
```
### 3. Configuration
* Ensure your appsettings.json files (in MockShop.API and MockShop.BackgroundWorker) point to the Docker instances.
* Database: Host=localhost;Database=MockShopDb;Username=postgres;Password=mysecretpassword
* RabbitMQ: localhost
* Redis: localhost:6379
### 4. Running the Application
Since this solution contains a Web API and a Background Worker, you need to run both:

**Option A: Visual Studio**
* Right-click Solution -> Set Startup Projects.
* Select Multiple startup projects.
* Set both MockShop.API and MockShop.BackgroundWorker to Start.
* Press F5.
**Option B: Terminal**
* Open two terminal windows:
**Terminal 1:**
```
dotnet run --project MockShop.API
```
**Terminal 2:**
```
dotnet run --project MockShop.BackgroundWorker
```
## 🧪 TESTING THE API
* Go to Swagger UI: http://localhost:5003/swagger
* Login: Use the /api/Auth/login endpoint to get a JWT Token (Default user: eren@work.com / 12345).
* Authorize: Click the lock icon and enter the token (e.g., eyJhbG...).
* Create Order: POST a request to /api/Orders.
* Observe: Check the Worker Console logs to see the RabbitMQ message consumption and database update.
## 📜 LICENSE
* This project is open-source and available under the MIT License.