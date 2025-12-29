using System;

namespace TradingSimulator.Core.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public decimal RequiredAmount { get; }
        public decimal AvailableAmount { get; }

        public InsufficientFundsException(decimal required, decimal available)
            : base($"Insufficient funds. Required: {required}, Available: {available}")
        {
            RequiredAmount = required;
            AvailableAmount = available;
        }
    }
}
