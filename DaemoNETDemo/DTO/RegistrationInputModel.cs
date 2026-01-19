using System.ComponentModel.DataAnnotations;

namespace DaemoNETDemo.DTO
{
    public class RegistrationInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        // Additional fields for ApplicationUser
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Job Title")]
        public int SelectedJobTitleId { get; set; }

        [Display(Name = "Department")]
        public int SelectedDepartmentId { get; set; }

        [Display(Name = "Role")]
        public string SelectedRole { get; set; } = "";

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; } = "";

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = "";
    }
}
