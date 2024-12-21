using DotnetApi.Dtos.Comment;
using DotnetApi.Extensions;
using DotnetApi.Interfaces;
using DotnetApi.Mappers;
using DotnetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers
{

    [Route("/api/comments")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid) {
                return BadRequest();
            }

            var comments = await _commentRepository.GetAllSync();

            return Ok(comments.Select(s => s.ToCommentDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid) {
                return BadRequest();
            }

            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null) {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Post([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentRequestDto)
        {
            if(!ModelState.IsValid) {
                return BadRequest();
            }

            if (!await _stockRepository.StockExists(stockId)) {
                return BadRequest("Stock does not exists");
            }

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = commentRequestDto.ToCommentFromCreateDto(stockId); 
            commentModel.AppUserId = appUser.Id;
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentRequestDto)
        {
            if(!ModelState.IsValid) {
                return BadRequest();
            }

            var comment = await _commentRepository.UpdateAsync(id, commentRequestDto);

            return (comment == null) ? NotFound() : Ok(comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid) {
                return BadRequest();
            }

            var comment = await _commentRepository.DeleteAsync(id);

            return (comment == null) ? NotFound() : Ok(comment);              
        }

    }
}
