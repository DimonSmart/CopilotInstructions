# Blazor Patterns

## Component Design

### Focused component with explicit parameters and events

```razor
@* ProductCard.razor — single responsibility *@
<MudCard>
    <MudCardContent>
        <MudText Typo="Typo.h6">@Product.Name</MudText>
        <MudText>@Product.Price.ToString("C")</MudText>
    </MudCardContent>
    <MudCardActions>
        <MudButton OnClick="() => Selected.InvokeAsync(Product)">Select</MudButton>
    </MudCardActions>
</MudCard>

@code {
    [Parameter, EditorRequired] public ProductDto Product { get; set; } = null!;
    [Parameter] public EventCallback<ProductDto> Selected { get; set; }
}
```

### Cascading values for widely-shared context

```razor
@* Instead of drilling CurrentUser through every layer *@
<CascadingValue Value="@currentUser">
    <AppLayout />
</CascadingValue>

@* Any descendant accesses it without parameters: *@
@code {
    [CascadingParameter] private UserDto? CurrentUser { get; set; }
}
```

## State Management

### DI service for cross-component mutable state

```csharp
// Register as Scoped for Blazor Server, Singleton for pure WASM
public sealed class CartState
{
    private readonly List<CartItem> _items = [];
    public IReadOnlyList<CartItem> Items => _items;
    public event Action? Changed;

    public void Add(CartItem item)
    {
        _items.Add(item);
        Changed?.Invoke();
    }
}
```

```razor
@inject CartState Cart
@implements IDisposable

@code {
    protected override void OnInitialized() => Cart.Changed += StateHasChanged;
    public void Dispose() => Cart.Changed -= StateHasChanged;
}
```

### Persisting state across the prerender boundary (.NET 8+)

```razor
@code {
    [SupplyParameterFromPersistentComponentState]
    private ProductListDto? Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Products ??= await ProductService.GetAllAsync();
    }
}
```

## MudBlazor Integration

### Keep wrapper parameter names aligned with MudBlazor API

```razor
@* MudBlazor uses Visible/VisibleChanged — mirror that in your wrapper *@
<MyConfirmDialog Visible="@showDialog"
                 VisibleChanged="@((v) => showDialog = v)"
                 OnConfirm="HandleConfirm" />

@code {
    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public EventCallback OnConfirm { get; set; }
}
```

### Prefer MudBlazor layout primitives over raw divs

```razor
@* Good: semantic layout — no custom CSS needed *@
<MudStack Row="true" Spacing="2" AlignItems="AlignItems.Center">
    <MudAvatar>@user.Initials</MudAvatar>
    <MudText>@user.DisplayName</MudText>
</MudStack>
```

## Interactivity

### JS interop only after render

```razor
@inject IJSRuntime JS

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JS.InvokeVoidAsync("initChart", chartRef);
    }
}
```

### Batch StateHasChanged — call once after all mutations

```razor
@code {
    private async Task LoadDataAsync(CancellationToken ct)
    {
        var data = await DataService.GetAsync(ct);
        Items = data;
        // One StateHasChanged after all state is set, not inside a loop
        StateHasChanged();
    }
}
```
