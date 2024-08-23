# API Gateway

This API Gateway is built using Ocelot and is responsible for routing requests to the appropriate microservices.

## Key Features
- Routes requests to the Authentication and Notification services.
- Can be extended to include authentication, rate limiting, and caching.

## Running the Gateway

To run the API Gateway:

```bash
cd ApiGateway
dotnet run
