# CopilotInstructions Template

Spin up shared project-level guidance for both GitHub Copilot and Codex in minutes. Instead of duplicating the same standards in multiple files,
this template generates a single source of truth and thin entry points for each assistant.

## What's included

- A `dotnet new` template that scaffolds:
  - `docs/AI_RULES.md` (primary rules)
  - `.github/copilot-instructions.md` (Copilot pointer)
  - `AGENTS.md` (Codex pointer)
- Opinionated guidance on naming, structure, and code style.
- Optional switches:
  - `--no-codex` (skip `AGENTS.md`)
  - `--no-copilot` (skip `.github/copilot-instructions.md`)

## Quick start

1. Install the template once on your machine:

   ```bash
   dotnet new install DimonSmart.CopilotInstructions.Template
   ```

2. Generate instruction files in any repository:

   ```bash
   dotnet new copilot-instructions
   ```

## Optional generation examples

Generate only Copilot files:

```bash
dotnet new copilot-instructions --no-codex true
```

Generate only Codex files:

```bash
dotnet new copilot-instructions --no-copilot true
```

## Updating the template

When a new release ships with refined guidelines, reinstall the template and recreate files where you need the latest version:

```bash
dotnet new install DimonSmart.CopilotInstructions.Template --force
```

Run `dotnet new copilot-instructions` again in each repository that should adopt the update.

## Optional extras

- `docs/codex-prompts/` contains reusable prompt templates that can be copied into `~/.codex/prompts`.
- If you use GitHub Copilot coding agent hooks, consider adding `.github/hooks/hooks.json` in your target repository to enforce checks such as `dotnet test`.

## Why it matters

Both Copilot and Codex can follow the same standards without file duplication. This reduces maintenance overhead and keeps AI suggestions consistent
across tools and teams.

## Resources

- GitHub: https://github.com/DimonSmart/CopilotInstructions
- NuGet: https://www.nuget.org/packages/DimonSmart.CopilotInstructions.Template

## Feedback

Ideas for improving the instructions or template? Open an [issue](https://github.com/DimonSmart/CopilotInstructions/issues) or submit a
[pull request](https://github.com/DimonSmart/CopilotInstructions/pulls). Contributions are welcome.
