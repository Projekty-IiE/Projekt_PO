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

        [ObservableProperty]
        private string _title = "Mini Trading Simulator";

        [ObservableProperty]
        private string _statusMessage = "Loading...";

        [ObservableProperty]
        private string _transactionMessage;

        [ObservableProperty]
        private decimal _balance;

        [ObservableProperty]
        private decimal _totalValue;

        [ObservableProperty]
        private string _autoTickButtonText = "START AUTO-TICK";

        public ObservableCollection<PortfolioItem> PortfolioItems { get; } = new();
        public ObservableCollection<Transaction> Transactions { get; } = new();

        // --- ZMIANA 1: Lista dostępnych akcji dla Dropdowna ---
        public ObservableCollection<Stock> AvailableStocks { get; } = new();

        [ObservableProperty]
        private string _symbolInput = string.Empty;

        [ObservableProperty]
        private int _quantityInput;

        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            // --- ZMIANA 2: Załadowanie listy akcji z serwisu do listy w ViewModelu ---
            foreach (var stock in _marketService.Stocks)
            {
                AvailableStocks.Add(stock);
            }

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
            try
            {
                _portfolioService.Buy("AAPL", 5);
                _portfolioService.Buy("TSLA", 2);
            }
            catch { }
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
                TransactionMessage = "Error: Please select a stock symbol.";
                return;
            }

            if (QuantityInput <= 0)
            {
                TransactionMessage = "Error: Quantity must be greater than 0.";
                return;
            }

            try
            {
                var transaction = _portfolioService.Buy(SymbolInput, QuantityInput);
                Transactions.Insert(0, transaction);
                TransactionMessage = $"Success! Bought {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
                // Nie czyścimy SymbolInput, żeby użytkownik mógł łatwo dokupić więcej tego samego
                QuantityInput = 0;
            }
            catch (Exception ex)
            {
                TransactionMessage = $"Transaction Failed: {ex.Message}";
            }
        }

        [RelayCommand]
        private void SellStock()
        {
            if (string.IsNullOrWhiteSpace(SymbolInput))
            {
                TransactionMessage = "Error: Please select a stock symbol to sell.";
                return;
            }

            if (QuantityInput <= 0)
            {
                TransactionMessage = "Error: Quantity must be greater than 0.";
                return;
            }

            try
            {
                var transaction = _portfolioService.Sell(SymbolInput, QuantityInput);
                Transactions.Insert(0, transaction);
                TransactionMessage = $"SOLD! {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
                QuantityInput = 0;
            }
            catch (Exception ex)
            {
                TransactionMessage = $"SELL FAILED: {ex.Message}";
            }
        }

        private void RefreshData()
        {
            Balance = _portfolioService.Balance;
            TotalValue = _portfolioService.TotalValue;

            StatusMessage = $"Cash: {Balance:C} | Portfolio Value: {TotalValue:C}";

            PortfolioItems.Clear();
            foreach (var item in _portfolioService.Items)
            {
                PortfolioItems.Add(item);
            }
        }
    }
}