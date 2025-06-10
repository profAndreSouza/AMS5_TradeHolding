using System;
using System.Collections.Generic;

namespace CurrencyAPI.Domain.Entities
{
    public class Currency
    {
        public Guid Id { get; private set; }
        public string Symbol { get; private set; }
        public string Name { get; private set; }
        public string Backing { get; private set; }
        public bool Reverse { get; private set; } = false;

        // Relacionamento: 1 Currency tem N Histories
        private readonly List<History> _histories = new();
        public IReadOnlyCollection<History> Histories => _histories.AsReadOnly();

        public Currency(string symbol, string name, string backing, bool reverse = false)
        {
            Id = Guid.NewGuid();
            SetSymbol(symbol);
            SetName(name);
            SetBacking(backing);
            SetReverse(reverse);
        }

        public void AddHistory(History history)
        {
            if (history == null)
                throw new ArgumentNullException(nameof(history));

            if (history.CurrencyId != Id)
                throw new InvalidOperationException("History currency ID mismatch.");

            _histories.Add(history);
        }

        private void SetSymbol(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol is required.");

            Symbol = symbol.ToUpper();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            Name = name;
        }

        private void SetBacking(string backing)
        {
            if (string.IsNullOrWhiteSpace(backing))
                throw new ArgumentException("Backing is required.");

            Backing = backing.ToUpper();
        }

        public void SetReverse(bool reverse)
        {
            Reverse = reverse;
        }
    }
}
