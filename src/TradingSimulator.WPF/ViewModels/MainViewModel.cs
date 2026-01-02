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

        [ObservableProperty]
        private decimal _balance;

        [ObservableProperty]
        private decimal _totalValue;

        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            RefreshData();
        }
        private void RefreshData()
        {
            Balance = _portfolioService.Balance;
            TotalValue = _portfolioService.TotalValue;

            int stocksCount = _marketService.Stocks.Count;

            StatusMessage = $"Data Refreshed. Cash: {Balance:C} " +
                $"| Portfolio's Value: {TotalValue:C} " +
                $"| Market Stocks: {stocksCount}";
        }
    }
}
