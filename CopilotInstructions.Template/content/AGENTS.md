---
id: agents-entry
scope: [codex]
category: entry-point
requires: [ai-rules]
---
# Codex instructions

Primary rules: `docs/AI_RULES.md`.

When changing code:
- read `docs/ai-context/core.md` first
- read `docs/ai-context/dotnet.md` for C# and .NET changes
- apply matching skills from `.codex/skills/` when present
- apply every file in `docs/ai-context/packs/` when present
- use prompts from `docs/codex-prompts/` for task-specific guidance when present
- keep changes small and predictable
- run repo checks before finishing (see `docs/AI_RULES.md` for commands)

## User Overrides

Read `AGENTS.local.md` after this file if it exists.
Rules there take higher priority and are not overwritten by template updates.

Before doing non-trivial work, check if the current task contains a durable correction, preference, or rule.
If it does, record it in `AGENTS.local.md` — not here.
