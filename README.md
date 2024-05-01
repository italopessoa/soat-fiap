# FIAP Tech Challenge - ByteMeBurger API

## Description
This repository contains the source code for the ByteMeBurger API, part of the FIAP Tech Challenge. The API is designed to manage a burger restaurant's operations, including order placement, customer registration, and product management. The recent updates introduce a new endpoint for customer registration and enhance various aspects of the application, such as error handling, data models, and service configurations.

## Getting Started

### Prerequisites
- Docker
- .NET SDK
- Optionally, an IDE such as Visual Studio or VSCode

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/soat-fiap/FIAP.TechChallenge.ByteMeBurger.git
   cd FIAP.TechChallenge.ByteMeBurger
    ```

2. Start the services using Docker:

   ```bash
    docker-compose up -d
   ```
3. Testing
To run the automated tests included in the repository, use the following command:
   ```bash
    dotnet test
   ```

This command will execute all tests defined in the test projects and provide a summary of the test results.
