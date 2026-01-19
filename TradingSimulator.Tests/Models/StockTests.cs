using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;
using Xunit;

namespace TradingSimulator.Tests.Models
{
    public class StockTests
    {
        [Fact]
        public void Constructor_CreatesStock_WithValidInitialData()
        {
            var stock = new Stock("AAPL", "Apple Inc.", 150m);

            Assert.Equal("AAPL", stock.Symbol);
            Assert.Equal("Apple Inc.", stock.Name);
            Assert.Equal(150m, stock.Price);
            Assert.Single(stock.PriceHistory);
        }

        [Fact]
        public void Constructor_Throws_WhenSymbolIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                new Stock("", "Apple", 150m));
        }

        [Fact]
        public void Constructor_Throws_WhenInitialPriceIsInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
                new Stock("AAPL", "Apple", 0m));
        }

        [Fact]
        public void UpdatePrice_UpdatesPriceAndLastChange()
        {
            var stock = new Stock("AAPL", "Apple", 100m);

            stock.UpdatePrice(0.10m);

            Assert.Equal(110m, stock.Price);
            Assert.Equal(10m, stock.LastChange);
        }

        [Fact]
        public void UpdatePrice_AddsNewEntryToPriceHistory()
        {
            var stock = new Stock("AAPL", "Apple", 100m);

            stock.UpdatePrice(0.05m);

            Assert.Equal(2, stock.PriceHistory.Count);
        }

        [Fact]
        public void UpdatePrice_DoesNotAllowPriceBelowMinimum()
        {
            var stock = new Stock("AAPL", "Apple", 1m);

            stock.UpdatePrice(-0.99m);

            Assert.Equal(0.01m, stock.Price);
            Assert.True(stock.LastChange < 0);
        }

        [Fact]
        public void Price_Setter_Throws_WhenPriceIsInvalid()
        {
            var stock = new Stock("AAPL", "Apple", 100m);

            Assert.Throws<ArgumentException>(() => stock.Price = 0m);
        }
    }
}
