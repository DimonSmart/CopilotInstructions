# CopilotInstructions Template

Spin up project-level guidance for GitHub Copilot in minutes. Instead of assembling expectations and naming rules by hand, this template
creates a ready-to-use `copilot-instructions.md` file with a single command.

## What's included

- A `dotnet new` template that scaffolds `.github/copilot-instructions.md`.
- Opinionated guidance on naming, structure, and code style that Copilot can follow.
- An optional `--filename` parameter to generate instructions under a custom path.

## Quick start

1. Install the template once on your machine:

   ```bash
   dotnet new install DimonSmart.CopilotInstructions.Template
   ```

2. Generate the instruction file in any repository:

   ```bash
   dotnet new copilot-instructions
   ```

The `copilot-instructions.md` file appears in your project and is ready to share with the team.

## Updating the template

When a new release ships with refined guidelines, reinstall the template and recreate the file where you need the latest version:

```bash
dotnet new install DimonSmart.CopilotInstructions.Template --force
```

Run `dotnet new copilot-instructions` again in each repository that should adopt the update.

## Why it matters

GitHub Copilot reads `copilot-instructions.md` before composing suggestions. Clear conventions help Copilot produce accurate, consistent
completions—especially in larger teams and long-lived codebases.

## Resources

- GitHub: https://github.com/DimonSmart/CopilotInstructions
- NuGet: https://www.nuget.org/packages/DimonSmart.CopilotInstructions.Template

## Feedback

Ideas for improving the instructions or template? Open an [issue](https://github.com/DimonSmart/CopilotInstructions/issues) or submit a
[pull request](https://github.com/DimonSmart/CopilotInstructions/pulls). Contributions are welcome.
