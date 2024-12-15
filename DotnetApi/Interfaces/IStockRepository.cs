using DotnetApi.Dtos.Stock;
using DotnetApi.Models;

namespace DotnetApi.Interfaces
{
    public interface IStockRepository
    {
        public Task<List<Stock>> GetAllAsync();
        public Task<Stock?> GetByIdAsync(int id);
        public Task<Stock> CreateAsync(Stock stockModel);
        public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockRequestDto);
        public Task<Stock?> DeleteAsync(int id);
        public Task<Boolean> StockExists(int id);
    }
}