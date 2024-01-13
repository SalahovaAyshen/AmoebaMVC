using System.ComponentModel.DataAnnotations;

namespace Amoeba.Areas.Manage.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Can't be empty")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
