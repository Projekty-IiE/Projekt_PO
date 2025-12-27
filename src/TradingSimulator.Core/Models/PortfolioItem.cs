using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    public class PortfolioItem
    {
        public Stock Stock { get; }
        public int Quantity { get; private set; }

        public decimal TotalValue => Stock.Price * Quantity;

        public PortfolioItem(Stock stock, int quantity)
        {
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            Quantity = quantity;
        }

        public void Add(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            Quantity += amount;
        }

        public void Remove(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            if (amount > Quantity)
                throw new InvalidOperationException("Not enough shares to remove");

            Quantity -= amount;
        }

        public override string ToString()
        {
            return $"{Stock.Symbol} | Qty: {Quantity} | Value: {TotalValue:C}";
        }
    }
}

