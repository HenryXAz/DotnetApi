using DotnetApi.Dtos.Comment;
using DotnetApi.Models;

namespace DotnetApi.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto() {
                Id = commentModel.Id,
                Tittle = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId,
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto commentRequestDto, int stockId)
        {
            return new Comment() {
                Title = commentRequestDto.Title,
                Content = commentRequestDto.Content,
                StockId = stockId,
            };
        }

        public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto commentRequestDto) 
        {
            return new Comment() {
                Title = commentRequestDto.Title,
                Content = commentRequestDto.Content,
            };
        }
    }
}