---
id: pack-blazor
scope: [codex, copilot]
category: pack
requires: [dotnet-rules, pack-aspnet]
applies-to: "**/*.razor"
---
# Blazor pack

- Prefer small components with clear parameters and events.
- Keep conditional markup and loops shallow; move repeated or stateful rendering decisions into code when the `.razor` file becomes hard to scan.
- Do not move business logic into `.razor` markup without reason.
- When wrapping MudBlazor components, keep parameter names aligned with MudBlazor APIs, such as `Visible`, unless the wrapper adds a genuinely different concept.
- Avoid unnecessary state duplication between parent, child, and services.
