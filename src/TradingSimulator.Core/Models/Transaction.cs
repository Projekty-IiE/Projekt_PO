using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Base abstract class for all trade operations (BUY / SELL).
    /// Encapsulates common immutable properties for an execution:
    /// identifier, timestamp, symbol, quantity, price per share and optional realized P&L.
    /// </summary>
    public abstract class Transaction
    {
        /// <summary>
        /// Unique identifier for this transaction.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Execution timestamp. Set by constructor (defaults to local <see cref="DateTime.Now"/> when not provided).
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Upper-cased stock ticker associated with the transaction.
        /// </summary>
        public string StockSymbol { get; }

        /// <summary>
        /// Executed quantity (must be greater than 0).
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Execution price per share (must be greater than 0).
        /// </summary>
        public decimal PricePerShare { get; }

        /// <summary>
        /// Realized profit or loss associated with this transaction when applicable (null for plain buys).
        /// </summary>
        public decimal? RealizedPnL { get; }

        /// <summary>
        /// Total value of the transaction: Quantity * PricePerShare.
        /// </summary>
        public decimal TotalValue => Quantity * PricePerShare;

        /// <summary>
        /// Creates a new transaction instance and validates inputs.
        /// </summary>
        /// <param name="stockSymbol">Ticker symbol (non-empty).</param>
        /// <param name="quantity">Number of shares (must be > 0).</param>
        /// <param name="pricePerShare">Price per share (must be > 0).</param>
        /// <param name="time">Optional execution time; when null <see cref="DateTime.Now"/> is used.</param>
        /// <param name="realizedPnL">Optional realized P&L for transactions that close positions.</param>
        /// <exception cref="ArgumentException">Thrown when symbol is empty or numeric values are invalid.</exception>
        public Transaction(string stockSymbol, int quantity, decimal pricePerShare,
            DateTime? time = null, decimal? realizedPnL = null)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol))
                throw new ArgumentException("Stock symbol cannot be empty");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            if (pricePerShare <= 0)
                throw new ArgumentException("Price must be greater than 0");

            Id = Guid.NewGuid();
            Time = time ?? DateTime.Now;
            StockSymbol = stockSymbol.ToUpper();
            Quantity = quantity;
            PricePerShare = pricePerShare;
            RealizedPnL = realizedPnL;
        }

        /// <summary>
        /// Transaction type exposed for UI (e.g. "BUY" or "SELL").
        /// Concrete implementations must override this.
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Human-readable representation intended for logs and simple debugging.
        /// </summary>
        /// <returns>String containing id, type, symbol, quantity, price, total and PnL.</returns>
        public override string ToString()
        {
            return $"ID: {Id} \n {Type.ToUpper()} | {StockSymbol} | QTY: " +
                $" {Quantity} | Price per share: {PricePerShare} | Total: {TotalValue} " +
                $"| PnL: {RealizedPnL} | Time: {Time}";
        }
    }
}