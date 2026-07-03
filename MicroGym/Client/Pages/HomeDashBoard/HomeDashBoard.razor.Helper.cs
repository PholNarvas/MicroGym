using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.HomeDashBoard
{
    public partial class HomeDashBoard
    {
        private bool MatchesAlertSearch(User member, string search)
        {
            if (string.IsNullOrEmpty(search)) return true;

            return member.FirstName.ToLower().Contains(search)
                || member.LastName.ToLower().Contains(search)
                || member.Email.ToLower().Contains(search);
        }

        // Home-specific expiry label — shows how long ago it expired instead of generic "EXPIRED"
        private string GetHomeExpiryTagText(int daysLeft)
        {
            if (daysLeft < 0)
            {
                var daysAgo  = Math.Abs(daysLeft);
                if (daysAgo < 7) return $"Expired {daysAgo}d ago";
                var weeksAgo = daysAgo / 7;
                return $"Expired {weeksAgo}w ago";
            }
            if (daysLeft == 0) return "Tomorrow";
            return $"{daysLeft}d left";
        }
    }
}
