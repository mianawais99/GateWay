# GateWay

This project is an ASP.NET Core Web API that utilizes YARP (Yet Another Reverse Proxy) to act as a gateway for routing requests to various backend applications. By leveraging YARP, this solution ensures efficient and flexible request handling, improving the scalability and maintainability of the overall architecture.

## Features

- **Reverse Proxy**: Routes requests to different backend services.
- **Load Balancing**: Distributes incoming requests across multiple instances of backend services.
- **Configurable Routing**: Easily configure routes and destinations via appsettings.json or other configuration sources.
- **Extensibility**: Easily extend the gateway with custom middleware and routing logic.

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/mianawais99/GateWay.git
    cd GateWay
    ```

2. Restore the dependencies:

    ```bash
    dotnet restore
    ```

3. Build the project:

    ```bash
    dotnet build
    ```

### Configuration

Configure your routes and backend destinations in the `appsettings.json` file:

```json
{
  "ReverseProxy": {
    "Routes": {
      "route1": {
        "ClusterId": "cluster1",
        "Match": {
          "Path": "/api/service1/{**catch-all}"
        }
      },
      "route2": {
        "ClusterId": "cluster2",
        "Match": {
          "Path": "/api/service2/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "cluster1": {
        "Destinations": {
          "destination1": {
            "Address": "https://localhost:5001/"
          }
        }
      },
      "cluster2": {
        "Destinations": {
          "destination2": {
            "Address": "https://localhost:5002/"
          }
        }
      }
    }
  }
}
