# CopilotInstructions Template

NuGet template that scaffolds a reusable `copilot-instructions.md` file with common C# coding standards.

## Using the published template

```bash
# install from NuGet
dotnet new install DimonSmart.CopilotInstructions.Template

# create the instructions file in the current directory
dotnet new copilot-instructions --output .

# customize the generated filename
dotnet new copilot-instructions --output . --filename Copilot.md
```

## Development workflow

1. Update `CopilotInstructions.Template/content/copilot-instructions/copilot-instructions.md` when the guidance changes.
2. Adjust metadata in `CopilotInstructions.Template/content/copilot-instructions/.template.config/template.json` if you need new parameters or post actions.
3. Bump the package version in the release tag (see below) before publishing.

If you want to test a build before pushing to NuGet:

```bash
dotnet pack CopilotInstructions.Template/TemplatePack.csproj --configuration Release --output artifacts
```

The command produces a `.nupkg` in the `artifacts/` folder that you can install manually (for example, via `dotnet new install ./artifacts/*.nupkg`) before pushing it to NuGet.

## Releasing to NuGet

Publishing is automated through GitHub Actions. Create and push a tag following the `v*` pattern (for example, `v1.0.0`). The workflow will:

1. Pack the template with the version taken from the tag name (without the leading `v`).
2. Push the package to NuGet using the `NUGET_API_KEY` secret. Optionally, override the feed URL with a `NUGET_SOURCE` secret.

Make sure the repository secrets `NUGET_API_KEY` (and optionally `NUGET_SOURCE`) are configured before tagging a release.
