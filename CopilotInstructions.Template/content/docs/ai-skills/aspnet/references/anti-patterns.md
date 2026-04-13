# ASP.NET Core Anti-Patterns

## Endpoints and Controllers

### Fat handler — business logic inside the action

**Problem:** The action method accumulates queries, calculations, email sending, and logging. It becomes untestable and violates single responsibility.

```csharp
// BAD
[HttpPost]
public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
{
    // 80 lines: EF queries, price calculation, inventory check, email sending...
}
```

**Fix:** Extract to a service. The action validates input, calls the service, and maps the result to HTTP.

### Missing CancellationToken

**Problem:** Async endpoints that ignore request cancellation continue running after the client disconnects, wasting server resources.

```csharp
// BAD
[HttpGet("{id}")]
public async Task<IActionResult> Get(Guid id)
{
    var item = await _repo.GetByIdAsync(id); // no cancellation
    return Ok(item);
}
```

**Fix:** Accept `CancellationToken ct` in every async action and pass it to every I/O call.

### Swallowing exceptions with an empty catch

```csharp
// BAD: the caller gets 200 OK even on failure
try { await _service.DoSomethingAsync(); }
catch { }
```

**Fix:** Let exceptions propagate to global error-handling middleware (`IExceptionHandler`). Catch only specific, expected exceptions and return a meaningful `Results.Problem` response.

### Exposing internal exception details to callers

```csharp
// BAD: leaks stack trace and implementation details
return BadRequest(ex.Message);
```

**Fix:** Log the exception with a correlation ID; return `Results.Problem` with only a user-safe message and the correlation ID.

## Dependency Injection

### Scoped service captured inside a Singleton

**Problem:** Injecting `DbContext` (Scoped) into a Singleton captures a single request scope for the application's lifetime, causing data corruption and thread-safety bugs.

```csharp
// BAD
builder.Services.AddSingleton<IMyService, MyService>();
// MyService constructor: public MyService(AppDbContext db) { ... }
```

**Fix:** Inject `IServiceScopeFactory` and create a scope on demand, or change the DI lifetime to Scoped.

### Resolving Scoped services from the root container

```csharp
// BAD: leaks a Scoped service into the root container
var db = app.Services.GetRequiredService<AppDbContext>();
```

**Fix:** Use `app.Services.CreateScope()` and resolve from the child scope inside a `using` block.

## Middleware

### Too much logic in anonymous middleware lambdas

```csharp
// BAD: hard to test, hard to name, clutters Program.cs
app.Use(async (ctx, next) =>
{
    // 30 lines of auth, logging, header manipulation...
    await next();
});
```

**Fix:** Extract to a named middleware class with an `InvokeAsync` method.

### Middleware that modifies the response after `next()` on error paths

**Problem:** Checking `context.Response.StatusCode` after `await next(context)` is unreliable because headers may already be sent.

**Fix:** Use `IExceptionHandler` or exception-handling middleware that runs before headers are committed.

## Responses

### Returning ambiguous status codes

```csharp
// BAD: is null intentional? Is 200 correct?
return Ok(null);
```

**Fix:** Use `Results.NoContent()`, `Results.NotFound()`, or `Results.Ok(dto)` — make intent explicit.

### Blocking on async code

```csharp
// BAD: deadlocks under load on the thread pool
var result = _service.GetAsync().Result;
```

**Fix:** Make the calling method `async Task` and `await` through the entire call chain.
