using System;
using System.Collections.Generic;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a certain Stock. Class methods allow to manipulate
    /// price of the stock. Contains price history as a list.
    /// </summary>
    public class Stock
    {
        public string Symbol { get; } // Ticker ex.: "NVDA"
        public string Name { get; } // Full name ex.: "NVIDIA Corporation"

        private decimal price;
        public decimal Price
        {
            get => price;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be greater than 0");

                price = value;
                PriceHistory.Add(value);
            }
        }

        public decimal LastChange { get; private set; } //required to color prices after ticks

        public List<decimal> PriceHistory { get; }

        public Stock(string symbol, string name, decimal initialPrice)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be empty");

            if (initialPrice <= 0)
                throw new ArgumentException("Initial price must be greater than 0");

            Symbol = symbol.ToUpper();
            Name = name;
            PriceHistory = new List<decimal>();

            Price = initialPrice;
        }

        public void UpdatePrice(decimal percentageChange)
        {
            decimal newPrice = Price + Price * percentageChange;
            LastChange = Math.Max(newPrice, 0.01m) - Price;
            Price = Math.Max(newPrice, 0.01m); // We assume that stock cannot fall below 0.01$
        }

        public override string ToString()
        {
            return $"{Symbol} - {Price:c}";
        }
    }
}
