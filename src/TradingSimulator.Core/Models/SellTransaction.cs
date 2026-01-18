using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TradingSimulator.Core.Models
{
    public class SellTransaction : Transaction
    {
        public override string Type => "SELL";

        public SellTransaction() : base() { }

        public SellTransaction(
            string stockSymbol,
            int quantity,
            decimal pricePerShare,
            decimal? realizedPnL = null,
            DateTime? time = null)
            : base(stockSymbol, quantity, pricePerShare, time, realizedPnL)
        {
        }
    }
}