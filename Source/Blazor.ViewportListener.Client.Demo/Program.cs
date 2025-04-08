using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Blazor.ViewportListener.Client.Demo;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        await builder.Build().RunAsync();
    }
}