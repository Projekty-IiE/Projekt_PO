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
    public class PortfolioService : IPortfolioService
    {
        private readonly Portfolio portfolio;
        private readonly IMarketService marketService;
        public decimal Balance => portfolio.Balance;
        public decimal RealizedPnL => portfolio.RealizedPnL;
        public decimal TotalValue => portfolio.TotalValue;
        public IReadOnlyList<PortfolioItem> Items => portfolio.Items;

        public IReadOnlyList<Transaction> Transactions => portfolio.Transactions;

        public IReadOnlyList<Stock> AllStocks => marketService.Stocks;

        public PortfolioService(Portfolio portfolio, IMarketService marketService)
        {
            this.portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
            this.marketService = marketService ?? throw new ArgumentNullException(nameof(marketService));
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

            var stock = marketService.Stocks
                .FirstOrDefault(s => s.Symbol == symbol.ToUpper());

            if (stock == null)
                throw new ArgumentException($"Stock {symbol} not found.");

            return stock;
        }


        public void LoadPortfolio(decimal balance, decimal realizedPnL,
    List<PortfolioItem>? items, List<Transaction>? transactions, List<Stock>? marketData)
        {
            if (marketData != null) //loading whole market
            {
                foreach (var savedStock in marketData)
                {
                    var liveStock = marketService.Stocks
                        .FirstOrDefault(s => s.Symbol == savedStock.Symbol);

                    if (liveStock != null)
                    {
                        liveStock.Price = savedStock.Price;

                        if (savedStock.PriceHistory != null && savedStock.PriceHistory.Count > 0)
                        {
                            liveStock.PriceHistory.Clear();
                            //liveStock.PriceHistory.AddRange(savedStock.PriceHistory);
                            foreach (var price in savedStock.PriceHistory)
                            {
                                liveStock.PriceHistory.Add(price);
                            }
                        }
                    }
                }
            }

            var safeItems = items ?? new List<PortfolioItem>();

            foreach (var item in safeItems) //loading owned stocks
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