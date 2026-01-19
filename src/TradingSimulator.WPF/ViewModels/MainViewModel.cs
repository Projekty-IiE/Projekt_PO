using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Services;
using TradingSimulator.WPF.Services;
using TradingSimulator.WPF.Views;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;


namespace TradingSimulator.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly IMarketService _marketService;
        private readonly IPortfolioService _portfolioService;
        private readonly FileService _fileService = new FileService(); 
        private readonly SoundService _soundService = new();
        private DispatcherTimer _timer = null!;
        private TransactionHistoryWindow? _transactionHistoryWindow;
        public SeriesCollection PriceSeries { get; set; } = new SeriesCollection();
        public ObservableCollection<PortfolioItem> PortfolioItems { get; } = new();
        public ObservableCollection<Transaction> Transactions { get; } = new();
        public ObservableCollection<Stock> AvailableStocks { get; } = new();
        public IPortfolioService PortfolioService => _portfolioService; //so that MainWindow can see acc balance

        [ObservableProperty]
        private string[] _labels = Array.Empty<string>();

        [ObservableProperty]
        private string _statusMessage = "Welcome!";

        [ObservableProperty]
        private string? _transactionMessage;

        [ObservableProperty]
        private decimal _balance;

        [ObservableProperty]
        private decimal _totalValue;

        [ObservableProperty]
        private decimal _totalRealizedPnL;

        [ObservableProperty]
        private decimal _totalUnrealizedPnL;

        [ObservableProperty]
        private string _autoTickButtonText = "START AUTO-TICK";

        [ObservableProperty]
        private string _symbolInput = string.Empty;

        [ObservableProperty]
        private int _quantityInput;

        [ObservableProperty]
        private PortfolioItem? _selectedPortfolioItem;

        public MainViewModel(IMarketService marketService, IPortfolioService portfolioService)
        {
            _marketService = marketService;
            _portfolioService = portfolioService;

            foreach (var stock in _marketService.Stocks)
            {
                AvailableStocks.Add(stock);
            }

            InitializeTimer();
            WarmUpMarket(20);
            SeedDemoData();
            RefreshData();
            SelectedPortfolioItem = PortfolioItems.FirstOrDefault();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (s, e) => NextTick();
        }

        private void WarmUpMarket(int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                _marketService.UpdateMarket();
            }
        }
        private void SeedDemoData()
        {
            try
            {
                _portfolioService.Buy("AAPL", 5);
                _portfolioService.Buy("TSLA", 2);
                _portfolioService.Buy("MSFT", 1);
            }
            catch { }
        }

        [RelayCommand]
        private void NextTick()
        {
            _marketService.UpdateMarket();
            UpdateChart(SelectedPortfolioItem?.Stock);
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

                _soundService.Play("trade_open.wav");
                
                TransactionMessage = $"Success! Bought {transaction.Quantity} x {transaction.StockSymbol} @ {transaction.PricePerShare:C}";
                RefreshData();
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

                _soundService.Play("trade_close.wav");

                string pnlText = transaction.RealizedPnL >= 0 ? $"+{transaction.RealizedPnL:C}" : $"{transaction.RealizedPnL:C}";
                TransactionMessage = $"SOLD! {transaction.Quantity} x {transaction.StockSymbol} (PnL: {pnlText})";

                RefreshData();
                QuantityInput = 0;
            }
            catch (Exception ex)
            {
                TransactionMessage = $"SELL FAILED: {ex.Message}";
            }
        }

        [RelayCommand]
        private void OpenTransactionHistory()
        {
            if (_transactionHistoryWindow == null || !_transactionHistoryWindow.IsVisible)
            {
                _transactionHistoryWindow = new TransactionHistoryWindow(this);
                _transactionHistoryWindow.Show();
            }
            else
            {
                _transactionHistoryWindow.Activate();
            }
        }
        private void RefreshData()
        {
            var selectedSymbol = SelectedPortfolioItem?.Stock.Symbol;

            Balance = _portfolioService.Balance;
            TotalValue = _portfolioService.TotalValue;
            TotalRealizedPnL = _portfolioService.RealizedPnL;

            PortfolioItems.Clear();
            foreach (var item in _portfolioService.Items)
            {
                PortfolioItems.Add(item);
            }

            SelectedPortfolioItem = PortfolioItems
                .FirstOrDefault(p => p.Stock.Symbol == selectedSymbol);

            TotalUnrealizedPnL = PortfolioItems.Any()
                ? PortfolioItems.Sum(i => i.UnrealizedPnL)
                : 0;
        }

        partial void OnSelectedPortfolioItemChanged(PortfolioItem? value)
        {
            if (value?.Stock == null)
                return;
            UpdateChart(value.Stock);
        }

        partial void OnSymbolInputChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var stock = AvailableStocks.FirstOrDefault(s => s.Symbol == value);
            if (stock == null)
                return;

            UpdateChart(stock);
        }

        private void UpdateChart(Stock? stock)
        {
            if (stock == null)
                return;

            PriceSeries.Clear();

            PriceSeries.Add(new LineSeries
            {
                Title = stock.Symbol,
                Values = new ChartValues<decimal>(stock.PriceHistory),
                PointGeometry = null,
                StrokeThickness = 2,
                LineSmoothness = 0.1,
                Fill = Brushes.Transparent
            });

            Labels = Enumerable.Range(1, stock.PriceHistory.Count)
                               .Select(i => i.ToString())
                               .ToArray();
        }

        [RelayCommand]
        private void SaveSession()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName="TradingSave",
                DefaultExt=".json",
                Filter="JSON documents (.json)|*.json"
            };

            if(dialog.ShowDialog()==true)
            {
                var state = new SessionState()
                {
                    Balance = _portfolioService.Balance,
                    RealizedPnL=_portfolioService.RealizedPnL,
                    Items = _portfolioService.Items.ToList(),
                    Transactions = _portfolioService.Transactions.ToList(),
                    MarketData=_portfolioService.AllStocks.ToList(),
                };
                _fileService.Save(dialog.FileName, state);
                StatusMessage = "Session Saved Successfully!";
            }
        }
        [RelayCommand]
        private void LoadSession()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "JSON documents (.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                var state = _fileService.Load(dialog.FileName);

                if (state != null)
                {
                    _portfolioService.LoadPortfolio(
                        state.Balance, 
                        state.RealizedPnL,
                        state.Items ?? new List<PortfolioItem>(),
                        state.Transactions ?? new List<Transaction>(),
                        state.MarketData
                        );

                    RefreshData(); 
                    Transactions.Clear();
                    foreach (var t in _portfolioService.Transactions)
                    {
                        Transactions.Insert(0, t);
                    }

                    StatusMessage = "Session Loaded Successfully!";
                }
            }
        }

    }
}