using System.ComponentModel.DataAnnotations;

namespace MicroGym.Shared.DTOs
{
    public class AddExistingMemberDto
    {
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

        [Required(ErrorMessage = "Please select a payment method.")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal? PaymentAmount { get; set; }

        // When set, activates annual/tier membership on this user
        public int? TierID { get; set; }

        // Historical dates from logbook
        [Required(ErrorMessage = "Date joined is required.")]
        public DateTime DateJoined { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Expiry date is required.")]
        public DateTime ExpiryDate { get; set; } = DateTime.Today.AddMonths(1);

        // Annual membership tier dates (only used when TierID is set)
        public DateTime? TierExpiryDate { get; set; }
    }
}
