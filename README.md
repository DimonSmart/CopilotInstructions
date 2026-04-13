# CopilotInstructions Template

Generate compact shared AI guidance for both GitHub Copilot and Codex. The template keeps one main entry file, moves details into short context files,
and stays lightweight enough for a normal `dotnet new` workflow.

## What's included

- A `dotnet new` template that scaffolds:
  - `docs/AI_RULES.md` (primary entry point)
  - `docs/ai-context/core.md` (design principles and general rules)
  - `docs/ai-context/dotnet.md` (C#/.NET rules)
  - `docs/ai-skills/*/references/*.md` (shared detailed skill references)
  - `docs/ai-context/packs/*.md` (selected optional packs)
  - `docs/ai-context/references/*.md` (short checklists)
  - `.codex/skills/*/SKILL.md` (Codex project-local skills)
  - `.github/copilot-instructions.md` (Copilot entry point)
  - `.github/skills/*/SKILL.md` (Copilot project-local skills)
  - `AGENTS.md` (Codex entry point)
- Optional path-specific GitHub Copilot instructions in `.github/instructions/`.
- Optional switches:
  - `--no-codex` (skip `AGENTS.md`)
  - `--no-copilot` (skip `.github/copilot-instructions.md`)
  - `--profile` (`default`, `library`, `aspnet`, `blazor`)
  - `--packs "tests;mcp"` (additional packs)
  - `--with-path-instructions true`

## Quick start

1. Install the template once on your machine:

   ```bash
   dotnet new install DimonSmart.CopilotInstructions.Template
   ```

2. Generate instruction files in any repository:

   ```bash
   dotnet new copilot-instructions
   ```

## Structure

```text
docs/
  AI_RULES.md
  ai-context/
    core.md
    dotnet.md
    packs/
    references/
  ai-skills/
    nullable-attributes/
      references/

.codex/
  skills/

.github/
  copilot-instructions.md
  instructions/
  skills/

AGENTS.md
```

`docs/AI_RULES.md` stays the main entry point. `core.md` holds design principles and general rules, `dotnet.md` holds C#/.NET-specific rules,
`packs/` adds small project-specific context, `references/` keeps checklists out of the main file, and `docs/ai-skills/` keeps heavier skill references
shared by native project-local skills in `.codex/skills/` and `.github/skills/`.

## Profiles

- `default`: core guidance only.
- `library`: for reusable packages and public API design.
- `aspnet`: for HTTP-first ASP.NET services and web apps.
- `blazor`: for Razor component projects and MudBlazor-style UI work.

## Optional generation examples

Generate only Copilot files:

```bash
dotnet new copilot-instructions --no-codex true
```

Generate only Codex files:

```bash
dotnet new copilot-instructions --no-copilot true
```

Generate Blazor guidance with test and MCP packs:

```bash
dotnet new copilot-instructions --profile blazor --packs "tests;mcp"
```

Generate GitHub Copilot path-specific instructions:

```bash
dotnet new copilot-instructions --with-path-instructions true
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

## Smoke test

Run the repository smoke test before publishing template changes:

```powershell
dotnet run .\scripts\SmokeTest-Template.cs
```

The smoke test is a file-based C# app. It packs the template, installs the resulting package, generates several scenarios, and validates that expected files and links are present.

## Resources

- GitHub: https://github.com/DimonSmart/CopilotInstructions
- NuGet: https://www.nuget.org/packages/DimonSmart.CopilotInstructions.Template

## Feedback

Ideas for improving the instructions or template? Open an [issue](https://github.com/DimonSmart/CopilotInstructions/issues) or submit a
[pull request](https://github.com/DimonSmart/CopilotInstructions/pulls). Contributions are welcome.
