using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TradingSimulator.Core.Exceptions;
using TradingSimulator.Core.Interfaces;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents an investment portfolio that manages cash balance
    /// and current stock positions.
    /// </summary>
    public class Portfolio
    {
        private decimal balance;
        private readonly List<PortfolioItem> items;

        public decimal Balance => balance;
        public IReadOnlyList<PortfolioItem> Items => items;

        public decimal TotalValue => balance + items.Sum(i => i.TotalValue);
        
        private readonly List<Transaction> _transactions = new List<Transaction>(); //to enable saving and loading to JSON
        public IReadOnlyList<Transaction> Transactions => _transactions;
        public Portfolio(decimal initialBalance = 10000m)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            balance = initialBalance;
            items = new();
        }

        public Transaction BuyStock(Stock stock, int quantity)
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

            Transaction transact = new BuyTransaction(
                stock.Symbol,
                quantity,
                stock.Price, 
                DateTime.Now);

            _transactions.Add(transact); //saving to history

            return transact;
        }

        public Transaction SellStock(Stock stock, int quantity)
        {
            if (stock == null)
                throw new ArgumentNullException(nameof(stock));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            var item = items.FirstOrDefault(i => i.Stock.Symbol == stock.Symbol);

            if (item == null)
                throw new InsufficientSharesException(quantity, 0);

            if (item.Quantity < quantity)
                throw new InsufficientSharesException(quantity, item.Quantity);

            decimal realizedPnL = (stock.Price - item.AveragePrice) * quantity;
            decimal totalValue = stock.Price * quantity;

            UpdateHoldings(stock, -quantity);
            UpdateCash(totalValue);
            
            Transaction transact = new SellTransaction(
                stock.Symbol,
                quantity,
                stock.Price,
                realizedPnL,
                DateTime.Now);

            _transactions.Add(transact);

            return transact;
        }

        // ===== PRIVATE HELPERS =====

        private void UpdateCash(decimal amount)
        {
            if (balance + amount < 0)
                throw new ArgumentException("Balance cannot be negative.");

            balance += amount;
        }

        private void UpdateHoldings(Stock stock, int quantity)
        {
            var item = items.FirstOrDefault(i => i.Stock.Symbol == stock.Symbol);

            if (item == null)
            {
                if (quantity <= 0)
                    throw new InvalidOperationException("Cannot remove stock that is not owned.");

                items.Add(new PortfolioItem(stock, quantity, stock.Price));
            }
            else
            {
                if (quantity > 0)
                    item.Add(quantity, stock.Price);
                else
                    item.Remove(Math.Abs(quantity));

                if (item.Quantity == 0)
                    items.Remove(item);
            }
        }
        public void LoadPortfolio(decimal newBalance, 
            IEnumerable<PortfolioItem> newItems, IEnumerable<Transaction> history)
        {
            balance = newBalance;
            items.Clear();
            if(newItems!=null)
            {
                items.AddRange(newItems);
            }
            _transactions.Clear();
            if(history!=null)
            {
                _transactions.AddRange(history);
            }
        }
    }
}
