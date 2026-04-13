---
id: pack-analyzers
scope: [codex, copilot]
category: pack
requires: [core-rules]
---
# Analyzers pack

- Fix analyzer findings at the root cause before considering suppression.
- Prefer small, local suppressions over broad project-wide disables.
- Do not change analyzer configuration without a clear repo policy reason.
- Keep code style changes separate from behavior changes when possible.
- When a rule becomes project policy, document it near the configuration.
