# ASP.NET Core Patterns

## Minimal APIs

### Thin endpoint delegating to a service

```csharp
app.MapPost("/orders", async (
    CreateOrderRequest request,
    IOrderService orders,
    CancellationToken ct) =>
{
    var result = await orders.CreateAsync(request, ct);
    return Results.CreatedAtRoute("GetOrder", new { id = result.Id }, result);
})
.WithName("CreateOrder")
.Produces<OrderDto>(StatusCodes.Status201Created)
.ProducesValidationProblem();
```

### Boundary validation before domain logic

```csharp
app.MapPost("/products", async (
    CreateProductRequest request,
    IValidator<CreateProductRequest> validator,
    IProductService products,
    CancellationToken ct) =>
{
    var validation = await validator.ValidateAsync(request, ct);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var product = await products.CreateAsync(request, ct);
    return Results.Created($"/products/{product.Id}", product);
});
```

### Route groups for shared prefix, auth, and OpenAPI metadata

```csharp
var api = app.MapGroup("/api/v1")
    .RequireAuthorization()
    .WithOpenApi();

var products = api.MapGroup("/products");
products.MapGet("/", GetAllAsync);
products.MapGet("/{id:guid}", GetByIdAsync);
products.MapPost("/", CreateAsync);
```

## Controllers

### Thin action delegating to a service

```csharp
[HttpPost]
public async Task<IActionResult> Create(
    [FromBody] CreateOrderRequest request,
    CancellationToken ct)
{
    var result = await _orders.CreateAsync(request, ct);
    return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
}
```

## Dependency Injection

### Correct service lifetimes

| Lifetime | Use for |
|----------|---------|
| `Singleton` | Stateless services, caches, configuration wrappers |
| `Scoped` | Per-request state: `DbContext`, current user context |
| `Transient` | Lightweight, stateless utilities |

### HttpClient via IHttpClientFactory

```csharp
// Register — typed client
builder.Services.AddHttpClient<IExternalApiClient, ExternalApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApi:BaseUrl"]!);
});

// DO NOT: new HttpClient() anywhere; DO NOT: inject raw HttpClient
```

## Middleware

### Purpose-specific middleware class

```csharp
public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await next(context);
        logger.LogInformation(
            "Request {Method} {Path} completed in {Elapsed}ms",
            context.Request.Method,
            context.Request.Path,
            sw.ElapsedMilliseconds);
    }
}
```

## Error Handling

### ProblemDetails for consistent error responses

```csharp
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// GlobalExceptionHandler logs the exception and returns RFC 9457 ProblemDetails
// with a correlation ID — never exposes ex.Message to the caller
```
