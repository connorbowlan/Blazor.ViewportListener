using Microsoft.JSInterop;

namespace Blazor.ViewportListener;

public class ViewportListenerService(
    IJSRuntime jsRuntime,
    ViewportListenerServiceOptions? options = null)
    : IAsyncDisposable
{
    public ViewportListenerServiceOptions Options = options ?? new ViewportListenerServiceOptions();

    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.ViewportListener/interop.js")
            .AsTask());

    public async ValueTask AddListenerAsync(DotNetObjectReference<Viewport>? component, string callbackMethod)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("ViewportListener.addListener", component, callbackMethod);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;

            await module.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    public async Task<ViewportInfo> GetViewportInfoAsync()
    {
        var module = await _moduleTask.Value;

        return await module.InvokeAsync<ViewportInfo>("ViewportListener.getViewportInfo");
    }

    public async ValueTask RemoveListenerAsync(DotNetObjectReference<Viewport>? component)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("ViewportListener.removeListener", component);
    }
}