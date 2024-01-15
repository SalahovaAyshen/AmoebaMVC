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
        public async Task<IActionResult> Index(int key = 1 )
        {
            ICollection<Service> service = await _context.Services.ToListAsync();
            ICollection<Portfolio> portfolios;

            switch (key)
            {
                case 2:
                    portfolios = await _context.Portfolios.OrderByDescending(p=>p.Id).Take(8).ToListAsync();
                    break;
                default:
                   portfolios = await _context.Portfolios.Take(8).ToListAsync();
                    break;
            }
            HomeVM homeVM = new HomeVM
            {
                Services = service,
                Portfolios = portfolios,
            };
            return View(homeVM);
        }

    }
}
