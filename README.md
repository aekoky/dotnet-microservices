This is repository a demonstration of a microservices architecture using .NET Core, RabbitMQ, Docker, and Docker Compose. This project showcases a modular approach to building scalable and maintainable applications.

### 🧱 Architecture Overview

The solution is structured into multiple services, each encapsulating specific business logic and responsibilities:

* **Core**: The foundational layer containing shared logic and utilities.
* **FileService**: Handles file-related operations.
* **RenderingService**: Manages rendering tasks.
* **TemplateService**: Responsible for template management.
* **UserService**: Manages user-related functionalities.

Each service is designed to be independently deployable, promoting a decoupled and scalable system.([Medium][1])

### 🐳 Docker & Docker Compose Integration

The repository includes several Docker-related configurations:

* **Dockerfiles**: Each service has its own `Dockerfile` for containerization.
* **docker-compose.yml**: Defines the multi-container Docker application.
* **docker-compose.development.yml**: Specifically tailored for the development environment.
* **docker-compose.production.yml**: Optimized for the production environment.

These configurations facilitate the orchestration of services, ensuring consistent environments across development and production stages.

### 📦 RabbitMQ Messaging

RabbitMQ is utilized for asynchronous communication between services. This message broker enables:([Medium][2])

* **Decoupling**: Services can communicate without direct dependencies.
* **Scalability**: Easily handle increased loads by scaling services independently.
* **Reliability**: Ensures messages are delivered even if a service is temporarily unavailable.([GitHub][3])

Implementing RabbitMQ in this architecture allows for efficient and reliable inter-service communication.([DevOps.dev][4])

### 🚀 Getting Started

To set up the project locally:

1. **Clone the repository**:

   ```bash
   git clone https://github.com/aekoky/dotnet-microservices.git
   cd dotnet-microservices
   ```



2. **Build and start the services**:

   ```bash
   docker-compose -f docker-compose.yml up --build
   ```



This command will build the images and start the containers as defined in the `docker-compose.yml` file.

3. **Access the services**:

   Once the containers are running, you can access the services via their respective ports as specified in the `docker-compose` configurations.

For a more in-depth understanding and additional configurations, refer to the repository's [README.md](https://github.com/aekoky/dotnet-microservices/blob/main/README.md).

### 🔗 Additional Resources

* [Getting Started with .NET Core, Docker, and RabbitMQ — Part 3](https://medium.com/trimble-maps-engineering-blog/getting-started-with-net-core-docker-and-rabbitmq-part-3-66305dc50ccf): A tutorial that walks through integrating RabbitMQ with .NET Core and Docker.
* [Docker-Compose for Asp.Net Core & RabbitMQ](https://blog.devops.dev/docker-compose-for-asp-net-core-rabbitmq-d532125b1cec): An article demonstrating how to configure a Docker-Compose environment for ASP.NET Core Web API and RabbitMQ.([DevOps.dev][4], [DevOps.dev][5])

These resources provide further insights into building and managing microservices architectures with the mentioned technologies.

If you have any specific questions or need assistance with any part of the setup, feel free to ask!

[1]: https://medium.com/%40boucekdev/build-microservices-with-net-core-and-rabbitmq-step-by-step-bfbc30c73304?utm_source=chatgpt.com "Build Microservices with .NET Core and RabbitMQ (Step-by-Step)"
[2]: https://medium.com/trimble-maps-engineering-blog/getting-started-with-net-core-docker-and-rabbitmq-part-3-66305dc50ccf?utm_source=chatgpt.com "Getting Started with .NET Core, Docker, and RabbitMQ — Part 3"
[3]: https://github.com/rahulsahay19/eShopping?utm_source=chatgpt.com "GitHub - rahulsahay19/eShopping: Clean ..."
[4]: https://blog.devops.dev/rabbitmq-using-net-core-and-docker-a21aee949220?utm_source=chatgpt.com "RabbitMQ using .Net Core and Docker | by José Sousa - DevOps.dev"
[5]: https://blog.devops.dev/docker-compose-for-asp-net-core-rabbitmq-d532125b1cec?utm_source=chatgpt.com "Docker-Compose for Asp.Net Core & RabbitMQ - DevOps.dev"
