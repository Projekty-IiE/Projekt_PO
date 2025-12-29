using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TradingSimulator.Core.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public decimal RequiredAmount { get; }
        public decimal AvailableAmount { get; }

        public InsufficientFundsException(decimal required, decimal available)
            : base(
                $"Insufficient funds. Required: {required:C}, Available: {available:C}"
              )
        {
            RequiredAmount = required;
            AvailableAmount = available;
        }

        public InsufficientFundsException() { }
        public InsufficientFundsException(string message) : base(message) { }
        public InsufficientFundsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
