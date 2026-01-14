using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly Portfolio portfolio;
        private readonly IMarketService marketService;
        public decimal Balance => portfolio.Balance;
        public decimal TotalValue => portfolio.TotalValue;
        public IReadOnlyList<PortfolioItem> Items => portfolio.Items;

        public PortfolioService(Portfolio portfolio, IMarketService marketService)
        {
            this.portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
            this.marketService = marketService ?? throw new ArgumentNullException(nameof(marketService));
        }

        public Transaction Buy(string symbol, int quantity)
        {
            var stock = GetStock(symbol);
            portfolio.BuyStock(stock, quantity);

            return new BuyTransaction(stock.Symbol,
                quantity,
                stock.Price
                );
        }

        public Transaction Sell(string symbol, int quantity)
        {
            var stock = GetStock(symbol);

            var item = Items.FirstOrDefault(i => i.Stock.Symbol == symbol.ToUpper());

            decimal avgBuyPrice = item?.AveragePrice ?? 0;

            portfolio.SellStock(stock, quantity);

            decimal realizedPnL = (stock.Price - avgBuyPrice) * quantity;

            return new SellTransaction(stock.Symbol,
                quantity,
                stock.Price,
                realizedPnL);   
        }

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
    }
}