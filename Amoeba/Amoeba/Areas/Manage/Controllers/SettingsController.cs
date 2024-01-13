using Amoeba.Areas.Manage.ViewModels;
using Amoeba.DAL;
using Amoeba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Settings> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Settings settings = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (settings == null) return NotFound();
            UpdateSettingsVM settingsVM = new UpdateSettingsVM
            {
                Value = settings.Value,
            };
            return View(settingsVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingsVM settingsVM)
        {
            if (id <= 0) return BadRequest();
            Settings settings = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (settings == null) return NotFound();
            if (!ModelState.IsValid) return View(settingsVM); 
            settings.Value = settingsVM.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
