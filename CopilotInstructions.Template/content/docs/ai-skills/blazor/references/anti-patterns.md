# Blazor Anti-Patterns

## Component Design

### Monolithic components

**Problem:** One component owns search, filtering, pagination, cart, and user state — markup becomes unreadable and logic untestable.

```razor
@* BAD *@
@code {
    private List<Product> products = [];
    private Cart cart = new();
    private User? user;
    private string searchTerm = "";
    private int page = 1;
    // 400+ more lines...
}
```

**Fix:** Decompose into `ProductSearch`, `ProductGrid`, `CartSidebar` — each owning one responsibility.

### Parameter drilling

**Problem:** The same value passed through 4+ component levels creates invisible coupling and makes refactoring painful.

```razor
@* BAD *@
<Layout User="@user">
    <Sidebar User="@user">
        <Nav User="@user" />
    </Sidebar>
</Layout>
```

**Fix:** Use `CascadingValue` / `[CascadingParameter]` for broadly needed context.

## State

### State duplication

**Problem:** Keeping the same data in a parent parameter, a child field, and a DI service causes out-of-sync bugs.

**Fix:** Pick one owner. Pass as parameter for display-only data. Use a DI service for shared mutable state.

### Fire-and-forget event handlers

```razor
@* BAD: exceptions are silently swallowed; component may be disposed before the task ends *@
<MudButton OnClick="@(() => _ = LoadAsync())">Load</MudButton>
```

```razor
@* GOOD: async Task handler surfaces exceptions to the error boundary *@
<MudButton OnClick="@LoadAsync">Load</MudButton>

@code {
    private async Task LoadAsync()
    {
        Items = await DataService.GetAsync();
    }
}
```

## Render Modes

### Over-using InteractiveServer for static content

**Problem:** Using `InteractiveServer` for a marketing page wastes a SignalR connection and increases TTFB.

**Fix:** Use `Static` rendering for pages that have no interactivity.

### JS interop during prerender

```razor
@* BAD: throws NotSupportedException during server-side prerender *@
protected override async Task OnInitializedAsync()
{
    await JS.InvokeVoidAsync("initChart");
}
```

```razor
@* GOOD: runs only after the DOM is live *@
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
        await JS.InvokeVoidAsync("initChart");
}
```

### Forgetting the prerender phase with Auto mode

**Problem:** Logic that checks `OperatingSystem.IsBrowser()` or uses WASM-only APIs runs during the server-side prerender pass in Auto mode and throws.

**Fix:** Guard platform-specific code with `RendererInfo.IsInteractive` or move it to `OnAfterRenderAsync`.

## MudBlazor

### Inventing parameter names that differ from MudBlazor

**Problem:** A wrapper component exposes `IsVisible` instead of `Visible`, or `OnClose` instead of `OnClosed`, breaking API familiarity.

**Fix:** Keep wrappers aligned with MudBlazor naming unless the wrapper genuinely introduces a new concept.

### Nesting MudDialog inside MudDialog without a portal

**Problem:** Nested dialogs without `@rendermode` isolation cause z-index and backdrop stacking issues.

**Fix:** Each dialog should be rendered at the root `MudDialogProvider` level, not as a child inside another dialog's content.
