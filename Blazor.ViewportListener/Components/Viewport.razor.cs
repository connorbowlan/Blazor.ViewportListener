using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.ViewportListener;

public partial class Viewport : IAsyncDisposable
{
    private DotNetObjectReference<Viewport>? _dotNetObjectReference;

    private ViewportInfo? _viewportInfo;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Debug { get; set; }

    [Parameter]
    public ViewportSize[] Sizes { get; set; } = [];

    [Parameter]
    public EventCallback<ViewportInfo> ViewportChanged { get; set; }

    private ViewportSize? CurrentViewportSize
    {
        get
        {
            if (_viewportInfo == null)
            {
                return null;
            }

            return _viewportInfo.Width switch
            {
                < 576 => ViewportSize.ExtraSmall,
                < 768 => ViewportSize.Small,
                < 992 => ViewportSize.Medium,
                < 1200 => ViewportSize.Large,
                < 1400 => ViewportSize.ExtraLarge,
                _ => ViewportSize.ExtraExtraLarge
            };
        }
    }

    private bool ShouldShow
    {
        get
        {
            if (CurrentViewportSize.HasValue)
            {
                return Sizes.Contains(CurrentViewportSize.Value);
            }

            return false;
        }
    }

    [Inject]
    private ViewportListenerService ViewportListenerService { get; set; } = null!;

    public async ValueTask DisposeAsync()
    {
        await ViewportListenerService.RemoveListenerAsync(_dotNetObjectReference);
        _dotNetObjectReference?.Dispose();
        GC.SuppressFinalize(this);
    }

    [JSInvokable]
    public async Task HandleViewportChangedAsync()
    {
        _viewportInfo = await ViewportListenerService.GetViewportInfoAsync();

        StateHasChanged();

        await ViewportChanged.InvokeAsync(_viewportInfo);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Debug)
            {
                Console.WriteLine($"Component {nameof(Viewport)} initialized.");
            }

            _viewportInfo = await ViewportListenerService.GetViewportInfoAsync();

            if (Debug)
            {
                Console.WriteLine("ViewportInfo initialized.");
            }

            _dotNetObjectReference = DotNetObjectReference.Create(this);

            await ViewportListenerService.AddListenerAsync(_dotNetObjectReference, nameof(HandleViewportChangedAsync));

            if (Debug)
            {
                Console.WriteLine("ViewportInfo initialized.");
            }

            StateHasChanged();
        }
    }
}