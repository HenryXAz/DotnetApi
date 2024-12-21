using DotnetApi.Data;
using DotnetApi.Interfaces;
using DotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private ApplicationDBContext _context;

        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context; 
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio?> DeletePortfolio(AppUser user, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUser == user && x.Stock.Symbol.ToLower() == symbol.ToLower());


            if (portfolioModel == null) {
                return null;
            }

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();

            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.UserId == user.Id)
                .Select(stock => new Stock 
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                }).ToListAsync();
        }
    }
}