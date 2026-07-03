namespace MicroGym.Client.Services
{
    public class ThemeService
    {
        public string Theme { get; private set; } = "dark";
        public bool IsDark => Theme == "dark";

        public event Action? OnThemeChanged;

        public void SetTheme(string theme)
        {
            if (Theme == theme) return;
            Theme = theme;
            OnThemeChanged?.Invoke();
        }

        public void ToggleTheme()
        {
            SetTheme(IsDark ? "light" : "dark");
        }
    }
}
