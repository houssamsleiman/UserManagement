using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Common.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "The First Name  is required")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name  is required")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string LastName { get; set; }
         
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password  is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password )]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%#^*?&])[A-Za-z\\d$@$!%#^*?&].{8,}$",ErrorMessage = "Password must be at least 8 characters long and contain a number and contain one uppercase letter and one lowercase letter and one special character")]
        public string Password { get; set; }

        public string Token { get; set; }

        public Status Status { get; set; }

        public string PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
   
}