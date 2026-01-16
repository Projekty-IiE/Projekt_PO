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
        public List<PortfolioItem>? Items { get; set; }
        public List<Transaction>? Transactions { get; set; }
    }
}
