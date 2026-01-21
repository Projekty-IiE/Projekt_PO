using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a SELL operation for a single stock.
    /// Inherits from <see cref="Transaction"/> and provides realized P&amp;L when available.
    /// </summary>
    public class SellTransaction : Transaction
    {
        /// <summary>
        /// Transaction type exposed for UI and logs. Always returns "SELL".
        /// </summary>
        public override string Type => "SELL";

        /// <summary>
        /// Creates a new <see cref="SellTransaction"/>.
        /// </summary>
        /// <param name="stockSymbol">Ticker or symbol of the stock being sold.</param>
        /// <param name="quantity">Number of shares sold (positive integer).</param>
        /// <param name="pricePerShare">Execution price per share.</param>
        /// <param name="realizedPnL">Realized profit or loss for the closed portion (optional).</param>
        /// <param name="time">Optional timestamp for the execution; when null the base class will set a default (usually UtcNow).</param>
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
