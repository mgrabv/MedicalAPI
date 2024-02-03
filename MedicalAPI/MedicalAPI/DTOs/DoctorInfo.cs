using System.ComponentModel.DataAnnotations;

namespace MedicalAPI.DTOs
{
    public class DoctorInfo
    {
        [Required(ErrorMessage = "Doctor's FirstName is required")]
        [StringLength(100, ErrorMessage = "Doctor's FirstName cannot be longer than 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Doctor's LastName is required")]
        [StringLength(100, ErrorMessage = "Doctor's LastName cannot be longer than 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Doctor's Email is required")]
        [StringLength(100, ErrorMessage = "Doctor's Email cannot be longer than 100 characters.")]
        public string Email { get; set; }
    }
}
