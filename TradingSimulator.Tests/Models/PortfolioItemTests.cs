using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.Core.Models;
using Xunit;

namespace TradingSimulator.Tests.Models
{
    public class PortfolioItemTests
    {
        [Fact]
        public void Constructor_CreatesPortfolioItem_WhenDataIsValid()
        {
            var stock = new Stock("AAPL", "Apple", 150m);

            var item = new PortfolioItem(stock, 5, 150m);

            Assert.Equal(5, item.Quantity);
            Assert.Equal(stock, item.Stock);
            Assert.Equal(150m, item.AveragePrice);
        }

        [Fact]
        public void Add_IncreasesQuantity_AndRecalculatesAveragePrice()
        {
            var stock = new Stock("AAPL", "Apple", 150m);
            var item = new PortfolioItem(stock, 5, 150m);

            item.Add(3, 180m);

            Assert.Equal(8, item.Quantity);

            var expectedAvg =
                (150m * 5 + 180m * 3) / 8;

            Assert.Equal(expectedAvg, item.AveragePrice);
        }

        [Fact]
        public void Remove_DecreasesQuantity()
        {
            var item = new PortfolioItem(
                new Stock("AAPL", "Apple", 150m),
                5,
                150m
            );

            item.Remove(2);

            Assert.Equal(3, item.Quantity);
        }

        [Fact]
        public void Remove_Throws_WhenRemovingTooMuch()
        {
            var item = new PortfolioItem(
                new Stock("AAPL", "Apple", 150m),
                2,
                150m
            );

            Assert.Throws<InvalidOperationException>(() => item.Remove(5));
        }
    }
}

    

