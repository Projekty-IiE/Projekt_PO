using System;
using System.Collections.Generic;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Services
{
    public class MarketEngine : IMarketService
    {
        private readonly Random random = new();
        private readonly List<Stock> stocks;

        public IReadOnlyList<Stock> Stocks => stocks;

        public MarketEngine(IEnumerable<Stock> stocks)
        {
            if (stocks == null) { throw new ArgumentNullException(nameof(stocks)); }

            this.stocks = new List<Stock>(stocks);
        }

        private decimal GetChange()
        {
            // Zmiana od -2% do +2%
            return (decimal)(random.NextDouble() * 0.04 - 0.02);
        }

        public void UpdateMarket()
        {
            foreach (var stock in stocks)
            {

                decimal changePercent = GetChange();

                stock.UpdatePrice(changePercent);
            }
        }
    }
}