namespace MicroGym.Shared.Model
{
    public class MembershipTier
    {
        public int     TierID         { get; set; }
        public string  TierName       { get; set; } = string.Empty;
        public decimal Fee            { get; set; }
        public decimal DiscountPct    { get; set; }
        public int     DurationMonths { get; set; }
        public bool    IsActive       { get; set; }
    }
}
