using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents an investment portfolio that manages cash balance
    /// and current stock positions, and creates transactions
    /// during buy and sell operations.
    /// </summary>
    public class Portfolio
    {
        decimal balance;
        readonly List<PortfolioItem> items;

        public decimal Balance => balance;
        public IReadOnlyList<PortfolioItem> Items => items;

        public decimal TotalValue => balance + items.Sum(i => i.TotalValue);

        public Portfolio(decimal initialBalance = 10000m)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            balance = initialBalance;
            items = new();
        }
    }
}
