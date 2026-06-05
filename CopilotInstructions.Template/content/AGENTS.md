---
id: agents-entry
scope: [codex, claude, gemini]
category: entry-point
requires: [ai-rules]
---
# Agent instructions

Primary rules: `docs/AI_RULES.md`.

When changing code:
- read `docs/ai-context/core.md` first
- read `docs/ai-context/dotnet.md` for C# and .NET changes
- apply matching skills from `.agents/skills/` when present
- apply every file in `docs/ai-context/packs/` when present
- keep changes small and predictable
- run repo checks before finishing (see `docs/AI_RULES.md` for commands)

## README writing style

When creating or editing `README.md` or `README*.md`, follow:

- `docs/style/readme-style.md`
- `docs/style/llm-antipatterns.md`
- `docs/style/readme-review-checklist.md`
- `docs/style/examples.md`

For README work, use:

- `.agents/skills/readme-human-style/SKILL.md`

Main rule: README text must be concrete, technical, and close to the author's personal engineering style. Avoid generic LLM writing and marketing tone.

## User Overrides

Read `AGENTS.local.md` after this file if it exists.
Rules there take higher priority and are not overwritten by template updates.

Before doing non-trivial work, check if the current task contains a durable correction, preference, or rule.
If it does, record it in `AGENTS.local.md` — not here.
