namespace MicroGym.Shared.Model
{
    public class MemberPaymentRecord
    {
        public int       PaymentID     { get; set; }
        public string    PlanName      { get; set; } = string.Empty;
        public string?   PlanType      { get; set; }
        public decimal   AmountPaid    { get; set; }
        public DateTime  PaymentDate   { get; set; }
        public string    PaymentMethod { get; set; } = string.Empty;
    }
}
