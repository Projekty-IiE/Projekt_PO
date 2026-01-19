using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Services;
using TradingSimulator.WPF.ViewModels;
using TradingSimulator.WPF.Views;
using System.Globalization;
using System.Threading;
using TradingSimulator.Core.Data;

namespace TradingSimulator.WPF
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IMarketService>(provider =>
            {
                var initialStocks = new List<Stock>
                {
                    new Stock("AAPL", "Apple Inc.", 150m),
                    new Stock("TSLA", "Tesla Inc.", 220m),
                    new Stock("MSFT", "Microsoft", 310m),
                    new Stock("GOOGL", "Alphabet Inc.", 280m),
                    new Stock("PLTR", "Palantir Tech", 140m),
                    new Stock("BTCUSD", "Bitcoin", 96430m),
                    new Stock("NFLX", "Netflix Inc.", 88m),
                    new Stock("ADBE", "Adobe Inc.", 112m),
                    new Stock("WHR", "Whirlpool Corporation", 84m),
                    new Stock("INTU", "Intuit Inc.", 25m),
                    new Stock("ORCL", "Oracle Corporation", 121m),
                    new Stock("ICE", "Intercontinental Exch.", 140m),
                    new Stock("JPM", "JP Morgan Chase", 170m),
                    new Stock("NKE", "Nike", 70m),
                    new Stock("META", "Meta", 690m)
                };
                return new MarketEngine(initialStocks);
            });
            services.AddDbContext<TradingDbContext>();
            services.AddSingleton<Portfolio>(provider => new Portfolio(100000m));
            services.AddSingleton<IPortfolioService, PortfolioService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var culture = new CultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            base.OnStartup(e);

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}