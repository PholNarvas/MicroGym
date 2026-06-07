namespace MicroGym.Shared.Model
{
    public class User
    {
        public int UserId { get; set; }
        public int MemberID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Member";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
