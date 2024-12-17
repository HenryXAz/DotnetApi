using System.ComponentModel.DataAnnotations;

namespace DotnetApi.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "title must be 5 character")]
        [MaxLength(280, ErrorMessage = "title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "content be 5 character")]
        [MaxLength(280, ErrorMessage = "content cannot be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
