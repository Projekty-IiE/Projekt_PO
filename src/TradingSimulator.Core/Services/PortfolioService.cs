using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;


namespace TradingSimulator.Core.Services
{
    /// <summary>
    /// Facade service that exposes portfolio operations to the UI and higher-level layers.
    /// Internally delegates business logic to a <see cref="Portfolio"/> instance and uses an
    /// <see cref="IMarketService"/> to resolve live <see cref="Stock"/> instances.
    /// </summary>
    public class PortfolioService : IPortfolioService
    {
        private readonly Portfolio portfolio;
        private readonly IMarketService marketService;

        /// <summary>
        /// Current cash balance from the underlying portfolio.
        /// </summary>
        public decimal Balance => portfolio.Balance;

        /// <summary>
        /// Cumulative realized profit and loss from the underlying portfolio.
        /// </summary>
        public decimal RealizedPnL => portfolio.RealizedPnL;

        /// <summary>
        /// Total account value (cash + market value of holdings).
        /// </summary>
        public decimal TotalValue => portfolio.TotalValue;

        /// <summary>
        /// Current holdings exposed as a read-only list.
        /// </summary>
        public IReadOnlyList<PortfolioItem> Items => portfolio.Items;

        /// <summary>
        /// Chronological transaction history exposed as a read-only list.
        /// </summary>
        public IReadOnlyList<Transaction> Transactions => portfolio.Transactions;

        /// <summary>
        /// All stocks available from the market service.
        /// </summary>
        public IReadOnlyList<Stock> AllStocks => marketService.Stocks;

        /// <summary>
        /// Creates a new <see cref="PortfolioService"/>.
        /// </summary>
        /// <param name="portfolio">Underlying <see cref="Portfolio"/> instance (required).</param>
        /// <param name="marketService">Market service used to resolve live stock instances (required).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="portfolio"/> or <paramref name="marketService"/> is null.</exception>
        public PortfolioService(Portfolio portfolio, IMarketService marketService)
        {
            this.portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
            this.marketService = marketService ?? throw new ArgumentNullException(nameof(marketService));
        }

        /// <summary>
        /// Buys the specified quantity of a stock identified by <paramref name="symbol"/>.
        /// Resolves the live <see cref="Stock"/> from the market service and delegates to <see cref="Portfolio.BuyStock"/>.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the stock to buy (case-insensitive).</param>
        /// <param name="quantity">Number of shares to buy (must be > 0).</param>
        /// <returns>The created <see cref="Transaction"/> representing the buy.</returns>
        /// <exception cref="ArgumentException">When <paramref name="symbol"/> is empty or the stock cannot be found.</exception>
        /// <exception cref="TradingSimulator.Core.Exceptions.InsufficientFundsException">When the portfolio has insufficient cash.</exception>
        public Transaction Buy(string symbol, int quantity)
        {
            var stock = GetStock(symbol);
            return portfolio.BuyStock(stock, quantity);
        }

        /// <summary>
        /// Sells the specified quantity of a stock identified by <paramref name="symbol"/>.
        /// Resolves the live <see cref="Stock"/> from the market service and delegates to <see cref="Portfolio.SellStock"/>.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the stock to sell (case-insensitive).</param>
        /// <param name="quantity">Number of shares to sell (must be > 0 and <= owned quantity).</param>
        /// <returns>The created <see cref="Transaction"/> representing the sell.</returns>
        /// <exception cref="ArgumentException">When <paramref name="symbol"/> is empty or the stock cannot be found.</exception>
        /// <exception cref="TradingSimulator.Core.Exceptions.InsufficientSharesException">When attempting to sell more shares than owned.</exception>
        public Transaction Sell(string symbol, int quantity)
        {
            var stock = GetStock(symbol);
            return portfolio.SellStock(stock, quantity);
        }

        /// <summary>
        /// Resolves a live <see cref="Stock"/> instance from the market service by symbol.
        /// Throws <see cref="ArgumentException"/> when the symbol is empty or stock is not found.
        /// </summary>
        /// <param name="symbol">Ticker symbol to resolve.</param>
        /// <returns>Resolved <see cref="Stock"/> instance from <see cref="IMarketService.Stocks"/>.</returns>
        private Stock GetStock(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be empty.");

            var stock = marketService.Stocks
                .FirstOrDefault(s => s.Symbol == symbol.ToUpper());

            if (stock == null)
                throw new ArgumentException($"Stock {symbol} not found.");

            return stock;
        }

        /// <summary>
        /// Loads portfolio state and optionally updates market data.
        /// - If <paramref name="marketData"/> is provided, the method will attempt to update the live
        ///   market service stocks' prices and price history to match the saved snapshot (matching by symbol).
        /// - Re-links saved <see cref="PortfolioItem.Stock"/> references to live stock instances when possible.
        /// - Replaces underlying portfolio state (balance, realized PnL, items and transactions).
        /// </summary>
        /// <param name="balance">New cash balance.</param>
        /// <param name="realizedPnL">New realized PnL.</param>
        /// <param name="items">Saved portfolio items (may be null).</param>
        /// <param name="transactions">Saved transaction history (may be null).</param>
        /// <param name="marketData">Saved market snapshot to apply to the live market (may be null).</param>
        public void LoadPortfolio(decimal balance, decimal realizedPnL,
            List<PortfolioItem>? items, List<Transaction>? transactions, List<Stock>? marketData)
        {
            if (marketData != null) // loading whole market snapshot into live market
            {
                foreach (var savedStock in marketData)
                {
                    var liveStock = marketService.Stocks
                        .FirstOrDefault(s => s.Symbol == savedStock.Symbol);

                    if (liveStock != null)
                    {
                        // update live price (this also updates PriceHistory via the Stock property setter)
                        liveStock.Price = savedStock.Price;

                        if (savedStock.PriceHistory != null && savedStock.PriceHistory.Count > 0)
                        {
                            liveStock.PriceHistory.Clear();
                            foreach (var price in savedStock.PriceHistory)
                            {
                                liveStock.PriceHistory.Add(price);
                            }
                        }
                    }
                }
            }

            var safeItems = items ?? new List<PortfolioItem>();

            // Re-link saved portfolio items to live stock instances where possible.
            foreach (var item in safeItems)
            {
                var liveStock = marketService.Stocks
                    .FirstOrDefault(s => s.Symbol == item.Symbol);
                if (liveStock != null)
                {
                    item.Stock = liveStock;
                }
            }

            portfolio.LoadPortfolio(balance, realizedPnL, safeItems, transactions ?? new List<Transaction>());
        }
    }
}