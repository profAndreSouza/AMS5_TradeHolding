using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CurrencyAPI.Domain.Entities
{
    public class History
    {
        public Guid Id { get; private set; }
        public Guid CurrencyId { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Date { get; private set; }

        // Propriedade de navegação
        [JsonIgnore]
        public Currency Currency { get; private set; }

        public History(Guid currencyId, decimal price, DateTime date)
        {
            Id = Guid.NewGuid();
            SetCurrencyId(currencyId);
            SetPrice(price);
            SetDate(date);
        }

        private void SetCurrencyId(Guid currencyId)
        {
            if (currencyId == Guid.Empty)
                throw new ArgumentException("CurrencyId is invalid.");

            CurrencyId = currencyId;
        }

        private void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Price must be greater than or equal to zero.");

            Price = price;
        }

        private void SetDate(DateTime date)
        {
            if (date == default)
                throw new ArgumentException("Date is required.");

            Date = date;
        }
    }
}
