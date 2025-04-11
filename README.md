# Blazor.ViewportListener

**Blazor.ViewportListener** is a lightweight utility for Blazor applications that enables components to respond to browser viewport size changes using JavaScript interop. It provides a `Viewport` component and a service to conditionally render UI based on customizable breakpoints.

## Features

- Tracks browser viewport `width` and `height` in real-time.
- Exposes a `Viewport` component for conditional rendering.
- Easily configurable responsive breakpoints.
- Minimal setup with clean Blazor integration.

## Installation

1. Install via NuGet:

```bash
dotnet add package Blazor.ViewportListener
```

2. Add the ViewportListenerService class to your DI container (in Program.cs):

```
builder.Services.AddScoped<ViewportListenerService>();
```

## Usage

Create an instance of the `Viewport` component in on a page or in another component and define the breakpoints via the `Sizes` parameter:

```
<Viewport Sizes="[ViewportSize.ExtraSmall, ViewportSize.Small, ViewportSize.Medium]">
	Show me in extra small, small, and medium!
</Viewport>

<Viewport Sizes="[ViewportSize.Large, ViewportSize.ExtraLarge, ViewportSize.ExtraExtraLarge]">
        Show me in large, extra large, and extra extra large!
</Viewport>
```