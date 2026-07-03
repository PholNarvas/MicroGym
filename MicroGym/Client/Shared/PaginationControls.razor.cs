using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class PaginationControls
    {
        [Parameter] public int CurrentPage { get; set; }
        [Parameter] public int TotalPages  { get; set; }
        [Parameter] public int TotalItems  { get; set; }
        [Parameter] public int PageSize    { get; set; }

        [Parameter] public EventCallback<int> OnPageChanged { get; set; }
    }
}
