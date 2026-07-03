using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages
{
    // Page-specific helpers for MemberDashBoard.
    // Shared helpers like FormatLastVisit and IsMembershipValid
    // are available from BaseApplication.razor.Helper.cs
    public partial class MemberDashBoard
    {
        private bool MatchesSearch(User member, string search)
        {
            if (string.IsNullOrEmpty(search)) return true;

            return member.FirstName.ToLower().Contains(search)
                || member.LastName.ToLower().Contains(search)
                || member.Email.ToLower().Contains(search);
        }

        private bool MatchesStatus(User member)
        {
            if (string.IsNullOrEmpty(statusFilter)) return true;

            if (statusFilter == "Renewed")
                return member.LastPaymentDate.HasValue
                    && member.LastPaymentDate.Value.Date >= DateTime.Today.AddDays(-30);

            if (statusFilter == "Active")
                return member.ExpiryStatus == "Active";

            // Inactive/Expiry card — matches:
            // 1. No subscription set (null ExpiryDate)
            // 2. Subscription expired
            // 3. Annual membership expired
            if (statusFilter == "Inactive")
                return !member.ExpiryDate.HasValue
                    || member.ExpiryDate.Value.Date < DateTime.Today
                    || (member.TierExpiryDate.HasValue && member.TierExpiryDate.Value.Date < DateTime.Today);

            return false;
        }

        private IEnumerable<User> ApplySort(IEnumerable<User> query)
        {
            // Renewals always sort most-recently-renewed first — ignore sort controls
            if (statusFilter == "Renewed")
                return query.OrderByDescending(m => m.LastPaymentDate);

            return (sortField, sortAscending) switch
            {
                ("Name",      true)  => query.OrderBy(m => m.FirstName).ThenBy(m => m.LastName),
                ("Name",      false) => query.OrderByDescending(m => m.FirstName).ThenByDescending(m => m.LastName),
                ("Expiry",    true)  => query.OrderBy(m => m.ExpiryDate),
                ("Expiry",    false) => query.OrderByDescending(m => m.ExpiryDate),
                ("LastVisit", true)  => query.OrderBy(m => m.LastVisit),
                ("LastVisit", false) => query.OrderByDescending(m => m.LastVisit),
                ("DateJoined", true) => query.OrderBy(m => m.CreatedAt),
                ("DateJoined", false)=> query.OrderByDescending(m => m.CreatedAt),
                _                    => query.OrderBy(m => m.FirstName).ThenBy(m => m.LastName)
            };
        }
    }
}
