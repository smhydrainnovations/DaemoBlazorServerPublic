using System.ComponentModel.DataAnnotations;

namespace DaemoNETDemo.DTO
{
    public class RegistrationViaHttpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        // Additional fields for ApplicationUser
        [Required]
       
        public string FirstName { get; set; } = "";

        [Required]
      
        public string LastName { get; set; } = "";

        [Required]
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

      
        public string JobTitle { get; set; }

       
        public string Department { get; set; }

      
        public string Role { get; set; } = "";

       
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        
        public string Address { get; set; } = "";

        [Phone]
      
        public string PhoneNumber { get; set; } = "";
    }
}
