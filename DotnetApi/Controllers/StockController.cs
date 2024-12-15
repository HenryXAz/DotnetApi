using DotnetApi.Dtos.Stock;
using DotnetApi.Interfaces;
using DotnetApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers 
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepository.GetAllAsync();                
            var stocksDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocksDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult >Post([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);  

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockRequestDto)
        {
            var stockModel = await _stockRepository.UpdateAsync(id, stockRequestDto);

            if (stockModel == null) {
                return NotFound();
            }

            return Redirect(Url.Action("GetById", "StockController", new { id = stockModel.Id }));
        } 

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null) {
                return NotFound();
            }

            return NoContent(); 
        }
    }
}
