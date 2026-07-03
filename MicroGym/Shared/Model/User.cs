namespace MicroGym.Shared.Model
{
    public class User
    {
        public int      UserId           { get; set; }
        public int      MemberShipTypeID { get; set; }
        public string?  MemberShipType   { get; set; }
        public string   FirstName        { get; set; } = string.Empty;
        public string   LastName         { get; set; } = string.Empty;
        public string   Email            { get; set; } = string.Empty;
        public string   PasswordHash     { get; set; } = string.Empty;
        public string   Role             { get; set; } = "Member";
        public bool     IsActive         { get; set; }
        public string?  Phone            { get; set; }
        public DateTime  CreatedAt       { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate      { get; set; }
        public string?   ExpiryStatus    { get; set; }
        public string?   PaymentMethod   { get; set; }
        public decimal?  Price           { get; set; }
        public DateTime? LastVisit        { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string?   ProfilePicture  { get; set; }

        // Tier info — populated from sp_GetAllUsers JOIN with MembershipTiers
        public int?      TierID          { get; set; }
        public DateTime? TierExpiryDate  { get; set; }
        public decimal   TierDiscountPct { get; set; } = 0;

        // Populated by pr_GetMembersExpiring7Days — what needs attention
        // Values: 'Subscription', 'Annual', 'Both'
        public string?   AlertType       { get; set; }
        public int?      DaysUntilExpiry { get; set; }

        // Populated only by pr_GetUserByID (single-user queries, e.g. MemberProfileDrawer)
        public decimal TotalPaid    { get; set; } = 0;
        public int     RenewalCount { get; set; } = 0;

        // True when the member has a paid active tier (e.g. Annual)
        public bool IsAnnualMember =>
            TierID.HasValue &&
            TierExpiryDate.HasValue &&
            TierExpiryDate.Value.Date >= DateTime.Today &&
            TierDiscountPct > 0;
    }
}
