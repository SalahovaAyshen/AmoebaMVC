using Amoeba.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Amoeba.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetSettingAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
            return settings;
        }
    }
}
