---
name: blazor
description: Build and review Blazor components with correct render mode selection, component design, state management, and MudBlazor integration.
compatibility: Requires a Blazor project (.NET 8+).
---

# Blazor

## When to Use

- Adding or modifying Blazor components (`.razor` files)
- Choosing or changing a component render mode
- Designing component hierarchies, state flow, or event handling
- Wrapping or extending MudBlazor components
- Debugging prerendering, hydration, or JS interop issues

## When Not to Use

- Pure Razor Pages or MVC views with no Blazor components
- Static HTML/CSS changes with no component logic
- Server-side ASP.NET Core endpoints (use aspnet skill)
- MAUI Blazor Hybrid-specific patterns

## Inputs

| Input | Required | Description |
|-------|----------|-------------|
| Component intent | Yes | What the component should render or do |
| Render mode | No | Server, WebAssembly, Auto, or Static |
| UI library | No | MudBlazor, Radzen, or plain HTML |

## Workflow

1. Choose the render mode that matches the interactivity and connectivity requirements.
2. Design small, single-responsibility components with explicit `[Parameter]` and `EventCallback`.
3. Keep all logic except trivial helpers out of `.razor` markup — put it in `@code` or an injected service.
4. For widely-shared state, avoid parameter drilling — use `CascadingValue` or a scoped DI service.
5. Validate behavior in both the prerender phase and the interactive phase for Server or Auto modes.
6. Use `OnAfterRenderAsync(firstRender)` for JS interop, not `OnInitializedAsync`.

## Documentation

- [Blazor overview](https://learn.microsoft.com/aspnet/core/blazor/)
- [Render modes (.NET 8+)](https://learn.microsoft.com/aspnet/core/blazor/components/render-modes)
- [Component lifecycle](https://learn.microsoft.com/aspnet/core/blazor/components/lifecycle)
- [State management](https://learn.microsoft.com/aspnet/core/blazor/state-management)
- [JS interop](https://learn.microsoft.com/aspnet/core/blazor/javascript-interoperability/)
- [MudBlazor docs](https://mudblazor.com/docs/overview)

### References

- [patterns.md](../../../docs/ai-skills/blazor/references/patterns.md) — Component patterns, state strategies, MudBlazor integration
- [anti-patterns.md](../../../docs/ai-skills/blazor/references/anti-patterns.md) — Common Blazor mistakes and corrections

## Render Mode Quick Reference

| Mode | Runs Where | Best For |
|------|------------|----------|
| `Static` | Server only | SEO pages, read-only content |
| `InteractiveServer` | Server via SignalR | Real-time, thin clients |
| `InteractiveWebAssembly` | Browser (WASM) | Offline-capable, client-heavy UI |
| `InteractiveAuto` | Server first → WASM | Best of both worlds |

## Validation

- Render mode matches the interactivity and connectivity requirement.
- No business logic hidden inside `.razor` markup.
- MudBlazor wrapper parameter names align with the upstream MudBlazor API (`Visible`, not `IsVisible`).
- No unnecessary state duplication across parent, child, and services.
- JS interop only runs in `OnAfterRenderAsync`, not during prerender.
