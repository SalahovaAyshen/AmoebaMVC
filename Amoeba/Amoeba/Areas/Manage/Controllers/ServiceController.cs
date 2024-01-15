using Amoeba.Areas.Manage.ViewModels;
using Amoeba.DAL;
using Amoeba.Models;
using Amoeba.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = nameof(UserRole.Admin))]

    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int count = await _context.Services.CountAsync();
            ICollection<Service> services = await _context.Services.Skip((page-1)*3).Take(3).ToListAsync();
            PaginationVM<Service> pagination = new PaginationVM<Service>
            {
                TotalPage = Math.Ceiling((double)count / 3),
                CurrentPage = page,
                Items = services
            };
           return View(pagination);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM serviceVM)
        {
            if (!ModelState.IsValid) return View();
            Service service = new Service
            {
                Title = serviceVM.Title,
                Description = serviceVM.Description,
                Icon = serviceVM.Icon,
            };
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();
            UpdateServiceVM serviceVM = new UpdateServiceVM 
            { 
               Title = service.Title,
               Description = service.Description,
               Icon = service.Icon,
            };
            return View(serviceVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateServiceVM serviceVM)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();
            if(!ModelState.IsValid) return View(serviceVM);
            service.Title = serviceVM.Title;
            service.Description = serviceVM.Description;
            service.Icon = serviceVM.Icon;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
