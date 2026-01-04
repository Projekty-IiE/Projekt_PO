using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;
using System.Windows.Threading;

namespace TradingSimulator.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly IMarketService _marketService;
        private readonly IPortfolioService _portfolioService;
        private DispatcherTimer _timer;

        /// <summary>
        /// Każde pole oznaczone [ObservableProperty] community toolkit automatycznie generuje PUBLICZNĄ właściwość z INotifyPropertyChanged.
        /// </summary>
        [ObservableProperty]
        private string _title = "Mini Trading Simulator";

        [ObservableProperty]
        private string _statusMessage = "Loading...";

        [ObservableProperty]
        private decimal _balance;

        [ObservableProperty]
        private decimal _totalValue;

        [ObservableProperty]
        private string _autoTickButtonText = "START AUTO-TICK";

        public ObservableCollection<PortfolioItem> PortfolioItems { get; } = new();

        [ObservableProperty]
        private string _symbolInput = string.Empty;

        [ObservableProperty]
        private int _quantityInput;

        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            InitializeTimer();
            SeedDemoData();
            RefreshData();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (s, e) => NextTick();
        }

        private void SeedDemoData()
        {
            _portfolioService.Buy("AAPL", 5);
            _portfolioService.Buy("TSLA", 2);
        }

        [RelayCommand]
        private void NextTick()
        {
            _marketService.UpdateMarket();
            RefreshData();
        }

        [RelayCommand]
        private void ToggleAutoTick()
        {
            if (_timer.IsEnabled)
            {

                _timer.Stop();
                AutoTickButtonText = "START AUTO-TICK";
                StatusMessage = "Simulation Paused.";
            }
            else
            {

                _timer.Start();
                AutoTickButtonText = "STOP AUTO-TICK";
                StatusMessage = "Simulation Running...";
            }
        }

        [RelayCommand]
        private void BuyStock()
        {
            if (string.IsNullOrWhiteSpace(SymbolInput))
            {
                StatusMessage = "Error: Please enter a stock symbol.";
                return;
            }

            if (QuantityInput <= 0)
            {
                StatusMessage = "Error: Quantity must be greater than 0.";
                return;
            }

            try
            {
                var transaction = _portfolioService.Buy(SymbolInput, QuantityInput);
                StatusMessage = $"Success! Bought {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
                SymbolInput = string.Empty;
                QuantityInput = 0;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Transaction Failed: {ex.Message}";
            }
        }

        [RelayCommand]
        private void SellStock()
        {
            if (string.IsNullOrWhiteSpace(SymbolInput))
            {
                StatusMessage = "Error: Please enter a stock symbol to sell.";
                return;
            }

            if (QuantityInput <= 0)
            {
                StatusMessage = "Error: Quantity must be greater than 0.";
                return;
            }

            try
            {
                var transaction = _portfolioService.Sell(SymbolInput, QuantityInput);
                StatusMessage = $"SOLD! {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
                SymbolInput = string.Empty;
                QuantityInput = 0;
            }
            catch (Exception ex)
            {
                StatusMessage = $"SELL FAILED: {ex.Message}";
            }
        }

        private void RefreshData()
        {
            Balance = _portfolioService.Balance;
            TotalValue = _portfolioService.TotalValue;

            StatusMessage = $"Data Refreshed. Cash: {Balance:C} | Portfolio Value: {TotalValue:C}";

            PortfolioItems.Clear();
            foreach (var item in _portfolioService.Items)
            {
                PortfolioItems.Add(item);
            }
        }
    }
}