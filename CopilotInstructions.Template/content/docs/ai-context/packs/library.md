---
id: pack-library
scope: [codex, copilot]
category: pack
requires: [dotnet-rules]
---
# Library pack

- Design public APIs for long-term clarity, not for internal convenience.
- Keep dependencies minimal and avoid application-specific assumptions.
- Document public types and members with concise XML comments.
- Prefer stable contracts over clever shortcuts or hidden behavior.
- Treat breaking API or behavioral changes as explicit decisions.
