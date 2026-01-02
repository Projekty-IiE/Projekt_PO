using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TradingSimulator.Core.Interfaces;

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
        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            LoadInitialStatus();
        }
        private void LoadInitialStatus()
        {
            decimal currentCash = _portfolioService.Balance;

            int stocksCount = _marketService.Stocks.Count;

            StatusMessage = $"System Ready. Cash: {currentCash:C} | Market Stocks: {stocksCount}";
        }
    }
}
