using System.Collections.Generic;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Interfaces
{
    /// <summary>
    /// Provides market data and operations for stocks in the trading simulator.
    /// </summary>
    public interface IMarketService
    {
        /// <summary>
        /// Gets the list of stocks currently available in the market.
        /// </summary>
        IReadOnlyList<Stock> Stocks { get; }

        /// <summary>
        /// Updates the market data for all stocks.
        /// </summary>
        void UpdateMarket();
    }
}