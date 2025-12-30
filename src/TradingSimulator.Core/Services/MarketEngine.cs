using System;
using System.Collections.Generic;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Services
{
    /// <summary>
    /// Simulates market fluidity by periodically changing stock prices.
    /// </summary>
    
    public class MarketEngine
    {
        private readonly Random random = new();
        private readonly List<Stock> stocks;

        public IReadOnlyList<Stock> Stocks => stocks;

        public MarketEngine(IEnumerable<Stock> stocks)
        {
            if(stocks == null) { throw new ArgumentNullException(nameof(stocks)); }

            this.stocks= new List<Stock>(stocks);
        }

        private decimal GetChange()
        {
            return (decimal)(random.NextDouble() *0.1-0.05); //from -0.05 to 0.05
        }
        
        public void Tick()
        {
            foreach (var stock in stocks)
            {
                decimal change = GetChange();
                stock.UpdatePrice(change);
            }
        }
    }
}
