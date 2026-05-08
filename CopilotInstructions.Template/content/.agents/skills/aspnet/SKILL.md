---
name: aspnet
description: Build and review ASP.NET Core endpoints, minimal APIs, controllers, middleware, and services following thin-handler and boundary-validation patterns.
compatibility: Requires an ASP.NET Core project (.NET 6+, preferably .NET 8+).
---

# ASP.NET Core

## When to Use

- Adding or modifying HTTP endpoints (minimal API or controller-based)
- Writing request pipeline components (middleware, filters, endpoint filters)
- Configuring DI registrations, hosted services, or startup logic
- Handling authentication, authorization, or request validation at the HTTP boundary
- Adding health checks, background workers, or outbound HTTP clients

## When Not to Use

- Pure Blazor component logic with no server endpoint (use blazor skill)
- Database migrations or EF Core model-only changes
- Client-side JavaScript or CSS

## Inputs

| Input | Required | Description |
|-------|----------|-------------|
| Endpoint intent | Yes | What the endpoint or service should do |
| API style | No | Minimal API, controller, gRPC, SignalR |
| Auth requirements | No | Anonymous, JWT bearer, cookie, policy-based |

## Workflow

1. Keep handlers thin — validate input, call the service, map the result to HTTP.
2. Validate request data at the HTTP boundary before passing to domain logic.
3. Return typed results (`Results.*` or `IActionResult`) that make status code and shape explicit.
4. Accept `CancellationToken` from the request context and pass it through every async I/O call.
5. Register services with the correct lifetime: Singleton for stateless, Scoped for per-request, Transient for lightweight utilities.

## Documentation

- [Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Controllers](https://learn.microsoft.com/aspnet/core/mvc/controllers/actions)
- [Middleware](https://learn.microsoft.com/aspnet/core/fundamentals/middleware/)
- [Dependency injection](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection)
- [Authentication](https://learn.microsoft.com/aspnet/core/security/authentication/)
- [Results (minimal API)](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/responses)

### References

- [patterns.md](../../../docs/ai-skills/aspnet/references/patterns.md) — Endpoint, service, and middleware patterns
- [anti-patterns.md](../../../docs/ai-skills/aspnet/references/anti-patterns.md) — Common ASP.NET Core mistakes

## Validation

- Handler is thin: no business logic or direct data access.
- Input is validated before domain logic runs.
- `CancellationToken` is accepted and forwarded through all async calls.
- Response type makes status code and payload shape obvious at the call site.
- DI lifetime matches service state guarantees.
