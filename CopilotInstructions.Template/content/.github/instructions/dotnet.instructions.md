---
applyTo: "**/*.cs"
id: instr-dotnet
requires: [dotnet-rules]
description: "C# file-level Copilot instructions"
---

- Prefer descriptive names, especially `*Id` parameters.
- Keep comments for intent, not narration.
- Use guard clauses instead of deep nesting.
- Keep async flows async and pass `CancellationToken`.
