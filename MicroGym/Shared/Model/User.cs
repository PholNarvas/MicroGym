namespace MicroGym.Shared.Model
{
    public class User
    {
        public int UserId { get; set; }
        public int MemberShipTypeID { get; set; }
        public string? MemberShipType { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Member";
        public bool IsActive { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }
        public string? Status { get; set; }

        public string? PaymentMethod { get; set; }
        public decimal? Price { get; set; }
        public DateTime? LastVisit { get; set; }
    }
}
