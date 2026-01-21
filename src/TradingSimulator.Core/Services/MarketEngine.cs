using System;
using System.Collections.Generic;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Services
{
    /// <summary>
    /// Simple market engine that simulates price movement for a set of stocks.
    /// The engine uses a pseudo-random percentage change per tick and applies it to each stock.
    /// </summary>
    public class MarketEngine : IMarketService
    {
        // Random generator used to produce percentage changes per tick.
        // Note: <see cref="System.Random"/> is not thread-safe; use a thread-safe alternative if accessed concurrently.
        private readonly Random random = new();

        // Backing collection of stocks managed by the engine.
        private readonly List<Stock> stocks;

        /// <summary>
        /// Gets the read-only list of stocks managed by the engine.
        /// Consumers may enumerate or bind to this collection to display available instruments.
        /// </summary>
        public IReadOnlyList<Stock> Stocks => stocks;

        /// <summary>
        /// Creates a new <see cref="MarketEngine"/> that will manage the provided stocks.
        /// </summary>
        /// <param name="stocks">Enumerable of stocks to be managed. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stocks"/> is null.</exception>
        public MarketEngine(IEnumerable<Stock> stocks)
        {
            if (stocks == null) { throw new ArgumentNullException(nameof(stocks)); }

            this.stocks = new List<Stock>(stocks);
        }

        /// <summary>
        /// Generates a random percentage change for a single tick.
        /// The returned value is in decimal form and represents a relative change (e.g. 0.01 = +1%).
        /// Range: -0.02m .. +0.02m (i.e. -2% .. +2%).
        /// </summary>
        /// <returns>Decimal percentage change to apply to a stock price.</returns>
        private decimal GetChange()
        {
            // Change from -2% to +2%
            return (decimal)(random.NextDouble() * 0.04 - 0.02);
        }

        /// <summary>
        /// Advances the market by one tick: computes a percentage change for each stock
        /// and updates its price by calling <see cref="Stock.UpdatePrice(decimal)"/>.
        /// </summary>
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