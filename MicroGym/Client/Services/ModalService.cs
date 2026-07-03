using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Services
{
    public class ModalService
    {
        public event Action<RenderFragment, string>? OnShow;
        public event Action? OnClose;

        public void Show(RenderFragment content, string title)
            => OnShow?.Invoke(content, title);

        public void Close()
            => OnClose?.Invoke();
    }
}
