using Microsoft.AspNetCore.Mvc;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.Application.DTOs;
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
                Backing = c.Backing
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
                Backing = currency.Backing
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurrencyDto dto)
        {
            var currency = new Currency(dto.Symbol, dto.Name, dto.Backing);
            await _service.AddAsync(currency);
            return CreatedAtAction(nameof(GetById), new { id = currency.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CurrencyDto dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var updated = new Currency(dto.Symbol, dto.Name, dto.Backing); // ou atualize campos diretamente
            typeof(Currency).GetProperty("Id")?.SetValue(updated, id); // ajustar ID manualmente

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
    }
}
