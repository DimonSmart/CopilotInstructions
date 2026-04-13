---
id: pack-aspnet
scope: [codex, copilot]
category: pack
requires: [dotnet-rules]
applies-to: "**/*.cs"
---
# ASP.NET pack

- Keep endpoints and controllers thin.
- Move business rules out of HTTP handlers and into services.
- Validate request data at the boundary.
- Return standard ASP.NET results that make status codes and payload shapes obvious.
- Use the request `CancellationToken` from ASP.NET entry points and pass it through to data access and outbound calls.
