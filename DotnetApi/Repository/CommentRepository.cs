using DotnetApi.Data;
using DotnetApi.Dtos.Comment;
using DotnetApi.Interfaces;
using DotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _dbContext.Comments.AddAsync(commentModel);
            await _dbContext.SaveChangesAsync();

            return commentModel; 
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null) {
                return null;
            }

            _dbContext.Remove(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> GetAllSync()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments.FindAsync(id) ?? null;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentRequestDto)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null) {
                return null;
            }

            comment.Title = commentRequestDto.Title;
            comment.Content = commentRequestDto.Content;

            await _dbContext.SaveChangesAsync();

            return comment;
        }
    }
}