using DotnetApi.Data;
using DotnetApi.Dtos.Stock;
using DotnetApi.Interfaces;
using DotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;
        
        public StockRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _dbContext.Stocks.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null) {
                return null;
            }

            _dbContext.Stocks.Remove(stockModel);
            await _dbContext.SaveChangesAsync();

            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _dbContext.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _dbContext.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockRequestDto)
        {
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        
            if (stock == null) {
                return null;
            }
            
        
            stock.Symbol = stockRequestDto.Symbol;
            stock.CompanyName = stockRequestDto.CompanyName;
            stock.Purchase = stockRequestDto.Purchase;
            stock.LastDiv = stockRequestDto.LastDiv;
            stock.Industry = stockRequestDto.Industry;
            stock.MarketCap = stockRequestDto.MarketCap;
        
            await _dbContext.SaveChangesAsync();
        
            return stock;
        }
    }
}