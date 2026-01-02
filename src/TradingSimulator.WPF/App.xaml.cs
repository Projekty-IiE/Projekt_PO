using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using TradingSimulator.Core.Interfaces;
using TradingSimulator.Core.Models;
using TradingSimulator.Core.Services;
using TradingSimulator.WPF.ViewModels;
using TradingSimulator.WPF.Views; 
namespace TradingSimulator.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
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
                    new Stock("GOOGL", "Alphabet Inc.", 2800m),
                    new Stock("PLTR", "Palantir Tech", 25m)
                };
            return new MarketEngine(initialStocks);
        });
        
        services.AddSingleton<Portfolio>(provider => new Portfolio(10000m));

        services.AddSingleton<IPortfolioService, PortfolioService>();

        services.AddSingleton<MainViewModel>();

        services.AddSingleton<MainWindow>();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}


