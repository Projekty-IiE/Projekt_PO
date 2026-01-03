using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly DispatcherTimer _timer;

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
        private string _symbolToBuy = string.Empty;

        [ObservableProperty]
        private int _quantityToBuy;

        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => NextTick();

            // Testowe dane
            try
            {
                _portfolioService.Buy("AAPL", 5);
                _portfolioService.Buy("TSLA", 2);
            }
            catch { }

            RefreshData();
        }

        [RelayCommand]
        private void NextTick()
        {
            _marketService.UpdateMarket();
            RefreshData();

            if (!_timer.IsEnabled)
            {
                // Opcjonalne: można tu zaktualizować status przy ręcznym kliknięciu
            }
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
            if (string.IsNullOrWhiteSpace(SymbolToBuy))
            {
                StatusMessage = "Error: Please enter a stock symbol.";
                return;
            }

            if (QuantityToBuy <= 0)
            {
                StatusMessage = "Error: Quantity must be greater than 0.";
                return;
            }

            try
            {
                var transaction = _portfolioService.Buy(SymbolToBuy, QuantityToBuy);
                StatusMessage = $"Success! Bought {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
                SymbolToBuy = string.Empty;
                QuantityToBuy = 0;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Transaction Failed: {ex.Message}";
            }
        }

        [RelayCommand]
        private void SellStock()
        {
            if (string.IsNullOrWhiteSpace(SymbolToBuy))
            {
                StatusMessage = "Error: Please enter a stock symbol to sell.";
                return;
            }

            if (QuantityToBuy <= 0)
            {
                StatusMessage = "Error: Quantity must be greater than 0.";
                return;
            }

            try
            {
                var transaction = _portfolioService.Sell(SymbolToBuy, QuantityToBuy);
                StatusMessage = $"SOLD! {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
                SymbolToBuy = string.Empty;
                QuantityToBuy = 0;
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
            int stocksCount = _marketService.Stocks.Count;

            StatusMessage = $"Data Refreshed. Cash: {Balance:C} | Portfolio Value: {TotalValue:C}";

            PortfolioItems.Clear();
            foreach (var item in _portfolioService.Items)
            {
                PortfolioItems.Add(item);
            }
        }
    }
}