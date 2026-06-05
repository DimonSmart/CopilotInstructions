---
name: spec-import
description: Import existing specs/docs/planning documents or migrate an old `.worklog` tree into `.specs`. Use only when invoked manually by the user as /spec-import SOURCE_FOLDER.
---

# Spec import

You are helping migrate existing planning documents into Spec Guided Dev.

This skill is a manual command:

```text
/spec-import <SOURCE_FOLDER>
```

It has side effects: it may rename or move files. Do not run it automatically.

## Required argument

This command requires one positional argument:

- `SOURCE_FOLDER`: path to the folder to import.

The first positional argument is the only source folder.

`SOURCE_FOLDER` is mandatory.

Before doing any work, validate that `SOURCE_FOLDER` is present, non-empty, and clearly looks like a folder path or folder name.

If `SOURCE_FOLDER` does not exist or is inaccessible, respond with:

```text
The specified folder does not exist or cannot be accessed. Please provide a valid folder path.
```

Valid usage examples:

```text
/spec-import docs/specs
/spec-import docs/planning
/spec-import old-specifications
/spec-import .worklog
```

If the folder path contains spaces, the user must quote it:

```text
/spec-import "docs/old specs"
```

Invalid usage examples:

```text
/spec-import
/spec-import into .specs
/spec-import migrate docs/specs into .specs
```

In the last invalid example, `migrate` would be parsed as the first positional argument, so it must not be accepted as `SOURCE_FOLDER`.

If `SOURCE_FOLDER` is missing, empty, ambiguous, or only implied, ask the user to provide it and stop.

Use this exact response pattern:

```text
Please provide SOURCE_FOLDER, for example: /spec-import docs/specs
```

Do not scan, classify, rename, move, or edit any files until `SOURCE_FOLDER` is explicitly provided.

## Worklog migration

When `SOURCE_FOLDER` is `.worklog`, migrate paths without changing document meaning:

- `.worklog/*.md` to `.specs/*.md`
- `.worklog/archive/*.md` to `.specs/archive/*.md`
- `.worklog/_templates/*.md` to `.specs/_templates/*.md`
- `.worklog/INDEX.md` to `.specs/INDEX.md`
- `.worklog/README.md` to `.specs/README.md`

Keep document numbers unchanged.

Do not delete existing Outcome sections automatically.

Use `.specs/deviations/` only for temporary current implementation gaps.

## Validation and setup

After `SOURCE_FOLDER` is explicitly provided:

1. Validate that `SOURCE_FOLDER` is accessible and contains supported documents.
2. If invalid, stop and report the error.
3. If valid, create `.specs/`, `.specs/archive/`, and `.specs/deviations/` if they do not exist.
4. Then proceed to the Read phase.

## Read

After validation and setup, read:

1. `docs/spec-guided-dev-method.md`
2. `.specs/README.md` if it exists
3. existing numbered files in `.specs/` and `.specs/archive/`
4. the folder specified by `SOURCE_FOLDER`

If `docs/spec-guided-dev-method.md` does not exist, report that the methodology file is missing and continue only with the available `.specs` conventions.

## Task

1. Inventory supported source documents from `SOURCE_FOLDER`.
2. Include Markdown files by default.
3. Include plain text files only if the user explicitly asks to import plain text or `.txt` files.
4. Ignore generated files, binaries, images, archives, build artifacts, hidden dependency folders, and nested dependency folders.
5. Determine the next `NNNN` number after existing numbered `.specs` and `.specs/archive` documents.
6. For each source document, choose one type: `spec`, `adr`, or `spike`.
7. Create a short kebab-case title from the existing file name or document heading.
8. Rename or move each current document to `.specs/NNNN.type-short-title.md`.
9. Rename or move each document the user explicitly identifies as historical or replaced to `.specs/archive/NNNN.type-short-title.md`.
10. Preserve ordering by prioritizing explicit source numbering first, followed by dates. If neither is present, use stable path sort.
11. Report the old path to new path mapping.
12. After import, update `.specs/INDEX.md`.

## Supported source documents

Include by default:

- `*.md`
- `*.markdown`

Include only when explicitly requested by the user:

- `*.txt`

Ignore:

- generated files
- binaries
- images
- archives
- build artifacts
- dependency folders such as `node_modules`, `.nuget`, `packages`, `bin`, `obj`, `dist`, `build`, `.git`
- temporary files
- editor backup files
- files that are clearly not planning, specification, decision, or research documents
- small local task notes or fixes that belong in commit messages

## Classification

Use exactly one of these document types:

- `spec`: durable product intent, requirements, behavior descriptions, contracts, scenarios, or stable patterns.
- `adr`: decisions, alternatives, tradeoffs, accepted approaches, or library choices.
- `spike`: research notes, experiments, feasibility checks, or open technical questions.

If a type cannot be determined confidently, leave the file in place and ask the user for classification.

Do not invent a classification only to complete the import.

## Naming

Target file name format:

```text
.specs/NNNN.type-short-title.md
```

Where:

- `NNNN` is a four-digit sequence number.
- `type` is one of `spec`, `adr`, or `spike`.
- `short-title` is a kebab-case label; `type` and `short-title` are joined by a hyphen.
- The extension is `.md`.

Use `.specs/NNNN.type-short-title.md` for current product intent, decisions, and investigations.

Use `.specs/archive/NNNN.type-short-title.md` only for documents the user explicitly identifies as historical or replaced.

Examples:

```text
.specs/0007.spec-console-rendering.md
.specs/0008.adr-double-buffering.md
.specs/archive/0009.spike-terminal-flickering.md
```

If importing from an older worklog format:

- `NNNN.type-title.active.md` becomes `.specs/NNNN.type-title.md`
- `NNNN.type-title.retired.md` becomes `.specs/archive/NNNN.type-title.md`

## Move rules

- Do not overwrite existing `.specs` files.
- If a target path already exists, keep the sequence number unique and choose the next available `NNNN`.
- If two documents would produce the same short title, keep both by preserving their sequence numbers.
- Do not add vague suffixes unless needed to avoid a path collision.
- If a source document is already correctly named and already located in `.specs/` or `.specs/archive/`, leave it unchanged.
- Prefer `git mv` for tracked files.
- Use normal filesystem move only for untracked files.
- Do not edit document content unless the user explicitly asks to normalize sections.
- Do not require or normalize `Outcome`.
- If an imported legacy spec or ADR contains `Outcome`, preserve it during import unless the user explicitly asks for normalization.
- Do not create numbered spec documents for product-neutral small changes that belong in commit messages.
- Do not create numbered spec documents for micro-changes or changes fully explained by the diff and commit message.

## Safety rules

- `SOURCE_FOLDER` is required.
- Do not infer `SOURCE_FOLDER`.
- Do not scan, classify, rename, move, or edit source documents before `SOURCE_FOLDER` is explicitly provided.
- Do not move files outside the repository root unless the user explicitly provided an external path and confirmed that it should be used.
- Do not delete source documents.
- Do not overwrite files.
- Do not modify file contents unless the user explicitly asks for content normalization.
- Report every moved, skipped, and unresolved document.

## Missing input behavior

If the user does not provide `SOURCE_FOLDER`, respond only with:

```text
Please provide SOURCE_FOLDER, for example: /spec-import docs/specs
```

Do not continue the import workflow until the user provides `SOURCE_FOLDER`.

## Final report

After completing the import, report:

1. Imported documents.
2. Old path to new path mapping.
3. Skipped documents and the reason.
4. Documents that need manual classification, if any.
5. The next available `NNNN` number after the operation.

Use a compact table for path mappings.
