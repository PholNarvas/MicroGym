using MicroGym.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace MicroGym.Client.Shared
{
    public partial class MainLayout : IDisposable
    {
        [Inject] private ThemeService ThemeService { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager NavManager { get; set; } = default!;

        private bool isNavigating = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var savedTheme = await JS.InvokeAsync<string>("microgym.getTheme");
                ThemeService.SetTheme(savedTheme);

                ThemeService.OnThemeChanged += HandleThemeChanged;
                NavManager.LocationChanged += OnLocationChanged;
            }
        }

        private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            isNavigating = true;
            await InvokeAsync(StateHasChanged);

            // Show the bar for 700ms — enough to cover data fetching on most pages
            await Task.Delay(700);

            isNavigating = false;
            await InvokeAsync(StateHasChanged);
        }

        private async void HandleThemeChanged()
        {
            await JS.InvokeVoidAsync("microgym.setTheme", ThemeService.Theme);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            ThemeService.OnThemeChanged -= HandleThemeChanged;
            NavManager.LocationChanged -= OnLocationChanged;
        }
    }
}
