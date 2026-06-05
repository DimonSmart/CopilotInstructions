using System.Diagnostics;
using System.Text;

var options = SmokeTestOptions.Parse(args);
var runner = new SmokeTestRunner(options);

try
{
    await runner.RunAsync();
    Console.WriteLine("Smoke test passed.");
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Smoke test failed: {exception.Message}");
    Console.Error.WriteLine(exception);
    Environment.ExitCode = 1;
}

sealed class SmokeTestRunner
{
    private readonly SmokeTestOptions _options;

    public SmokeTestRunner(SmokeTestOptions options)
    {
        _options = options;
    }

    public async Task RunAsync()
    {
        var repoRoot = ResolveRepoRoot(_options.RepositoryRoot);
        var templateProject = Path.Combine(repoRoot, "CopilotInstructions.Template", "TemplatePack.csproj");
        if (!File.Exists(templateProject))
        {
            throw new InvalidOperationException($"Template project was not found: {templateProject}");
        }

        var tempRoot = Path.Combine(Path.GetTempPath(), $"CopilotInstructions-Smoke-{Guid.NewGuid()}");
        var artifactsRoot = Path.Combine(tempRoot, "artifacts");
        var cliHome = Path.Combine(tempRoot, "dotnet-home");
        var scenariosRoot = Path.Combine(tempRoot, "generated");

        Directory.CreateDirectory(artifactsRoot);
        Directory.CreateDirectory(cliHome);
        Directory.CreateDirectory(scenariosRoot);

        var childEnvironment = CreateChildEnvironment(cliHome);

        try
        {
            await RunCommandAsync(
                "dotnet",
                ["pack", templateProject, "--configuration", _options.Configuration, "--output", artifactsRoot],
                repoRoot,
                childEnvironment);

            var packagePath = Directory
                .EnumerateFiles(artifactsRoot, "*.nupkg")
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .FirstOrDefault();

            if (packagePath is null)
            {
                throw new InvalidOperationException("Template package was not created.");
            }

            await RunCommandAsync(
                "dotnet",
                ["new", "install", packagePath],
                repoRoot,
                childEnvironment);

            var templateList = await RunCommandAsync(
                "dotnet",
                ["new", "list"],
                repoRoot,
                childEnvironment,
                printOutput: false);

            if (!templateList.StandardOutput.Contains("copilot-instructions", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Template short name was not registered.");
            }

            foreach (var scenario in CreateScenarios())
            {
                await ValidateScenarioAsync(repoRoot, scenariosRoot, scenario, childEnvironment);
            }
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    private static string ResolveRepoRoot(string? repositoryRoot)
    {
        var candidate = repositoryRoot is null
            ? Environment.CurrentDirectory
            : Path.GetFullPath(repositoryRoot);

        return Path.GetFullPath(candidate);
    }

    private static Dictionary<string, string?> CreateChildEnvironment(string cliHome)
    {
        var environment = Environment
            .GetEnvironmentVariables()
            .Cast<System.Collections.DictionaryEntry>()
            .ToDictionary(
                entry => (string)entry.Key,
                entry => entry.Value?.ToString(),
                StringComparer.Ordinal);

        environment["DOTNET_CLI_HOME"] = cliHome;
        environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        environment["DOTNET_NOLOGO"] = "1";
        environment["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1";
        environment["DOTNET_NEW_SKIP_UPDATE_CHECK"] = "1";

        return environment;
    }

    private static IReadOnlyList<Scenario> CreateScenarios() =>
    [
        new(
            "default",
            [],
            ExistingPaths:
            [
                "docs/AI_RULES.md",
                "docs/ai-context/core.md",
                "docs/ai-context/dotnet.md",
                "docs/ai-skills/nullable-attributes/references/nullable-attributes.md",
                "docs/ai-context/references/code-review-checklist.md",
                ".agents/skills/nullable-attributes/SKILL.md",
                ".github/copilot-instructions.md",
                ".github/skills/nullable-attributes/SKILL.md",
                "AGENTS.md",
            ],
            MissingPaths:
            [
                ".codex",
                ".claude",
                ".github/instructions",
                "docs/ai-context/packs",
                "docs/spec-guided-dev-method.md",
                ".specs",
            ],
            Contains:
            [
                new ContentExpectation(".github/copilot-instructions.md", "../docs/AI_RULES.md"),
                new ContentExpectation("AGENTS.md", "docs/AI_RULES.md"),
            ]),
        new(
            "no-copilot",
            ["--no-copilot", "true", "--with-path-instructions", "true"],
            ExistingPaths:
            [
                "docs/AI_RULES.md",
                "AGENTS.md",
                ".agents/skills/nullable-attributes/SKILL.md",
            ],
            MissingPaths:
            [
                ".github",
                ".agents/skills/spec-close/SKILL.md",
                ".claude/skills/spec-close/SKILL.md",
            ],
            Contains: []),
        new(
            "no-codex",
            ["--no-codex", "true"],
            ExistingPaths:
            [
                ".github/copilot-instructions.md",
                ".github/skills/nullable-attributes/SKILL.md",
            ],
            MissingPaths:
            [
                "AGENTS.md",
                "CLAUDE.md",
                "GEMINI.md",
                ".agents",
                ".claude",
            ],
            Contains: []),
        new(
            "with-spec-guided-dev",
            ["--with-spec-guided-dev", "true"],
            ExistingPaths:
            [
                "docs/spec-guided-dev-method.md",
                "docs/ai-context/packs/spec-guided-dev.md",
                ".specs/README.md",
                ".specs/INDEX.md",
                ".specs/archive",
                ".specs/deviations",
                ".specs/_templates/spec.md",
                ".specs/_templates/adr.md",
                ".specs/_templates/spike.md",
                ".specs/_templates/deviation.md",
                ".agents/skills/spec-import/SKILL.md",
                ".agents/skills/spec-index/SKILL.md",
                ".agents/skills/spec-start/SKILL.md",
                ".agents/skills/spec-reconcile/SKILL.md",
                ".agents/skills/spec-review/SKILL.md",
                ".claude/skills/spec-import/SKILL.md",
                ".claude/skills/spec-index/SKILL.md",
                ".claude/skills/spec-start/SKILL.md",
                ".claude/skills/spec-reconcile/SKILL.md",
                ".claude/skills/spec-review/SKILL.md",
                ".github/skills/spec-import/SKILL.md",
                ".github/skills/spec-index/SKILL.md",
                ".github/skills/spec-start/SKILL.md",
                ".github/skills/spec-reconcile/SKILL.md",
                ".github/skills/spec-review/SKILL.md",
            ],
            MissingPaths:
            [
                ".codex",
                ".worklog",
                ".specs/deviations/archive",
                ".specs/_templates/task.md",
                ".agents/skills/worklog-start/SKILL.md",
                ".claude/skills/worklog-start/SKILL.md",
                ".github/skills/worklog-start/SKILL.md",
                ".agents/skills/spec-close/SKILL.md",
                ".claude/skills/spec-close/SKILL.md",
                ".github/skills/spec-close/SKILL.md",
            ],
            Contains:
            [
                new ContentExpectation("docs/ai-context/packs/spec-guided-dev.md", "docs/spec-guided-dev-method.md"),
                new ContentExpectation("docs/spec-guided-dev-method.md", "Spec Guided Dev is an AI-assisted development method"),
                new ContentExpectation("docs/spec-guided-dev-method.md", "Why Specifications Are Numbered"),
                new ContentExpectation(".specs/_templates/spec.md", "## Specification"),
                new ContentExpectation(".specs/_templates/deviation.md", "Type: deviation"),
                new ContentExpectation(".specs/INDEX.md", "numbered spec documents are the source of truth"),
                new ContentExpectation(".agents/skills/spec-start/SKILL.md", ".specs/_templates/"),
                new ContentExpectation(".agents/skills/spec-start/SKILL.md", "NNNN.type-short-title.md"),
                new ContentExpectation(".agents/skills/spec-review/SKILL.md", "Potential semantic change hidden as cleanup"),
                new ContentExpectation(".agents/skills/spec-reconcile/SKILL.md", "Non-semantic cleanup needed"),
                new ContentExpectation(".agents/skills/spec-index/SKILL.md", ".specs/INDEX.md"),
            ]),
        new(
            "with-worklog-legacy",
            ["--with-worklog", "true"],
            ExistingPaths:
            [
                "docs/spec-guided-dev-method.md",
                "docs/ai-context/packs/spec-guided-dev.md",
                ".specs/README.md",
                ".specs/INDEX.md",
                ".specs/archive",
                ".specs/deviations",
                ".specs/_templates/spec.md",
                ".specs/_templates/adr.md",
                ".specs/_templates/spike.md",
                ".specs/_templates/deviation.md",
                ".agents/skills/spec-import/SKILL.md",
                ".agents/skills/spec-index/SKILL.md",
                ".agents/skills/spec-start/SKILL.md",
                ".agents/skills/spec-reconcile/SKILL.md",
                ".agents/skills/spec-review/SKILL.md",
                ".github/skills/spec-start/SKILL.md",
            ],
            MissingPaths:
            [
                ".worklog",
                "docs/worklog-method.md",
                "docs/ai-context/packs/worklog.md",
                ".agents/skills/worklog-start/SKILL.md",
            ],
            Contains:
            [
                new ContentExpectation("docs/spec-guided-dev-method.md", "Migration From Worklog"),
            ]),
        new(
            "with-spec-guided-dev-no-copilot",
            ["--with-spec-guided-dev", "true", "--no-copilot", "true"],
            ExistingPaths:
            [
                "docs/spec-guided-dev-method.md",
                ".specs/README.md",
                ".specs/INDEX.md",
                ".specs/archive",
                ".specs/deviations",
                ".specs/_templates/spec.md",
                ".agents/skills/spec-import/SKILL.md",
                ".agents/skills/spec-index/SKILL.md",
                ".agents/skills/spec-start/SKILL.md",
                ".agents/skills/spec-reconcile/SKILL.md",
                ".agents/skills/spec-review/SKILL.md",
                ".claude/skills/spec-import/SKILL.md",
                ".claude/skills/spec-index/SKILL.md",
                ".claude/skills/spec-start/SKILL.md",
                ".claude/skills/spec-reconcile/SKILL.md",
                ".claude/skills/spec-review/SKILL.md",
            ],
            MissingPaths:
            [
                ".github",
                ".worklog",
            ],
            Contains: []),
        new(
            "blazor-profile",
            ["--profile", "blazor"],
            ExistingPaths:
            [
                "docs/ai-context/packs/blazor.md",
                "docs/ai-context/packs/aspnet.md",
            ],
            MissingPaths:
            [
                "docs/ai-context/packs/library.md",
                "docs/ai-context/packs/tests.md",
            ],
            Contains: []),
        new(
            "combined",
            ["--profile", "aspnet", "--packs", "tests;mcp", "--with-path-instructions", "true"],
            ExistingPaths:
            [
                "docs/ai-context/packs/aspnet.md",
                "docs/ai-context/packs/tests.md",
                "docs/ai-context/packs/mcp.md",
                ".github/instructions/dotnet.instructions.md",
                ".github/instructions/tests.instructions.md",
                ".github/instructions/razor.instructions.md",
            ],
            MissingPaths:
            [
                "docs/ai-context/packs/blazor.md",
                "docs/ai-context/packs/library.md",
                "docs/ai-context/packs/analyzers.md",
            ],
            Contains:
            [
                new ContentExpectation(".github/copilot-instructions.md", ".github/instructions/"),
            ]),
    ];

    private static async Task ValidateScenarioAsync(
        string repoRoot,
        string scenariosRoot,
        Scenario scenario,
        IReadOnlyDictionary<string, string?> environment)
    {
        Console.WriteLine($"Validating scenario '{scenario.Name}'...");

        var outputPath = Path.Combine(scenariosRoot, scenario.Name);
        await RunCommandAsync(
            "dotnet",
            ["new", "copilot-instructions", ..scenario.Arguments, "--output", outputPath],
            repoRoot,
            environment);

        foreach (var relativePath in scenario.ExistingPaths)
        {
            AssertExists(outputPath, relativePath);
        }

        foreach (var relativePath in scenario.MissingPaths)
        {
            AssertNotExists(outputPath, relativePath);
        }

        foreach (var expectation in scenario.Contains)
        {
            AssertContains(outputPath, expectation.RelativePath, expectation.ExpectedText);
        }

        if (scenario.Name == "with-spec-guided-dev")
        {
            ValidateSpecGuidedDevMethodology(outputPath);
        }
    }

    private static void AssertExists(string root, string relativePath)
    {
        var path = Path.Combine(root, relativePath);
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            throw new InvalidOperationException($"Expected path to exist: {relativePath}");
        }
    }

    private static void AssertNotExists(string root, string relativePath)
    {
        var path = Path.Combine(root, relativePath);
        if (File.Exists(path) || Directory.Exists(path))
        {
            throw new InvalidOperationException($"Expected path to be absent: {relativePath}");
        }
    }

    private static void AssertContains(string root, string relativePath, string expectedText)
    {
        var path = Path.Combine(root, relativePath);
        var content = File.ReadAllText(path);
        if (!content.Contains(expectedText, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{relativePath}' to contain '{expectedText}'.");
        }
    }

    private static void AssertNotContains(string root, string relativePath, string unexpectedText)
    {
        var path = Path.Combine(root, relativePath);
        var content = File.ReadAllText(path);
        if (content.Contains(unexpectedText, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{relativePath}' not to contain '{unexpectedText}'.");
        }
    }

    private static void ValidateSpecGuidedDevMethodology(string outputPath)
    {
        AssertNotContains(outputPath, ".specs/_templates/spec.md", "\n## Outcome\n");
        AssertNotContains(outputPath, ".specs/_templates/adr.md", "\n## Outcome\n");
        AssertContains(outputPath, ".specs/_templates/spike.md", "## Result");
        AssertContains(outputPath, ".specs/_templates/spike.md", "## Recommendation");
        AssertContains(outputPath, ".specs/_templates/deviation.md", "Type: deviation");
        AssertNotExists(outputPath, ".specs/deviations/archive");

        foreach (var skillRoot in new[] { ".agents/skills", ".claude/skills", ".github/skills" })
        {
            AssertContains(outputPath, $"{skillRoot}/spec-review/SKILL.md", "Does the document have required sections for its type?");
            AssertContains(outputPath, $"{skillRoot}/spec-review/SKILL.md", "Are deviations temporary and separate from specs?");
            AssertContains(outputPath, $"{skillRoot}/spec-reconcile/SKILL.md", "Temporary deviation needed");
        }
    }

    private static async Task<CommandResult> RunCommandAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        IReadOnlyDictionary<string, string?> environment,
        bool printOutput = true)
    {
        Console.WriteLine($"> {fileName} {string.Join(" ", arguments.Select(QuoteArgument))}");

        var startInfo = new ProcessStartInfo(fileName)
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        startInfo.Environment.Clear();
        foreach (var entry in environment)
        {
            if (entry.Value is not null)
            {
                startInfo.Environment[entry.Key] = entry.Value;
            }
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Failed to start process '{fileName}'.");

        var standardOutputTask = process.StandardOutput.ReadToEndAsync();
        var standardErrorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var standardOutput = await standardOutputTask;
        var standardError = await standardErrorTask;
        var result = new CommandResult(process.ExitCode, standardOutput, standardError);

        if (printOutput)
        {
            PrintIfNotEmpty(standardOutput, Console.Out);
            PrintIfNotEmpty(standardError, Console.Error);
        }

        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Command failed with exit code {result.ExitCode}: {fileName} {string.Join(" ", arguments.Select(QuoteArgument))}{Environment.NewLine}{result.CombinedOutput}");
        }

        return result;
    }

    private static void PrintIfNotEmpty(string value, TextWriter writer)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            writer.Write(value);
            if (!value.EndsWith(Environment.NewLine, StringComparison.Ordinal))
            {
                writer.WriteLine();
            }
        }
    }

    private static string QuoteArgument(string argument)
    {
        if (argument.Length == 0)
        {
            return "\"\"";
        }

        return argument.Any(char.IsWhiteSpace) || argument.Contains('"')
            ? $"\"{argument.Replace("\"", "\\\"", StringComparison.Ordinal)}\""
            : argument;
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch
        {
            // Best-effort cleanup.
        }
    }
}

sealed record SmokeTestOptions(string Configuration, string? RepositoryRoot)
{
    public static SmokeTestOptions Parse(string[] args)
    {
        var configuration = "Release";
        string? repositoryRoot = null;

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];
            switch (argument)
            {
                case "--configuration":
                case "-c":
                    configuration = GetRequiredValue(args, ref index, argument);
                    break;

                case "--repo-root":
                    repositoryRoot = GetRequiredValue(args, ref index, argument);
                    break;

                default:
                    throw new ArgumentException($"Unknown argument: {argument}");
            }
        }

        return new SmokeTestOptions(configuration, repositoryRoot);
    }

    private static string GetRequiredValue(string[] args, ref int index, string optionName)
    {
        var valueIndex = index + 1;
        if (valueIndex >= args.Length)
        {
            throw new ArgumentException($"Missing value for {optionName}.");
        }

        index = valueIndex;
        return args[valueIndex];
    }
}

sealed record Scenario(
    string Name,
    string[] Arguments,
    string[] ExistingPaths,
    string[] MissingPaths,
    ContentExpectation[] Contains);

sealed record ContentExpectation(string RelativePath, string ExpectedText);

sealed record CommandResult(int ExitCode, string StandardOutput, string StandardError)
{
    public string CombinedOutput
    {
        get
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(StandardOutput))
            {
                builder.AppendLine("stdout:");
                builder.AppendLine(StandardOutput.TrimEnd());
            }

            if (!string.IsNullOrWhiteSpace(StandardError))
            {
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }

                builder.AppendLine("stderr:");
                builder.AppendLine(StandardError.TrimEnd());
            }

            return builder.ToString().TrimEnd();
        }
    }
}
