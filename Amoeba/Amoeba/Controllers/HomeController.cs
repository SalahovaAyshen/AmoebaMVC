using Amoeba.DAL;
using Amoeba.Models;
using Amoeba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Service> service = await _context.Services.ToListAsync();
            ICollection<Portfolio> portfolios = await _context.Portfolios.ToListAsync();
            
            HomeVM homeVM = new HomeVM
            {
                Services = service,
                Portfolios = portfolios,
            };
            return View(homeVM);
        }
    }
}
