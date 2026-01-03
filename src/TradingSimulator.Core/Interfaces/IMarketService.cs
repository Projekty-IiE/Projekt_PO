using System.Collections.Generic;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Interfaces
{
    public interface IMarketService
    {
        IReadOnlyList<Stock> Stocks { get; }

        void UpdateMarket();
    }
}