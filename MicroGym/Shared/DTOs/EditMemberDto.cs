using System.ComponentModel.DataAnnotations;

namespace MicroGym.Shared.DTOs
{
    public class EditMemberDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please select a membership plan.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid membership plan.")]
        public int MemberShipTypeID { get; set; }

        public bool? IsActive { get; set; }
    }
}
