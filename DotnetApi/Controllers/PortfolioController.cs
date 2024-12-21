using DotnetApi.Extensions;
using DotnetApi.Interfaces;
using DotnetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers
{
    [Route("api/portfolios")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository) 
        {
            _userManager = userManager;
            _stockRepository = stockRepository; 
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
 
            System.Console.WriteLine($"my claims = {HttpContext.User.Identity}");
            System.Console.WriteLine($"my claims = {HttpContext.User.Claims.GetType}");
            
            return Ok(userPortfolio);
        } 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null) {
                return BadRequest("Stock not found");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);


            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) {
                return BadRequest("Cannot add same stock to portfolio");
            }

            var portfolioModel = new Portfolio()
            {
                StockId = stock.Id,
                UserId = appUser.Id,

            };

            await _portfolioRepository.CreateAsync(portfolioModel); 


            if (portfolioModel == null) {
                return StatusCode(500, "Could not create");
            }

            return Created();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1) {
                await _portfolioRepository.DeletePortfolio(appUser, symbol);
                
                return Ok();
            }
 
            return BadRequest("Stock not in your portfolio");
        }

    }
}