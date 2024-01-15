using Amoeba.Areas.Manage.ViewModels;
using Amoeba.DAL;
using Amoeba.Models;
using Amoeba.Utilities.Enums;
using Amoeba.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles =nameof(UserRole.Admin))]

    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page =1 )
        {
            int count = await _context.Portfolios.CountAsync();
            ICollection<Portfolio> portfolios = await _context.Portfolios.Skip((page - 1) * 3).Take(3).ToListAsync();
            PaginationVM<Portfolio> pagination = new PaginationVM<Portfolio>
            {
                TotalPage = Math.Ceiling((double)count / 3),
                CurrentPage = page,
                Items = portfolios
            };
            return View(pagination);
        }
        public async Task<IActionResult> Create()
        {
            CreatePortfolioVM portfolioVM = new CreatePortfolioVM();
            portfolioVM.Categories = await _context.Categories.ToListAsync();
            return View(portfolioVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePortfolioVM portfolioVM)
        {
            portfolioVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid) return View(portfolioVM);
            if(portfolioVM.CategoryId<0 || await _context.Categories.AnyAsync(c => c.Id != portfolioVM.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Not found category id");
                return View(portfolioVM);
            }
            if (!portfolioVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Wrong type");
                return View(portfolioVM);
            }
            if (!portfolioVM.Photo.ValidateSize(2*1024))
            {
                ModelState.AddModelError("Photo", "Wrong size");
                return View(portfolioVM);
            }

            string filename = await portfolioVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "portfolio");
            Portfolio portfolio = new Portfolio
            {
                Name = portfolioVM.Name,
                Client = portfolioVM.Client,
                ProjectDate = portfolioVM.ProjectDate,
                ProjectUrl = portfolioVM.ProjectUrl,
                CategoryId = portfolioVM.CategoryId,
                Detail = portfolioVM.Detail,
                Image = filename
            };
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Portfolio portfolio = await _context.Portfolios.FirstOrDefaultAsync(p=>p.Id==id);
            if(portfolio is null) return NotFound();
            UpdatePortfolioVM portfolioVM  = new UpdatePortfolioVM 
            {
                Name = portfolio.Name,
                Client = portfolio.Client,
                ProjectDate = portfolio.ProjectDate,
                ProjectUrl = portfolio.ProjectUrl,
                CategoryId = (int) portfolio.CategoryId,
                Detail= portfolio.Detail,
                Categories = await _context.Categories.ToListAsync()
            };
            return View(portfolioVM);   
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdatePortfolioVM portfolioVM)
        {
            portfolioVM.Categories = await _context.Categories.ToListAsync();
            if (id <= 0) return BadRequest();
            Portfolio portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
            if (portfolio is null) return NotFound();
            if (!ModelState.IsValid) return View();
            if(portfolioVM.Photo is not null)
            {
                if (!portfolioVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "Wrong type");
                    return View(portfolioVM);
                }
                if (!portfolioVM.Photo.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Photo", "Wrong size");
                    return View(portfolioVM);
                }
                string filename = await portfolioVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "portfolio");
                portfolio.Image.DeleteFile(_env.WebRootPath, "assets", "img", "portfolio");
                portfolio.Image = filename;

            }
            portfolio.Name = portfolioVM.Name;
            portfolio.Client = portfolioVM.Client;
            portfolio.Detail = portfolioVM.Detail;
            portfolio.ProjectDate = portfolioVM.ProjectDate;
            portfolio.ProjectUrl = portfolioVM.ProjectUrl;
            portfolio.CategoryId = portfolioVM.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Portfolio portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
            if (portfolio is null) return NotFound();
            portfolio.Image.DeleteFile(_env.WebRootPath, "assets", "img", "portfolio");
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
