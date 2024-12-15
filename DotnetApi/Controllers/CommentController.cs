using DotnetApi.Dtos.Comment;
using DotnetApi.Interfaces;
using DotnetApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers
{
    [Route("/api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllSync();

            return Ok(comments.Select(s => s.ToCommentDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null) {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Post([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentRequestDto)
        {
            if (!await _stockRepository.StockExists(stockId)) {
                return BadRequest("Stock does not exists");
            }

            var commentModel = commentRequestDto.ToCommentFromCreateDto(stockId); 
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentRequestDto)
        {
            var comment = await _commentRepository.UpdateAsync(id, commentRequestDto);

            return (comment == null) ? NotFound() : Ok(comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteAsync(id);

            return (comment == null) ? NotFound() : Ok(comment);              
        }

    }
}
