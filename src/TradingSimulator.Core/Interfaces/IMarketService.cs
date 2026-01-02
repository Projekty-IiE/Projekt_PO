using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Interfaces
{
    public interface IMarketService
    {
        IReadOnlyList<Stock> Stocks { get; }
        void Tick();
    }
}