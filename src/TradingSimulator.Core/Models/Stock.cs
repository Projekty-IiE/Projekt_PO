using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a tradable stock with symbol, name, current price and price history.
    /// Provides methods to update the market price while maintaining a price history
    /// and last change for UI coloring.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// Ticker symbol (upper-cased). Example: "NVDA".
        /// </summary>
        public string Symbol { get; set; } // Ticker ex.: "NVDA"

        /// <summary>
        /// Full company name (read-only).
        /// </summary>
        public string Name { get; } // Full name ex.: "NVIDIA Corporation"

        private decimal price;

        /// <summary>
        /// Current market price. Setting the price validates the value (> 0), updates the <see cref="PriceHistory"/>, 
        /// and adjusts <see cref="LastChange"/>.
        /// </summary>
        public decimal Price
        {
            get => price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Price must be greater than 0");

                price = value;
                PriceHistory.Add(value);
            }
        }

        /// <summary>
        /// Latest change in price (delta) used by UI for coloring/updating displays.
        /// </summary>
        public decimal LastChange { get; private set; } //required to color prices after ticks

        /// <summary>
        /// Chronological price history. Uses <see cref="ObservableCollection{T}"/> to support bindings.
        /// </summary>
        public ObservableCollection<decimal> PriceHistory { get; set; }

        /// <summary>
        /// Parameterless constructor required for JSON deserialization.
        /// </summary>
        public Stock() { PriceHistory = new ObservableCollection<decimal>(); } //for json

        /// <summary>
        /// Creates a new stock with an initial price and records it to history.
        /// </summary>
        /// <param name="symbol">Ticker symbol (non-empty).</param>
        /// <param name="name">Full name.</param>
        /// <param name="initialPrice">Initial price (must be > 0).</param>
        public Stock(string symbol, string name, decimal initialPrice)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be empty");

            if (initialPrice <= 0)
                throw new ArgumentException("Initial price must be greater than 0");

            Symbol = symbol.ToUpper();
            Name = name;
            PriceHistory = new ObservableCollection<decimal>();
            Price = initialPrice;
        }

        /// <summary>
        /// Updates the current price by applying a percentage change.
        /// Ensures price never falls below 0.01 (application-imposed floor).
        /// </summary>
        /// <param name="percentageChange">Relative change expressed as decimal (e.g. 0.05 = +5%).</param>
        public void UpdatePrice(decimal percentageChange)
        {
            decimal newPrice = Price + Price * percentageChange;
            LastChange = Math.Max(newPrice, 0.01m) - Price;
            Price = Math.Max(newPrice, 0.01m); // We assume that stock cannot fall below 0.01$
        }

        /// <summary>
        /// Returns a short textual representation: symbol and formatted price.
        /// </summary>
        public override string ToString()
        {
            return $"{Symbol} - {Price:c}";
        }
    }
}
