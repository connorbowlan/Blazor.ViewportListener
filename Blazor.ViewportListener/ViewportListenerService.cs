using Microsoft.JSInterop;

namespace Blazor.ViewportListener;

/// <summary>
/// Service to listen to viewport changes and manage viewport listeners.
/// </summary>
/// <param name="jsRuntime">The JavaScript runtime to use for interop calls.</param>
/// <param name="options">Optional configuration options for the viewport listener service.</param>
public class ViewportListenerService(
    IJSRuntime jsRuntime,
    ViewportListenerServiceOptions? options = null)
    : IAsyncDisposable
{
    /// <summary>
    /// Gets the configuration options for the viewport listener service.
    /// </summary>
    public ViewportListenerServiceOptions Options = options ?? new ViewportListenerServiceOptions();

    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.ViewportListener/interop.js")
            .AsTask());

    /// <summary>
    /// Adds a window resize listener to an instance of a <see cref="DotNetObjectReference{TValue}" />
    /// and delegates a callback method to trigger when the resize event occurs.
    /// </summary>
    /// <param name="dotNetComponent">The instance of the component.</param>
    /// <param name="callbackMethod">The name of the method to trigger when the resize event occurs.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public async ValueTask AddListenerAsync(DotNetObjectReference<Viewport>? dotNetComponent, string callbackMethod)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("ViewportListener.addListener", dotNetComponent, callbackMethod);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="ViewportListenerService"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;

            await module.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the current viewport information.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation,
    /// with a <see cref="ViewportInfo"/> result containing the viewport information.
    /// </returns>
    public async Task<ViewportInfo> GetViewportInfoAsync()
    {
        var module = await _moduleTask.Value;

        return await module.InvokeAsync<ViewportInfo>("ViewportListener.getViewportInfo");
    }

    /// <summary>
    /// Removes a window resize listener from an instance of a <see cref="DotNetObjectReference{TValue}" />.
    /// </summary>
    /// <param name="component">The instance of the component.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public async ValueTask RemoveListenerAsync(DotNetObjectReference<Viewport>? component)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("ViewportListener.removeListener", component);
    }
}