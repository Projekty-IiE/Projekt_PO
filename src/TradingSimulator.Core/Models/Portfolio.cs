using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TradingSimulator.Core.Exceptions;
using TradingSimulator.Core.Interfaces;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents an investment portfolio that manages cash balance,
    /// current stock positions and transaction history.
    /// Provides methods to buy/sell stocks and to load state for persistence.
    /// </summary>
    public class Portfolio
    {
        private decimal balance;
        private readonly List<PortfolioItem> items;

        /// <summary>
        /// Current available cash balance.
        /// </summary>
        public decimal Balance => balance;

        /// <summary>
        /// Cumulative realized profit and loss from closed positions.
        /// </summary>
        public decimal RealizedPnL { get; private set; }

        /// <summary>
        /// Current holdings as a read-only list.
        /// </summary>
        public IReadOnlyList<PortfolioItem> Items => items;

        /// <summary>
        /// Total account value (cash + market value of holdings).
        /// </summary>
        public decimal TotalValue => balance + items.Sum(i => i.TotalValue);
        
        private readonly List<Transaction> _transactions = new List<Transaction>(); // to enable saving and loading to JSON

        /// <summary>
        /// Chronological transaction history (BUY/SELL).
        /// </summary>
        public IReadOnlyList<Transaction> Transactions => _transactions;

        /// <summary>
        /// Creates a new portfolio with an initial cash balance (default 10,000).
        /// </summary>
        /// <param name="initialBalance">Starting cash balance. Must be >= 0.</param>
        public Portfolio(decimal initialBalance = 10000m)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            balance = initialBalance;
            items = new();
        }

        /// <summary>
        /// Executes a buy operation: validates funds, updates cash and holdings,
        /// records and returns a <see cref="BuyTransaction"/>.
        /// </summary>
        /// <param name="stock">Stock to buy (must not be null).</param>
        /// <param name="quantity">Number of shares to buy (must be > 0).</param>
        /// <returns>Created <see cref="BuyTransaction"/> recorded in history.</returns>
        /// <exception cref="InsufficientFundsException">When cash is insufficient for purchase.</exception>
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

        /// <summary>
        /// Executes a sell operation: validates shares, computes realized P&L,
        /// updates holdings and cash, records and returns a <see cref="SellTransaction"/>.
        /// </summary>
        /// <param name="stock">Stock to sell (must not be null).</param>
        /// <param name="quantity">Number of shares to sell (must be > 0).</param>
        /// <returns>Created <see cref="SellTransaction"/> recorded in history.</returns>
        /// <exception cref="InsufficientSharesException">When attempting to sell more shares than owned.</exception>
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
            RealizedPnL += realizedPnL;

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

        /// <summary>
        /// Adjusts cash balance; prevents negative balances.
        /// </summary>
        private void UpdateCash(decimal amount)
        {
            if (balance + amount < 0)
                throw new ArgumentException("Balance cannot be negative.");

            balance += amount;
        }

        /// <summary>
        /// Adds or removes holdings for a given stock.
        /// Positive <paramref name="quantity"/> adds shares, negative removes.
        /// When a holding falls to zero it is removed from the list.
        /// </summary>
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

        /// <summary>
        /// Loads portfolio state (used for persistence). Clears existing holdings/history.
        /// </summary>
        /// <param name="newBalance">New cash balance.</param>
        /// <param name="newPnL">New realized PnL.</param>
        /// <param name="newItems">Enumerable of portfolio items to set (nullable).</param>
        /// <param name="history">Transaction history to set (nullable).</param>
        public void LoadPortfolio(decimal newBalance, decimal newPnL,
            IEnumerable<PortfolioItem> newItems, IEnumerable<Transaction> history)
        {
            balance = newBalance;
            RealizedPnL = newPnL;
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
