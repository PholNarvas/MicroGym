namespace MicroGym.Shared.Model
{
    public class RevenuePaymentDetail
    {
        public int      PaymentID     { get; set; }
        public int      UserID        { get; set; }
        public string   FirstName     { get; set; } = string.Empty;
        public string   LastName      { get; set; } = string.Empty;
        public string   PlanName      { get; set; } = string.Empty;
        public decimal  AmountPaid    { get; set; }
        public DateTime PaymentDate   { get; set; }
        public string   PaymentMethod { get; set; } = string.Empty;
        public string   Status        { get; set; } = string.Empty;
        public string   PaymentType   { get; set; } = string.Empty;
    }
}
