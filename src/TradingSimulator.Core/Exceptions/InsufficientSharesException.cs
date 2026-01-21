using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation requires more shares than are available.
    /// </summary>
    public class InsufficientSharesException : Exception
    {
        /// <summary>
        /// Gets the number of shares required for the operation.
        /// </summary>
        public int RequiredShares { get; }

        /// <summary>
        /// Gets the number of shares available.
        /// </summary>
        public int AvailableShares { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientSharesException"/> class with the required and available shares.
        /// </summary>
        /// <param name="required">The number of shares required.</param>
        /// <param name="available">The number of shares available.</param>
        public InsufficientSharesException(int required, int available)
            : base(
                $"Insufficient shares. Required: {required}, Available: {available}"
              )
        {
            RequiredShares = required;
            AvailableShares = available;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientSharesException"/> class.
        /// </summary>
        public InsufficientSharesException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientSharesException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InsufficientSharesException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientSharesException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public InsufficientSharesException(string message, Exception inner)
            : base(message, inner) { }
    }
}

