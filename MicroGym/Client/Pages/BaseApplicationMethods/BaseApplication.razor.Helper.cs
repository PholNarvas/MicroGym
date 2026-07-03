using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Checks if a member's membership is still active based on their ExpiryDate.
        // Returns true if expiry date is today or in the future.
        public bool IsMembershipValid(User? member)
        {
            return member?.ExpiryDate.HasValue == true
                && member.ExpiryDate.Value.Date >= DateTime.Today;
        }

        // Formats an expiry date into a readable string.
        // Example: June 26, 2026  |  Expired: March 01, 2025  |  No expiry set
        public string FormatExpiryDate(DateTime? expiryDate)
        {
            if (!expiryDate.HasValue)
                return "No expiry set";

            var formatted = expiryDate.Value.ToString("MMMM dd, yyyy");

            return formatted;
        }

        // Example: "Today", "Yesterday", "3 days ago", "2 weeks ago"
        public string FormatLastVisit(DateTime? lastVisit)
        {
            if (lastVisit is null) return "—";

            var daysAgo = (DateTime.Today - lastVisit.Value.Date).Days;

            if (daysAgo == 0) return "Today";
            if (daysAgo == 1) return "Yesterday";
            if (daysAgo < 7) return $"{daysAgo} days ago";

            var weeksAgo = daysAgo / 7;
            return weeksAgo == 1 ? "1 week ago" : $"{weeksAgo} weeks ago";
        }

        // ── Pagination Helpers ───────────────────────────────────

        // Returns one page of items from a list.
        protected static List<T> GetPagedItems<T>(IEnumerable<T> source, int page, int pageSize)
            => source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // Returns the total number of pages needed for a list.
        protected static int TotalPages<T>(IEnumerable<T> source, int pageSize)
        {
            var count = source.Count();
            return count == 0 ? 1 : (int)Math.Ceiling(count / (double)pageSize);
        }
        public string GetExpiryTagClass(int daysLeft)
        {
            if (daysLeft <= 0) return "ant-tag-red";     // expired
            if (daysLeft <= 7) return "ant-tag-orange";  // expiring this week
            if (daysLeft <= 15) return "ant-tag-gold";    // expiring this month
            return "ant-tag-green";                       // plenty of time left
        }

        public string GetExpiryTagText(int daysLeft)
        {
            if (daysLeft < 0)  return "EXPIRED";
            if (daysLeft == 0) return "Tomorrow";
            return $"{daysLeft}d left";
        }

        public (string text, string css) GetAnnualLeft(User m)
        {
            if (m.TierID == null || !m.TierExpiryDate.HasValue)
                return ("—", string.Empty);

            var days = (m.TierExpiryDate.Value.Date - DateTime.Today).Days;

            if (days < 0)
                return ("EXPIRED", "ant-tag-red");

            if (days < 30)
                return ($"{days}d", "ant-tag-orange");

            var months = (int)Math.Floor(days / 30.4);
            return ($"{months}mo", "ant-tag-gold");
        }

        public string GetGreeting()
        {
            var h = DateTime.Now.Hour;
            if (h < 12) return "Good morning";
            if (h < 17) return "Good afternoon";
            return "Good evening";
        }
    }
}
