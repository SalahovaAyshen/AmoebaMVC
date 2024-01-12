using Amoeba.DAL;
using Amoeba.Models;
using Amoeba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;

        public PortfolioController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            Portfolio portfolio = await _context.Portfolios.Include(p=>p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if(portfolio is null) return NotFound();
            PortfolioVM portfolioVM = new PortfolioVM
            {
              Portfolio = portfolio,
            };
            return View(portfolioVM);
        }
    }
}
