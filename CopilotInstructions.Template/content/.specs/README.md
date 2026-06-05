# Spec Guided Dev

Current spec documents live directly in this directory.

Archived historical documents live in `archive/`.

Temporary implementation deviations live in `deviations/`.

Use one increasing sequence for current and archived spec documents:

`NNNN.type-short-title.md`

Examples:

- `0001.spec-initial-product-shape.md`
- `0002.adr-rendering-architecture.md`
- `0003.spike-terminal-input-model.md`
- `archive/0004.spec-old-dialog-behavior.md`

Types:

- `spec` - durable product intent;
- `adr` - architectural or long-lived technical decision;
- `spike` - investigation result and recommendation;
- `deviation` - temporary implementation gap under `deviations/`.

There is no `task` spec type. Small local changes belong in commit messages
unless they really require a `spec`, `adr`, `spike`, or `deviation`.

Do not use recursive `.specs/**/*.md` as current product intent.
Use only numbered files directly under `.specs/`.

Read archived files only for explicit history questions, decision explanation,
replacement links, reconcile work, or index maintenance.

Read deviations only when current implementation state matters.

When product intent changes:

1. Move the old file from `.specs/` to `.specs/archive/`.
2. Create one or more new current documents in `.specs/` with new numbers.
3. In each new document, add `Replaces:` with the old document number.
4. Do not rewrite the semantic content of the archived document.

When implementation temporarily differs from a current specification, create or
update a deviation document under `deviations/`. Delete it when resolved.

Do not create `deviations/archive/` by default.

Existing spec documents may be edited in place for non-semantic cleanup.

Non-semantic cleanup preserves product behavior, domain contracts, scope,
non-goals, accepted decisions, constraints, and verification rules.

References use document numbers only, not filenames.

Use templates from `_templates`.

Before creating a new document, check the latest number across `.specs/` and
`.specs/archive/`.

Do not add a numbered document for micro-changes, typos, formatting, obvious
local fixes, small refactoring with no behavior or architecture meaning,
dependency patches with no project-specific decision, or changes that do not
affect the product as such.
