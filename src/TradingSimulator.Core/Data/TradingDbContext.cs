using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Data
{
    public class TradingDbContext : DbContext
    {
        // G³ówna tabela
        public DbSet<Transaction> Transactions { get; set; }

        // Podtypy (dla obs³ugi dziedziczenia)
        public DbSet<BuyTransaction> BuyTransactions { get; set; }
        public DbSet<SellTransaction> SellTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Baza w pliku trading.db
            optionsBuilder.UseSqlite("Data Source=trading.db");
        }
    }
}