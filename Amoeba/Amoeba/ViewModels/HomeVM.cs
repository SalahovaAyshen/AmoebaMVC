using Amoeba.Models;

namespace Amoeba.ViewModels
{
    public class HomeVM
    {
        public ICollection<Service> Services { get; set; }
        public ICollection<Portfolio> Portfolios { get; set; }

    }
}
