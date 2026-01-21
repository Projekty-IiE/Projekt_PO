using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a portfolio item: a holding of a single stock with quantity,
    /// average purchase price and derived values (total value, unrealized PnL).
    /// </summary>
    public class PortfolioItem
    {
        /// <summary>
        /// Stock symbol (kept in sync with <see cref="Stock.Symbol"/>).
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Reference to the <see cref="Stock"/> object for current market price and metadata.
        /// </summary>
        public Stock Stock { get; set; }

        /// <summary>
        /// Held quantity. Mutated via <see cref="Add"/> and <see cref="Remove"/>.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Current total market value of this holding: <c>Stock.Price * Quantity</c>.
        /// </summary>
        public decimal TotalValue => Stock.Price * Quantity;

        /// <summary>
        /// Weighted average purchase price for the held shares.
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// Unrealized profit or loss calculated using the current market price.
        /// </summary>
        public decimal UnrealizedPnL => TotalValue - (AveragePrice * Quantity);

        /// <summary>
        /// Creates a new portfolio item for the specified stock.
        /// </summary>
        /// <param name="stock">Stock instance (must not be null).</param>
        /// <param name="quantity">Initial quantity (must be > 0).</param>
        /// <param name="purchasePrice">Purchase price to set as initial average price.</param>
        public PortfolioItem(Stock stock, int quantity, decimal purchasePrice)
        {
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));
            Symbol = stock.Symbol;
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            Quantity = quantity;
            AveragePrice = purchasePrice;
        }

        /// <summary>
        /// Adds shares to this holding and recalculates the average purchase price.
        /// </summary>
        /// <param name="amount">Number of shares to add (must be > 0).</param>
        /// <param name="purchasePrice">Execution price used to update the weighted average.</param>
        public void Add(int amount, decimal purchasePrice)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            Quantity += amount;
            AveragePrice = (AveragePrice * (Quantity - amount) + purchasePrice * amount) / Quantity;
        }

        /// <summary>
        /// Removes shares from this holding.
        /// </summary>
        /// <param name="amount">Number of shares to remove (must be > 0 and <= current quantity).</param>
        /// <exception cref="InvalidOperationException">Thrown when attempting to remove more shares than held.</exception>
        public void Remove(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            if (amount > Quantity)
                throw new InvalidOperationException("Not enough shares to remove");

            Quantity -= amount;
        }

        /// <summary>
        /// Human-readable representation for debugging and UI lists.
        /// </summary>
        public override string ToString()
        {
            return $"{Stock.Symbol} | Qty: {Quantity} | Value: {TotalValue:C}";
        }
    }
}