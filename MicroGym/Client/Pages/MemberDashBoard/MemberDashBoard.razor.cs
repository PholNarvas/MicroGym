using MicroGym.Client.Service;
using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages
{
    public partial class MemberDashBoard
    {
        [Inject]
        private MemberService MemberService { get; set; } = default!;

        private List<Members> allMembers = new();
        private List<Members> filteredMembers = new();
        private bool isLoading = true;

        private string searchText = string.Empty;
        private string statusFilter = string.Empty;

        private int totalMembers => allMembers.Count;
        private int activeMembers => allMembers.Count(m => m.Status == "Active");
        private int inactiveMembers => allMembers.Count(m => m.Status != "Active");
    }
}
