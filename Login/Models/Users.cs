using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Login.Models
{
    public class Users
    {
        [Key]
        //[JsonIgnore]
        public int UserId { get; set; }
       [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Username can only contain letters and spaces.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        //[JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public int IsActive { get; set; } = 1;
    }
}
