using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Enums;

namespace TradingSimulator.Core.Models
{
    public class Transaction
    {
        public Guid Id { get; }
        public DateTime Time { get; }
        public EnumTransacitonType Type { get; }
        public string StockSymbol { get; }
        public int Quantity { get; }
        public decimal PricePerShare { get; }

        public decimal? RealizedPnL { get; }

        public decimal TotalValue => Quantity * PricePerShare;

        public Transaction(EnumTransacitonType type, string stockSymbol, int quantity, decimal pricePerShare,
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
            Type = type;
            StockSymbol = stockSymbol.ToUpper();
            Quantity = quantity;
            PricePerShare = pricePerShare;
            RealizedPnL = realizedPnL; 
        }

        public override string ToString()
        {
            return $"ID: {Id} \n {Type.ToString().ToUpper()} | {StockSymbol} | QTY: " +
                $" {Quantity} | Price per share: {PricePerShare} | Total: {TotalValue} " +
                $"| PnL: {RealizedPnL} | Time: {Time}";
        }
    }
}