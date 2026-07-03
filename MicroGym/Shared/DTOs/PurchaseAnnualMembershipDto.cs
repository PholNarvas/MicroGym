using System.ComponentModel.DataAnnotations;

namespace MicroGym.Shared.DTOs
{
    public class PurchaseAnnualMembershipDto
    {
        public int UserId { get; set; }

        public int TierID { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
