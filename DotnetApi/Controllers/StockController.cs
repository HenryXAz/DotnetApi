using DotnetApi.Dtos.Stock;
using DotnetApi.Helpers;
using DotnetApi.Interfaces;
using DotnetApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers 
{
    [Route("api/stocks")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var stocks = await _stockRepository.GetAllAsync(query);                
            var stocksDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stocksDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);  

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockRequestDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var stockModel = await _stockRepository.UpdateAsync(id, stockRequestDto);

            if (stockModel == null) {
                return NotFound();
            }

            return Redirect(Url.Action("GetById", "StockController", new { id = stockModel.Id }));
        } 

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null) {
                return NotFound();
            }

            return NoContent(); 
        }
    }
}
