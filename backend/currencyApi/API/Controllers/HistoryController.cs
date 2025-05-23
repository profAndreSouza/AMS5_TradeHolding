using Microsoft.AspNetCore.Mvc;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.Application.DTOs;
using CurrencyAPI.Domain.Entities;

namespace CurrencyAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _service;

        public HistoryController(IHistoryService service)
        {
            _service = service;
        }

        [HttpGet("{currencyId:guid}")]
        public async Task<IActionResult> GetByCurrency(Guid currencyId)
        {
            var histories = await _service.GetByCurrencyIdAsync(currencyId);
            var result = histories.Select(h => new HistoryDto
            {
                Id = h.Id,
                CurrencyId = h.CurrencyId,
                Price = h.Price,
                Date = h.Date
            });

            return Ok(result);
        }

        [HttpGet("{currencyId:guid}/range")]
        public async Task<IActionResult> GetByDateRange(Guid currencyId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var histories = await _service.GetByDateRangeAsync(currencyId, from, to);
            var result = histories.Select(h => new HistoryDto
            {
                Id = h.Id,
                CurrencyId = h.CurrencyId,
                Price = h.Price,
                Date = h.Date
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HistoryDto dto)
        {
            var history = new History(dto.CurrencyId, dto.Price, dto.Date);
            await _service.AddAsync(history);
            return Created("", dto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
