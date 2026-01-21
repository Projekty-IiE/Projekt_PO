using System;
using System.Collections.Generic;

namespace TradingSimulator.Core.Models
{
    /// <summary>
    /// Represents a snapshot of a trading session's runtime state.
    /// Contains cash balance, cumulative realized P&L, current portfolio items,
    /// executed transactions and optional market data (price history for tracked stocks).
    /// This object is suitable for serialization when persisting or restoring a session.
    /// </summary>
    public class SessionState
    {
        /// <summary>
        /// Initializes a new <see cref="SessionState"/> with empty collections.
        /// Keeping collections initialized avoids null checks when reading or mutating state.
        /// </summary>
        public SessionState()
        {
            Balance = 0m;
            RealizedPnL = 0m;
            Items = new List<PortfolioItem>();
            Transactions = new List<Transaction>();
            MarketData = new List<Stock>();
        }

        /// <summary>
        /// Current available cash balance (currency units).
        /// Update this value when deposits, withdrawals or trade executions occur.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Cumulative realized profit and loss from closed trades.
        /// Does not include unrealized P&amp;L stored on individual <see cref="PortfolioItem"/> instances.
        /// </summary>
        public decimal RealizedPnL { get; set; }

        /// <summary>
        /// Current portfolio holdings. Each <see cref="PortfolioItem"/> represents a position
        /// (symbol, quantity, average price, unrealized PnL).
        /// Initialized to an empty list to avoid null reference exceptions.
        /// </summary>
        public List<PortfolioItem> Items { get; set; }

        /// <summary>
        /// Chronological list of executed transactions (BUY / SELL).
        /// Use to reconstruct activity history, audit or replay session operations.
        /// </summary>
        public List<Transaction> Transactions { get; set; }

        /// <summary>
        /// Market data / price history for tracked stocks (including stocks not currently owned).
        /// Used to compute unrealized PnL, charting or backtesting without requiring ownership.
        /// </summary>
        public List<Stock> MarketData { get; set; }
    }
}
