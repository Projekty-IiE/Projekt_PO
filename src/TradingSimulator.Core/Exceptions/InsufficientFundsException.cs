using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TradingSimulator.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an account has insufficient funds to complete a transaction.
    /// </summary>
    public class InsufficientFundsException : Exception
    {
        /// <summary>
        /// Gets the amount required to complete the transaction.
        /// </summary>
        public decimal RequiredAmount { get; }

        /// <summary>
        /// Gets the amount currently available in the account.
        /// </summary>
        public decimal AvailableAmount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientFundsException"/> class with required and available amounts.
        /// </summary>
        /// <param name="required">The required amount.</param>
        /// <param name="available">The available amount.</param>
        public InsufficientFundsException(decimal required, decimal available)
            : base(
                $"Insufficient funds. Required: {required:C}, Available: {available:C}"
              )
        {
            RequiredAmount = required;
            AvailableAmount = available;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientFundsException"/> class.
        /// </summary>
        public InsufficientFundsException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientFundsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InsufficientFundsException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientFundsException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public InsufficientFundsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
