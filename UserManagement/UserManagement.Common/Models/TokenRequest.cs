using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Common.Models
{
    public class TokenRequest
    {
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [JsonProperty("username")]
        public string Username { get; set; }


        [Required(ErrorMessage = "The Password  is required")]
        [DataType(DataType.Password,ErrorMessage = "Password must be at least 8 characters long and contain a number and contain one uppercase letter and one lowercase letter and one special character")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
