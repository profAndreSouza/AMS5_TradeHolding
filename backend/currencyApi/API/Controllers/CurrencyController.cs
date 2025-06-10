using Microsoft.AspNetCore.Mvc;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.API.DTOs;
using CurrencyAPI.Domain.Entities;

namespace CurrencyAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _service;

        public CurrencyController(ICurrencyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _service.GetAllAsync();
            var result = currencies.Select(c => new CurrencyDto
            {
                Id = c.Id,
                Symbol = c.Symbol,
                Name = c.Name,
                Backing = c.Backing,
                Reverse = c.Reverse,
                Histories = c.Histories.Select(h => new HistoryDto
                {
                    Id = h.Id,
                    CurrencyId = h.CurrencyId,
                    Price = h.Price,
                    Date = h.Date
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currency = await _service.GetByIdAsync(id);
            if (currency == null) return NotFound();

            return Ok(new CurrencyDto
            {
                Id = currency.Id,
                Symbol = currency.Symbol,
                Name = currency.Name,
                Backing = currency.Backing,
                Reverse = currency.Reverse
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurrencyDto dto)
        {
            if (!ModelState.IsValid)  // Valida as anotações [Required] do DTO
                return BadRequest(ModelState);

            var currency = new Currency(dto.Symbol, dto.Name, dto.Backing, dto.Reverse);
            await _service.AddAsync(currency);
            return CreatedAtAction(nameof(GetById), new { id = currency.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CurrencyDto dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var updated = new Currency(dto.Symbol, dto.Name, dto.Backing, dto.Reverse);
            typeof(Currency).GetProperty("Id")?.SetValue(updated, id);

            await _service.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }

         // GET api/currencies/{id}/history?start=2023-01-01&end=2023-01-31
        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(Guid id, DateTime? start, DateTime? end)
        {
            var exists = await _service.GetByIdAsync(id);
            if (exists == null)
                return NotFound();

            var history = await _service.GetHistoryAsync(id, start, end);
            return Ok(history);
        }

        // GET api/currencies/convert?from=USD&to=EUR&amount=100
        [HttpGet("convert")]
        public async Task<IActionResult> Convert(string from, string to, decimal amount)
        {
            // lógica de conversão
            return Ok();
        }
    }
}
