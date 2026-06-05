# CopilotInstructions Template

Generate compact shared AI guidance for GitHub Copilot and agent entry files. The template keeps one main entry file, moves details into short context files,
and stays lightweight enough for a normal `dotnet new` workflow.

## What's included

- A `dotnet new` template that scaffolds:
  - `docs/AI_RULES.md` (primary entry point)
  - `docs/ai-context/core.md` (design principles and general rules)
  - `docs/ai-context/dotnet.md` (C#/.NET rules)
  - `docs/ai-skills/*/references/*.md` (shared detailed skill references)
  - `docs/ai-context/packs/*.md` (selected optional packs)
  - `docs/ai-context/references/*.md` (short checklists)
  - `.agents/skills/*/SKILL.md` (agent project-local skills)
  - `.github/copilot-instructions.md` (Copilot entry point)
  - `.github/skills/*/SKILL.md` (Copilot project-local skills)
  - `AGENTS.md` (shared agent entry point)
- Optional path-specific GitHub Copilot instructions in `.github/instructions/`.
- Optional Spec Guided Dev files with `.specs`, archive, temporary deviations, index, templates, and project-local skills.
- Optional switches:
  - `--no-codex` (skip `AGENTS.md`, companion agent files, and agent/Claude skills)
  - `--no-copilot` (skip `.github/copilot-instructions.md`)
  - `--profile` (`default`, `library`, `aspnet`, `blazor`)
  - `--packs "tests;mcp"` (additional packs)
  - `--with-path-instructions true`
  - `--with-spec-guided-dev true`
  - `--with-worklog true` (legacy alias)

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
  spec-guided-dev-method.md    # with --with-spec-guided-dev true
  ai-context/
    core.md
    dotnet.md
    packs/
    references/
  ai-skills/
    nullable-attributes/
      references/

.agents/
  skills/

.claude/              # with --with-spec-guided-dev true
  skills/

.github/
  copilot-instructions.md
  instructions/
  skills/

.specs/                  # with --with-spec-guided-dev true
  _templates/
  archive/
  deviations/
  INDEX.md
  README.md

AGENTS.md
```

`docs/AI_RULES.md` stays the main entry point. `core.md` holds design principles and general rules, `dotnet.md` holds C#/.NET-specific rules,
`packs/` adds small project-specific context, `references/` keeps checklists out of the main file, and `docs/ai-skills/` keeps heavier skill references
shared by native project-local skills in `.agents/skills/` and `.github/skills/`.

## Profiles

- `default`: core guidance only.
- `library`: for reusable packages and public API design.
- `aspnet`: for HTTP-first ASP.NET services and web apps.
- `blazor`: for Razor component projects and MudBlazor-style UI work; includes the ASP.NET pack.

## Optional generation examples

Generate only Copilot files:

```bash
dotnet new copilot-instructions --no-codex true
```

Generate only agent entry files:

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

Generate Spec Guided Dev methodology and skills:

```bash
dotnet new copilot-instructions --with-spec-guided-dev true
```

This adds `.specs/`, `.specs/archive/`, `.specs/deviations/`, `.specs/INDEX.md`, spec document templates, and `spec-start`, `spec-import`, `spec-reconcile`, `spec-review`, and `spec-index` skills.

Spec Guided Dev is an AI-assisted development method where a living specification guides implementation without replacing engineering judgment.

In AI development, the key skill is no longer just writing code, but describing intent precisely enough that both humans and AI agents can act on it.

The specification is not a task list. It is the durable description of the product: behavior, contracts, architecture, constraints, non-goals, and verification rules. If the implementation is deleted, the product should be rebuildable from the specification.

Spec documents use `NNNN.type-short-title.md` directly under `.specs/` for current documents, and the same naming under `.specs/archive/` for archived history. File names do not include active or retired lifecycle markers.

Spec document types are `spec` for durable product intent, `adr` for an accepted decision and rationale, `spike` for an investigation result and recommendation, and `deviation` for temporary implementation gaps under `.specs/deviations/`.

Legacy alias:

```bash
dotnet new copilot-instructions --with-worklog true
```

The legacy alias generates the Spec Guided Dev files and exists only for migration compatibility.

Migration from existing worklog projects:

1. Move `.worklog/` to `.specs/`.
2. Move `.worklog/archive/` to `.specs/archive/`.
3. Replace `docs/worklog-method.md` with `docs/spec-guided-dev-method.md`.
4. Replace `docs/ai-context/packs/worklog.md` with `docs/ai-context/packs/spec-guided-dev.md`.
5. Rename `worklog-*` skills to `spec-*` skills.
6. Keep document numbers unchanged.
7. Do not change document meaning during migration.
8. Do not delete existing Outcome sections automatically.
9. Use `.specs/deviations/` only for temporary current implementation gaps.

## Updating the template

When a new release ships with refined guidelines, reinstall the template and recreate files where you need the latest version:

```bash
dotnet new install DimonSmart.CopilotInstructions.Template --force
```

Run `dotnet new copilot-instructions` again in each repository that should adopt the update.

## Optional extras

- `docs/codex-prompts/` contains reusable Codex prompt templates that can be copied into `~/.codex/prompts`.
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
