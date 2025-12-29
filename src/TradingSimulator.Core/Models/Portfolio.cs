using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Exceptions;

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

        public void UpdateCash(decimal amount)
        {
            if (balance + amount < 0)
                throw new ArgumentException("Balance cannot be negative.");

            balance += amount;
        }

        public void UpdateHoldings(Stock stock, int quantity)
        {
            if (stock == null)
                throw new ArgumentNullException(nameof(stock));

            var item = items.FirstOrDefault(i => i.Stock.Symbol == stock.Symbol);

            if (item == null)
            {
                if (quantity <= 0)
                    throw new InvalidOperationException("Cannot remove stock that is not owned.");

                items.Add(new PortfolioItem(stock, quantity));
            }
            else
            {
                if (quantity > 0)
                    item.Add(quantity);
                else
                    item.Remove(Math.Abs(quantity));

                if (item.Quantity == 0)
                    items.Remove(item);
            }
        }
        public void BuyStock(Stock stock, int quantity)
        {
            if (stock == null)
                throw new ArgumentNullException(nameof(stock));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            decimal totalCost = stock.Price * quantity;

            if (balance < totalCost)
                throw new InsufficientFundsException(totalCost, balance);

            UpdateCash(-totalCost);
            UpdateHoldings(stock, quantity);
        }

    }
}
