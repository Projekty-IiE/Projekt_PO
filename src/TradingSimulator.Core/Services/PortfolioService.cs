using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Services
{
    public class PortfolioService
    {
        private readonly Portfolio portfolio;
        private readonly MarketEngine marketEngine;

        public PortfolioService(Portfolio portfolio, MarketEngine marketEngine)
        {
            this.portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
            this.marketEngine = marketEngine ?? throw new ArgumentNullException(nameof(marketEngine));
        }

        public Transaction Buy(string symbol, int quantity)
        {
            var stock = GetStock(symbol);
            return portfolio.BuyStock(stock, quantity);
        }

        public Transaction Sell(string symbol, int quantity)
        {
            var stock = GetStock(symbol);
            return portfolio.SellStock(stock, quantity);
        }

        private Stock GetStock(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be empty.");

            var stock = marketEngine.Stocks
                .FirstOrDefault(s => s.Symbol == symbol.ToUpper());

            if (stock == null)
                throw new ArgumentException($"Stock {symbol} not found.");

            return stock;
        }
    }

}
