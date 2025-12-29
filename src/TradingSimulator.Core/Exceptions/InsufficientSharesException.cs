using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Exceptions
{
    public class InsufficientSharesException : Exception
    {
        public int RequiredShares { get; }
        public int AvailableShares { get; }

        public InsufficientSharesException(int required, int available)
            : base($"Insufficient shares. Required: {required}, Available: {available}")
        {
            RequiredShares = required;
            AvailableShares = available;
        }
    }
}


