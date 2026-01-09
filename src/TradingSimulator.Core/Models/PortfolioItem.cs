using System;

namespace TradingSimulator.Core.Models
{
    public class PortfolioItem
    {
        public Stock Stock { get; }
        public int Quantity { get; private set; }
        public decimal AveragePrice { get; private set; }

        public decimal TotalValue => Stock.Price * Quantity;

        public decimal UnrealizedPnL => (Stock.Price - AveragePrice) * Quantity;

        public PortfolioItem(Stock stock, int quantity, decimal averagePrice)
        {
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0");

            Quantity = quantity;
            AveragePrice = averagePrice;
        }

        public void Add(int amount, decimal priceAtBuy)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");

            decimal totalCost = (Quantity * AveragePrice) + (amount * priceAtBuy);
            Quantity += amount;
            AveragePrice = totalCost / Quantity;
        }

        public void Remove(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");
            if (amount > Quantity) throw new InvalidOperationException("Not enough shares");

            Quantity -= amount;
        }
    }
}