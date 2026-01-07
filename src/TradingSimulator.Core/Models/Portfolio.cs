using System;
using System.Collections.Generic;
using System.Linq;
using TradingSimulator.Core.Enums;
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

            return new Transaction(
                EnumTransacitonType.Buy,
                stock.Symbol,
                quantity,
                stock.Price
            );
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

            decimal totalValue = stock.Price * quantity;

            UpdateHoldings(stock, -quantity);
            UpdateCash(totalValue);

            return new Transaction(
                EnumTransacitonType.Sell,
                stock.Symbol,
                quantity,
                stock.Price
            );
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
    }
}
