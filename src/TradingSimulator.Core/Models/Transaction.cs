using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Base abstract class for all trade operations (BUY / SELL).
    /// </summary>
    public abstract class Transaction
    {
        public Guid Id { get; private set; }
        public DateTime Time { get; private set; }
        public string StockSymbol { get; private set; } = null!;
        public int Quantity { get; private set; }
        public decimal PricePerShare { get; private set; }
        public decimal? RealizedPnL { get; private set; }

        public decimal TotalValue => Quantity * PricePerShare;

        protected Transaction() { }

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
        /// Transaction type exposed for UI.
        /// </summary>
        public abstract string Type { get; }

        public override string ToString()
        {
            return $"ID: {Id} \n {Type?.ToUpper()} | {StockSymbol} | QTY: " +
                $" {Quantity} | Price per share: {PricePerShare} | Total: {TotalValue} " +
                $"| PnL: {RealizedPnL} | Time: {Time}";
        }
    }
}