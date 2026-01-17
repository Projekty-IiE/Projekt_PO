using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Interfaces
{
    public interface IPortfolioService
    {
        decimal Balance { get; }
        decimal RealizedPnL { get; }
        decimal TotalValue { get; }
        IReadOnlyList<PortfolioItem> Items { get; }
        IReadOnlyList<Transaction> Transactions { get; }
        IReadOnlyList<Stock> AllStocks { get; }
        Transaction Buy(string symbol, int quantity);
        Transaction Sell(string symbol, int quantity);
        void LoadPortfolio(decimal balance, decimal realizedPnL, 
            List<PortfolioItem>? items, List<Transaction>? transactions, List<Stock>? marketData);
    }
}
