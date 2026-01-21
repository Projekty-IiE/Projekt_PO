using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a BUY operation for a single stock.
    /// Inherits from <see cref="Transaction"/> and only differs by the exposed <see cref="Type"/> value.
    /// </summary>
    public class BuyTransaction : Transaction
    {
        /// <summary>
        /// Transaction type exposed for UI and logs. Always returns "BUY".
        /// </summary>
        public override string Type => "BUY";

        /// <summary>
        /// Creates a new <see cref="BuyTransaction"/>.
        /// </summary>
        /// <param name="stockSymbol">Ticker or symbol of the stock being bought.</param>
        /// <param name="quantity">Number of shares purchased. Should be a positive integer.</param>
        /// <param name="pricePerShare">Execution price per share in account currency.</param>
        /// <param name="time">Optional timestamp for the execution; when null the base class will set a default (usually UtcNow).</param>
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
