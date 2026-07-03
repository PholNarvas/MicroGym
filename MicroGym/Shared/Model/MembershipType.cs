namespace MicroGym.Shared.Model
{
    public class MembershipType
    {
        public int     MembershipTypeID  { get; set; }
        public string  Name              { get; set; } = string.Empty;
        public decimal Price             { get; set; }
        public int?    DurationInMonths  { get; set; }
        public bool    IsWalkIn          { get; set; }
    }
}
