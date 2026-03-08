using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Reader
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Membership number is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Membership number must be between 4 and 20 characters.")]
        public string MembershipNumber { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime MembershipStartDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Date)]
        public DateTime? MembershipExpiryDate { get; set; }

        [StringLength(250, ErrorMessage = "Address must not exceed 250 characters.")]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
