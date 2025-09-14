# UniversityManagementSystem

UniversityManagementSystem is a modular, microservices-based application designed to manage university operations, including research topics, educational content, human resource processes, file storage, authentication, and more. Built using ASP.NET Core and modern cloud-native technologies, this project supports high scalability, observability, and extensibility.

## 🧱 Architecture

This system adopts a clean architecture with a layered folder structure per service:

```
├── src/                                  # Source code root
│   ├── Gateway/
│   │   └── Gateway.API/                  # API Gateway
│
│   ├── EduService/
│   │   ├── EduService.API/              # Web API Host
│   │   ├── EduService.Application/      # Use cases, CQRS handlers
│   │   ├── EduService.Domain/           # Domain entities, interfaces
│   │   ├── EduService.Infrastructure/   # EF Core, Repositories, Integrations
│
│   ├── HRMService/
│   │   ├── HRMService.API/
│   │   ├── HRMService.Application/
│   │   ├── HRMService.Domain/
│   │   ├── HRMService.Infrastructure/
│
│   ├── OSSService/
│   │   ├── OSSService.API/
│   │   ├── OSSService.Application/
│   │   ├── OSSService.Domain/
│   │   ├── OSSService.Infrastructure/
│
│   ├── ORDService/
│   │   ├── ORDService.API/
│   │   ├── ORDService.Application/
│   │   ├── ORDService.Domain/
│   │   ├── ORDService.Infrastructure/
│
│   ├── AuthService/
│   │   ├── AuthService.API/
│   │   ├── AuthService.Application/
│   │   ├── AuthService.Domain/
│   │   ├── AuthService.Infrastructure/
|
│   ├── NotificationService/
│   │   ├── NotificationService.API/
│   │   ├── NotificationService.Application/
│   │   ├── NotificationService.Domain/
│   │   ├── NotificationService.Infrastructure/
│
├── Shared/
│   ├── SharedKernel/                    # Common interfaces, base types, DTOs, value objects
│   ├── Shared.Auth/                     # JWT helpers, user context, authorization utilities
├────tests/								 # Unit Test projects
├────docs/								 # Documents, Diagram, etc..
├── docker-compose.yml                   # Docker orchestration
└── README.md                            # Project documentation
```

## 🔧 Tech Stack

- **Backend**: ASP.NET Core, Entity Framework Core  
- **Authentication**: JWT, ASP.NET Core Identity  
- **Database**: SQL Server  
- **API Gateway**: Ocelot  
- **Service Discovery**: Consul  
- **Asynchronous Messaging**: RabbitMQ  
- **Monitoring & Logging**: Elasticsearch + Kibana  
- **API Versioning**: ASP.NET API Versioning  
- **Containerization**: Docker, Docker Compose  
- **CI/CD & Deployment**:  

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)  
- [Docker Desktop](https://www.docker.com/products/docker-desktop)  
- [Consul](https://www.consul.io/)  
- [RabbitMQ](https://www.rabbitmq.com/)  
- [Elasticsearch + Kibana](https://www.elastic.co/)  
- SQL Server running locally or in Docker  

### Installation Steps

Run the install script to add required packages to each service:

```bash
.\install-packages.ps1 -SERVICE_NAME "ORDService"
```

Start all services via Docker Compose:

```bash
docker-compose up -d
```

Access the services via:

- API Gateway: http://localhost:9999  
- Consul UI: http://localhost:8500  
- Kibana: http://localhost:5601  
- RabbitMQ Management: http://localhost:15672  

## 📘 Services Overview

| Service                     | Description                                         |
|-------------------          |-----------------------------------------------------|
| AuthService           9000  | Handles user authentication and authorization       |
| EduService            9001  | Manages educational content and curriculum          |
| HRMService            9002  | Manages university HR processes                     |
| OSSService            9004  | Manages student related info						|
| ORDService            9003  | Manages research topics and scientific projects     |
| Gateway               9999  | API gateway that routes requests to services        |
| NotificationService   9009  | RabbitMQ consumer, Handles notifications            |

---

This project is a proprietary academic project owned by HCMIU.  
Developed by Trọng, Developer at CIS – HCMIU.  
📧 Contact: hdhtrong@hcmiu.edu.vn

