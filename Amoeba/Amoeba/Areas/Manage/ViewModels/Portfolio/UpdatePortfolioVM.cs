using Amoeba.Models;
using System.ComponentModel.DataAnnotations;

namespace Amoeba.Areas.Manage.ViewModels
{
    public class UpdatePortfolioVM
    {
        [Required(ErrorMessage = "Name can't be empty")]
        [MinLength(3, ErrorMessage = "Name length can't be less than 3")]
        [MaxLength(25, ErrorMessage = "Name length can't be more than 25")]

        public string Name { get; set; } = null!;
        public IFormFile? Photo { get; set; }
        public string Client { get; set; } = null!;
        public DateTime ProjectDate { get; set; }
        public string ProjectUrl { get; set; } = null!;
        public int CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public string? Detail { get; set; }
    }
}
