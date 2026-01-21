# SessionState

Brief: The `SessionState` class represents a complete snapshot of a trading session. It is used to persist or restore a user's trading session, display account and portfolio information in the UI, or run analyses and backtests.

## Properties

- `Balance` (decimal)
  - Current available cash balance. Update on deposits, withdrawals and when trades settle.
- `RealizedPnL` (decimal)
  - Cumulative realized profit and loss from closed positions. Does not include unrealized PnL.
- `Items` (List<PortfolioItem>)
  - Portfolio holdings. Each `PortfolioItem` contains `Stock`, `Quantity`, `AveragePrice` and `UnrealizedPnL`.
  - Initialized to an empty list to avoid null checks.
- `Transactions` (List<Transaction>)
  - Execution history: BUY and SELL transactions with timestamp, quantity and price. Useful for audit and UI history.
- `MarketData` (List<Stock>)
  - Price history / market snapshots for stocks (including those not owned). Helps compute unrealized PnL or drive charts.

## Usage notes & invariants

- Collections are initialized in the constructor to simplify consumers (no need to check for null).
- `RealizedPnL` should be updated when closing positions (e.g., when processing a SELL that reduces position to a lower quantity or to zero).
- `Items` and `Transactions` together allow reconstructing portfolio state; `MarketData` is optional but useful for charts/backtests.
- The class is intentionally simple and suited for JSON (de)serialization to persist session snapshots.

## Example: applying a transaction (conceptual)
1. Validate the transaction (sufficient balance for BUY, sufficient quantity for SELL).
2. Update `Balance` (subtract total value + fees on BUY, add total value - fees on SELL).
3. Update `Items`: add/increment or remove/decrement the related `PortfolioItem`.
4. Update `RealizedPnL` for any closed portion of a position.
5. Append the executed `Transaction` to `Transactions`.