using DotnetApi.Data;
using DotnetApi.Dtos.Stock;
using DotnetApi.Helpers;
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

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _dbContext.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.CompanyName)) {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            } 

            if (!string.IsNullOrWhiteSpace(query.Symbol)) {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy)) {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)) {
                    stocks = query.IsDescending 
                    ?
                        stocks.OrderByDescending(s => s.Symbol)
                    :
                        stocks.OrderBy(s => s.Symbol);
                }

            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
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