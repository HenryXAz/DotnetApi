using DotnetApi.Models;
using DotnetApi.Dtos.Comment;

namespace DotnetApi.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllSync();
        public Task<Comment?> GetByIdAsync(int id);
        public Task<Comment> CreateAsync(Comment commentModel);
        public Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentRequestDto);
        public Task<Comment?> DeleteAsync(int id);

    }
}