# Mini Trading Simulator (WPF, C#)

Mini Trading Simulator is a desktop application created as a university group project.  
The application simulates a fictional stock market and allows users to trade stocks,
manage a portfolio, and track profit and loss — all without using real market data
or an internet connection.

The main goal of the project is to demonstrate object-oriented programming,
clean architecture, and GUI development using WPF.
  
## Project Goals

- Simulate a simple stock market with randomly changing prices
- Allow users to buy and sell stocks
- Track portfolio value and profit/loss
- Store transaction history using a local database
- Apply clean architecture and separation of concerns
  
## Project Structure

The solution consists of **two main projects** located in a single repository:

MiniTradingSimulator/  
│  
├── MiniTradingSimulator.sln  
│  
├── src/  
│ ├── TradingSimulator.Core // Business logic + database (EF Core)  
│ └── TradingSimulator.WPF // WPF user interface (MVVM)  
│  
└── README.md  
  
### 🔹 TradingSimulator.Core
- Domain models (Stock, Portfolio, Transaction)
- Market simulation logic
- Portfolio management (buy/sell)
- Entity Framework Core (Code First)
- SQLite database
- No GUI or WPF dependencies
  
### 🔹 TradingSimulator.WPF

- WPF user interface
- MVVM architecture
- ViewModels and Commands
- Data binding and validation
- Communication with Core library
  
## Architecture

- **MVVM** pattern for WPF
- **Separation of concerns** (logic vs UI)
- **Code First** approach with Entity Framework Core
- Repository pattern for data access
  
## Technologies

- C#
- .NET
- WPF
- Entity Framework Core
- SQLite
- Git & GitHub
  
## How to Run

1. Clone the repository
2. Open `MiniTradingSimulator.sln` in Visual Studio
3. Restore NuGet packages
4. Run the `TradingSimulator.WPF` project
  
## Team

🔹[Jakub Gołąb](https://github.com/Agonyy24)  
🔹[Jakub Groblicki](https://github.com/Jablon22)  
🔹[Marcin Górski](https://github.com/margor6)  
🔹[Marcel Florjański](https://github.com/Dertosaid)  
  
## Notes

- The application uses fictional data only
- No real market data or internet connection is required
- The database is generated automatically using EF Core migrations
