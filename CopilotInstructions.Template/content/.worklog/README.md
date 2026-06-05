# Worklog

Current work documents live directly in this directory.

Archived historical documents live in `archive/`.

Use one increasing sequence for all work documents:

`NNNN.type-short-title.md`

Examples:

- `0001.spec-initial-mvp.md`
- `0002.adr-console-rendering.md`
- `archive/0003.spike-old-renderer.md`

Draft documents are not committed to the main branch.

Types:

- `spec` - current intended behavior;
- `adr` - accepted decision and rationale;
- `spike` - investigation result and recommendation.

There is no `task` worklog type. Small local changes belong in commit messages unless they require a `spec`, `adr`, or `spike`.

Do not use recursive `.worklog/**/*.md` as current context.
Use only numbered files directly under `.worklog/`.

Read archived files only for explicit history questions, decision explanation, replacement links, reconcile work, or index maintenance.

When a current document needs substantial semantic changes:

1. Move the old file from `.worklog/` to `.worklog/archive/`.
2. Create one or more new current documents in `.worklog/` with new numbers.
3. In each new document, add `Replaces:` with the old document number.
4. Do not rewrite the semantic content of the archived document.

Existing work documents may be edited in place for non-semantic cleanup.

Non-semantic cleanup preserves required behavior, decisions, scope, non-goals, constraints, and done criteria.

Do not archive and replace a document only because duplicated behavior was extracted into a shared spec or wording was cleaned up.

References use document numbers only, not filenames.

Do not split documents into separate folders by type.

Use templates from `_templates`.

Before creating a new document, check the latest number.

Before implementation, make sure the document has:

- clear goal;
- scope;
- non-goals if applicable;
- done criteria;
- verification plan.

After significant implementation, reconcile the work document with the actual result.

If the current document still describes the intended behavior or accepted decision, leave it unchanged.

If the requirement or decision changed substantially, archive the old document and create a new current document with `Replaces:`.

Do not use work documents for implementation status, completion notes, routine verification output, or execution summaries. Do not archive a spec only because it was implemented.

Do not add a numbered document for micro-changes, typos, formatting, obvious local fixes, small refactoring with no behavior or architecture meaning, dependency patches with no project-specific decision, or changes that do not affect the product as such. If a small change is fully explained by the diff and commit message, keep it out of `.worklog`.

Do not create a `spec` for product-neutral dependency updates, library API migrations, executor/source-generator style changes, or framework idiom alignment when behavior, domain contracts, and architecture decisions stay the same.
