# Mini Trading Simulator (WPF, C#)

Mini Trading Simulator is a desktop application created as a university group project.  
The application simulates a fictional stock market and allows users to trade stocks,
manage a portfolio, and track profit and loss - all without using real market data
or an internet connection.

The main goal of the project is to demonstrate object-oriented programming,
clean architecture, and GUI development using WPF.
  
## Features

- Live market simulation
- Portfolio management
- Real-time balance and portfolio value
- Unrealized PnL per position
- Transaction history
- Price chart
- JSON save/load - full session persistence
- Sound effects
- Database - stores all executed transactions (Optional Feature kept on designated branch)

## Project Structure

The solution consists of **two main projects** located in a single repository:

### 🔹 TradingSimulator.Core
- Domain models (Stock, Portfolio, Transaction)
- Market simulation logic
- Portfolio management (buy/sell)
- Entity Framework Core (Code First, Optional Feature)
- SQLite database (Optional Feature)
- No GUI or WPF dependencies
  
### 🔹 TradingSimulator.WPF

- WPF user interface
- MVVM architecture
- Data binding and validation
- Communication with Core library
  
## Technologies

- C# / .NET 8
- WPF
- MVVM
- CommunityToolkit.MVVM
- LiveCharts
- SQLite
- JSON serialization

## Dashboard Preview

<img width="2879" height="1663" alt="image" src="https://github.com/user-attachments/assets/e854c533-4b8a-432d-a1ba-4f98efee0259" />

  
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
