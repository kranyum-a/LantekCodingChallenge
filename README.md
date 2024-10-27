# Lantek Coding Challenge

## 1. Overview

This project implements a .NET console application to retrieve and display cutting machines from Lantek’s API, filtering by cutting technology (2D or 3D). The application meets all requirements by displaying each machine’s name, manufacturer, and cutting technology. The application is built with a focus on modularity, testing, and ease of configuration.

## 2. Application Structure

### 2.1. Project Components

- **Services**: Contains the core logic to interact with Lantek's API using `CuttingMachineService`.
- **Handlers**: Manages command-line interactions through the `CommandHandler` class.
- **Models**: Defines data models for API responses, such as `CuttingMachine`.
- **Interfaces**: Defines interfaces like `ICuttingMachineService` and `ICommandHandler` to facilitate dependency injection and testing.
- **Configuration**: Configurable settings like API URL, username, and password are stored in `appsettings.json` for ease of maintenance.

### 2.2. Key Libraries and Packages

- **Microsoft.Extensions.DependencyInjection**: For dependency injection.
- **Microsoft.Extensions.Logging**: For logging functionality.
- **Serilog**: For structured logging, configured to log to a file.
- **Moq**: Used for mocking dependencies in unit tests.
- **xUnit**: Testing framework to execute unit and integration tests.

---

## 3. Setup and Configuration

### 3.1. Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later.

### 3.2. Configuration File

- **appsettings.json**: Stores configuration details such as API endpoint and authentication credentials. You can update this file without modifying the code.

#### Example Configuration (`appsettings.json`)

```json
{
  "ApiSettings": {
    "Url": "https://app-academy-neu-codechallenge.azurewebsites.net/api/cut",
    "Username": "<username>",
    "Password": "<password>"
  }
}
```
### 3.3. Running the Application

1. **Clone the Repository**:
```bash
   git clone https://github.com/kranyum-a/LantekCodingChallenge.git
   cd LantekCodingChallenge/CuttingMachinesApp
```
2. **Restore Dependencies**:
```bash
dotnet restore
```
3. **Build and Run the Application**:
```
dotnet run
```
4. **Commands Available**:
- `help`: Lists all available commands.
- `all`: Fetches and displays all cutting machines.
- `2d`: Fetches and displays machines with 2D cutting technology.
- `3d`: Fetches and displays machines with 3D cutting technology.
- `exit`: Exits the application.

---

## 4. Testing

### 4.1. Test Structure
- **Unit Tests**: Validate individual components, such as `CuttingMachineService` and `CommandHandler`.
- **Integration Test**: Verify the application's interaction with Lantek's API.

### 4.2. Running the Tests
To run all tests (unit and integration) navigate to the `CuttingMachinesApp/CuttingMachinesApp.Test` directory and execute:
```bash
dotnet test
```
If you wish to separate unit tests and integration tests in the future, you could organize them under separate projects or use a trait-based filtering approach with `xUnit`.
### 4.3 Code Coverage
Each critical functionality has corresponding tests to ensure robust code coverage, including error handling and logging scenarios.
## 5. Technical Questions

The answers are in [Answers to technical questions.md](<Answers to technical questions.md>)

## 6. Feedback
If you have any questions or feedback, please feel free to reach out.