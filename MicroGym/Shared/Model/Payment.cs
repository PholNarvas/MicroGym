namespace MicroGym.Shared.Model
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public int MembershipTypeID { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
