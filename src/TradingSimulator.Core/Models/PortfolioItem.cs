using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a portfolio item. Portfolio consists of given portfolio items. 
    /// Portfolio item stores a given stock info (Stock object and Quantity of this stock).
    /// </summary>
    public class PortfolioItem
    {
        public string Symbol { get; set; }
        public Stock Stock { get; set; }
        public int Quantity { get; private set; }
        public decimal TotalValue => Stock.Price * Quantity;
        public decimal AveragePrice { get; private set; }
        public decimal UnrealizedPnL => TotalValue - (AveragePrice * Quantity);

        public PortfolioItem(Stock stock, int quantity, decimal purchasePrice)
        {
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));
            Symbol = stock.Symbol;
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            Quantity = quantity;
            AveragePrice = purchasePrice; 
        }

        public void Add(int amount, decimal purchasePrice)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            Quantity += amount;
            AveragePrice = (AveragePrice * (Quantity - amount) + purchasePrice * amount) / Quantity;
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