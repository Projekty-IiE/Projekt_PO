using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TradingSimulator.Core.Models
{
    public class BuyTransaction : Transaction
    {
        public override string Type => "BUY";

        public BuyTransaction() : base() { }

        public BuyTransaction(
            string stockSymbol,
            int quantity,
            decimal pricePerShare,
            DateTime? time = null)
            : base(stockSymbol, quantity, pricePerShare, time)
        {
        }
    }
}