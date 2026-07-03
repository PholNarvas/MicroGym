using MicroGym.Client.Service;
using MicroGym.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication : ComponentBase
    {
        [Inject]
        public MemberService MemberService { get; set; } = default!;
        [Inject]
        public MembershipTypeClientService MembershipTypeService { get; set; } = default!;
        [Inject]
        public AttendanceClientService AttendanceClientService { get; set; } = default!;
        [Inject]
        public RevenueService RevenueService { get; set; } = default!;
        [Inject]
        public AuthClientService AuthClientService { get; set; } = default!;
        [Inject]
        public ModalService ModalService { get; set; } = default!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    }
}
