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
            if (!expiryDate.HasValue) return "No expiry set";

            var formatted = expiryDate.Value.ToString("MMMM dd, yyyy");

            return expiryDate.Value.Date < DateTime.Today
                ? $"Expired: {formatted}"
                : formatted;
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
    }
}
