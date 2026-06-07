namespace MicroGym.Shared.Model
{
    public class Members
    {
        public int MemberID { get; set; }
        public int MemberShipTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateJoined { get; set; }
        public string Status { get; set; }
    }
}
