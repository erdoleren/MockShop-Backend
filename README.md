\# MockShop - Advanced E-Commerce Backend



A robust, scalable e-commerce backend system built with \*\*.NET 10\*\*, implementing \*\*🧅 Onion Architecture\*\* and \*\*Event-Driven Design\*\*.



\## 🚀 Key Features \& Technologies



\*   \*\*Onion Architecture:\*\* Strict separation of concerns (Domain, Application, Infrastructure, API).

\*   \*\*Event-Driven:\*\* Asynchronous communication using \*\*RabbitMQ\*\*.

\*   \*\*Background Processing:\*\* \*\*Worker Service\*\* for handling order fulfillment logic.

\*   \*\*Security:\*\* \*\*JWT Authentication\*\* \& Role-Based Authorization.

\*   \*\*Database:\*\* \*\*PostgreSQL\*\* with Entity Framework Core (Code First).

\*   \*\*Caching:\*\* \*\*Redis\*\* implementation for high-performance data retrieval.

\*   \*\*External Integration:\*\* Mocked \*\*SOAP Service\*\* adapter for banking transactions.

\*   \*\*DevOps:\*\* Docker Compose support for infrastructure.



\## 🏗️ System Flow

1\.  \*\*API:\*\* Receives order requests, validates via FluentValidation, and secures via JWT.

2\.  \*\*Payment:\*\* Simulates bank approval via SOAP Mock.

3\.  \*\*Database:\*\* Saves order state to PostgreSQL.

4\.  \*\*Message Broker:\*\* Publishes an "OrderPaid" event to RabbitMQ.

5\.  \*\*Worker:\*\* Consumes the event asynchronously and updates order status to "Shipped".



\## ⚙️ How to Run



\### 1. Infrastructure

Run the following command to start Postgres, RabbitMQ, and Redis:

```bash

docker-compose up -d

