using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class GymModal
    {
        [Parameter]
        public bool IsOpen { get; set; }
        [Parameter]
        public string Title { get; set; } = string.Empty;
        [Parameter]
        public string? Subtitle { get; set; }
        [Parameter]
        public string? MaxWidth { get; set; }
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        [Parameter]
        public EventCallback OnClose { get; set; }

        private async Task HandleClose() => await OnClose.InvokeAsync();
    }
}
