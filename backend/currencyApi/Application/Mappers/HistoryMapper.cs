using CurrencyAPI.Domain.Entities;
using CurrencyAPI.API.DTOs;

namespace CurrencyAPI.Application.Mappers
{

    public static class HistoryMapper
    {
        public static History ToEntity(this HistoryDto dto)
        {
            return new History(
                dto.CurrencyId,
                dto.Price,
                dto.Date
            );
        }
    }

}