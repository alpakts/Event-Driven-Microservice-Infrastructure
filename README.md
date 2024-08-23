
# Microservices Project with Kafka and API Gateway

## Project Purpose

This project aims to demonstrate a microservices architecture built using .NET , integrating an API Gateway (Ocelot) to manage routing and service communication. The core services include an Authentication Service for user management and a Notification Service that leverages Kafka for event-driven communication. 

The project showcases best practices in microservices design, including:
- Separation of concerns
- Asynchronous communication using Kafka
- API Gateway pattern for service routing

## Technologies Used

### 1. .NET 8
The backbone of the project, providing a powerful, modern framework for building high-performance applications. Each microservice is developed using .NET 6, ensuring scalability, security, and maintainability.

### 2. Ocelot
Ocelot is used as the API Gateway, a lightweight but powerful tool for managing service routing, load balancing, and request aggregation in microservices architecture.

### 3. Kafka
Apache Kafka is employed for asynchronous messaging between services. The Notification Service listens to Kafka topics to trigger actions, such as sending emails, based on specific events (e.g., user registration).

### 4. PostgreSQL
PostgreSQL is used as the database for storing user data in the Authentication Service. It is chosen for its robustness, reliability, and support for complex queries.

### 5. Docker
Docker is used to containerize the services, making it easy to deploy and manage the microservices across different environments. Docker Compose is used for orchestrating multiple containers, such as Kafka, Zookeeper, and PostgreSQL.

### 6. Swagger
Swagger is integrated into the services for API documentation, allowing easy testing and interaction with the microservices during development.

## Development Status

The project is currently under active development. Additional features, improvements, and optimizations are being continuously added. Stay tuned for more updates and enhancements.

For any contributions or suggestions, feel free to reach out or submit a pull request.
