# QuestEngine

QuestEngine is a .NET Core solution for managing quests and tracking player progress. The solution contains two main projects:

- **QuestEngine.Tests**: Contains unit tests for the QuestEngine application.
- **QuestEngine.WebAPI**: Provides API endpoints for the application.

The root of this repository contains the solution file `QuestEngine.sln`.

## Getting Started

Follow these instructions to set up the QuestEngine solution on your local machine for development and testing purposes.

### Prerequisites

- .NET Core SDK 8.0 or later
- A code editor or IDE, such as Visual Studio or Visual Studio Code

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/JonathanCastilleja/PlayStudiosChallenge.git
   cd PlayStudiosChallenge

2. **Open the solution**:
   Open the `QuestEngine.sln` file in your preferred cide editor or IDE

3. **Restore dependencies and buld the solution**:
   In the terminal or command prompt, navigate to the root of the repository and run:
   ```bash
   dotnet restore
   dotnet build

5. **Run the API project**:
   Navigate to the QuestEngine.WebAPI folder and start the Web API:
   ```bash
   cd QuestEngine.WebAPI
   dotnet run
   ```
   Once the application starts, check the console output to see which port it is running on.
   By default, it may be on port 5250. Open this link to see [Swagger GUI](http://localhost:5250/swagger).

   Adjust the port number in the URL according to the console output if it's different.

7. **Run the API tests**:
   Navigate to the QuestEngine.Tests folder and run the unit tests:
   ```bash
   cd QuestEngine.Tests
   dotnet test


