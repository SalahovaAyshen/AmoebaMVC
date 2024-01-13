using System.ComponentModel.DataAnnotations;

namespace Amoeba.Areas.Manage.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Can't be empty")]
        [MinLength(3, ErrorMessage ="Can't be less than 3")]
        [MaxLength(25, ErrorMessage ="Can't be more than 25")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        [MinLength(3, ErrorMessage = "Can't be less than 3")]
        [MaxLength(25, ErrorMessage = "Can't be more than 25")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        [MinLength(4, ErrorMessage = "Can't be less than 4")]
        [MaxLength(25, ErrorMessage = "Can't be more than 25")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Can't be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
