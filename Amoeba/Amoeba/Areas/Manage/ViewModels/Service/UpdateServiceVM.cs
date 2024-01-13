using System.ComponentModel.DataAnnotations;

namespace Amoeba.Areas.Manage.ViewModels
{
    public class UpdateServiceVM
    {
        [Required(ErrorMessage = "Can't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Can't be empty")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Can't be empty")]

        public string Icon { get; set; }
    }
}
