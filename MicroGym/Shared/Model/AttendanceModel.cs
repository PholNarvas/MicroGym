namespace MicroGym.Shared.Model
{
    public class AttendanceModel
    {
        public int AttendanceID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
