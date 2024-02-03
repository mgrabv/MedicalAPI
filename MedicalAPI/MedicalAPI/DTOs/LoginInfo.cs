using System.ComponentModel.DataAnnotations;

namespace MedicalAPI.DTOs
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "User's Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "User's Password is required.")]
        public string Password { get; set; }
    }
}
