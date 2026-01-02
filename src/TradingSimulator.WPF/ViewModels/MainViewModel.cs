using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TradingSimulator.Core.Interfaces;
using System.Collections.ObjectModel;
using TradingSimulator.Core.Models;

namespace TradingSimulator.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IMarketService _marketService;
        private readonly IPortfolioService _portfolioService;

        [ObservableProperty]
        private string _title = "Mini Trading Simulator";

        [ObservableProperty]
        private string _statusMessage = "Loading...";

        [ObservableProperty]
        private decimal _balance;

        [ObservableProperty]
        private decimal _totalValue;

        public ObservableCollection<PortfolioItem> PortfolioItems { get; } = new();

        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            // test
            try
            {
                _portfolioService.Buy("AAPL", 5);
                _portfolioService.Buy("TSLA", 2);
            }
            catch { }

            RefreshData();
        }
        private void RefreshData()
        {
            Balance = _portfolioService.Balance;
            TotalValue = _portfolioService.TotalValue;

            int stocksCount = _marketService.Stocks.Count;

            StatusMessage = $"Data Refreshed. Cash: {Balance:C} " +
                $"| Stocks' Value: {TotalValue-Balance:C} " +
                $"| Portfolio's Value: {TotalValue:C} " +
                $"| Market Stocks: {stocksCount}";

            PortfolioItems.Clear();
            foreach (var item in _portfolioService.Items)
            {
                PortfolioItems.Add(item);
            }
        }
    }
}
