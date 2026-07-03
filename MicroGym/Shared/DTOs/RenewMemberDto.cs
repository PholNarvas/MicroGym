using System.ComponentModel.DataAnnotations;

namespace MicroGym.Shared.DTOs
{
    public class RenewMemberDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a membership plan.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid membership plan.")]
        public int MemberShipTypeID { get; set; }

        [Required(ErrorMessage = "Please enter the payment amount.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal PaymentAmount { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
