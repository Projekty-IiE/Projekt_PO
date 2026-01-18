using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    public class SessionState
    {
        public decimal Balance {  get; set; }
        public decimal RealizedPnL { get; set; }
        public List<PortfolioItem>? Items { get; set; }
        public List<Transaction>? Transactions { get; set; }
        public List<Stock>? MarketData { get; set; } //to save pricehistory of stocks we dont own 

    }
}
