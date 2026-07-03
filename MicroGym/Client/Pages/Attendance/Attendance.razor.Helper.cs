using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.Attendance
{
    // Page-specific helpers for the Attendance page.
    // Shared helpers (FormatExpiryDate, IsMembershipValid, etc.)
    // are available from BaseApplication.razor.Helper.cs
    public partial class Attendance
    {
        // Used by both ModalFilteredMembers and OnSearchInput.
        // Returns true when the member matches the search term.
        // Handles partial matches on first name, last name, or full name.
        private bool MatchesMemberSearch(User member, string search)
        {
            if (string.IsNullOrEmpty(search)) return true;

            return member.FirstName.ToLower().Contains(search)
                || member.LastName.ToLower().Contains(search)
                || $"{member.FirstName} {member.LastName}".ToLower().Contains(search);
        }
    }
}
