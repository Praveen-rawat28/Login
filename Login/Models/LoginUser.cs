using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class LoginUser
    {
        [Key]
        
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
//public IActionResult GetUser()
//{
//    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

//    if (userIdClaim == null)
//    {
//        return BadRequest("User  ID not found in token.");
//    }


//    Console.WriteLine($"User  ID Claim Value: {userIdClaim.Value}");


//    if (!int.TryParse(userIdClaim.Value, out int userId))
//    {


//        return BadRequest("Invalid User ID format.");
//    }


//    var user = _context.Users.FirstOrDefault(x => x.UserId == userId);

//    if (user != null)
//    {
//        return Ok(user);
//    }
//    else
//    {
//        return NoContent();
//    }
//}