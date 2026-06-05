# Worklog-driven development

## Goal

Keep project intent, decisions, experiments, and significant changes in repository files instead of chat history.

The worklog is not a replacement for commits. It records meaningful engineering intent.

Small product-neutral changes belong in commit messages, not in `.worklog`.

## Directory

All work documents live in `.worklog`.

Current documents live directly in `.worklog/`.

Retired documents live in `.worklog/archive/`.

Use one increasing numeric sequence across both current and archived documents:

`NNNN.type-short-title.md`

Examples:

- `.worklog/0001.spec-initial-mvp.md`
- `.worklog/0002.spike-console-double-buffering.md`
- `.worklog/archive/0003.adr-old-rendering-model.md`

Do not include lifecycle markers such as `active` or `retired` in file names.

Do not split documents into `/specs`, `/adr`, or `/spikes`. The sequence matters more than classification folders.

When finding the next number, scan numbered worklog files in both `.worklog/` and `.worklog/archive/`, then use the maximum `NNNN` prefix plus one.

Ignore support files such as `.worklog/README.md`, `.worklog/INDEX.md`, `.worklog/_templates/*.md`, and `.worklog/archive/README.md`.

## Document Types

### spec

Use `spec` when the system gets a new capability or user-visible behavior changes.

Question answered: what should the system do?

Do not use `spec` for product-neutral technical chores such as dependency updates, API-style migrations, executor/source-generator style changes, or framework idiom alignment when the product behavior, domain contracts, and architecture decision stay the same.

### adr

Use `adr` for architectural or long-lived technical decisions.

Question answered: why did we choose this solution?

ADRs are immutable once they describe an accepted decision. If the decision changes, create a new ADR and archive the old one.

### spike

Use `spike` for research, experiments, feasibility checks, or technical uncertainty.

Question answered: what should be checked before making a decision?

## When Not To Create A Work Document

Do not create a numbered work document for:

- micro-changes;
- typo fixes;
- formatting-only changes;
- obvious bug fixes fully explained by the commit message;
- small refactoring with no behavior or architectural meaning;
- dependency patch updates with no project-specific decision.
- product-neutral library API migrations or framework-style follow-ups that keep the same behavior and domain contracts.

Also do not create a work document for small changes or fixes that do not affect the product as such: no architecture change, no library choice, no experiment, no new capability, no changed requirement, and no user-visible behavior change. Describe those changes at the commit-message level.

If the change is completely understandable from a diff and a commit message, keep it out of `.worklog`.

Library updates need a work document only when the project makes a durable choice, accepts a changed architecture or contract, runs a real investigation, or changes user-visible behavior. A follow-up to rewrite code to the newer library idiom is not enough by itself.

When unsure whether a small change deserves a work document, do not create one by default. Create a document only when the change needs durable requirements, decision history, or investigation notes.

There is no `task` work document type. Small local tasks stay in commit messages unless they are really a `spec`, `adr`, or `spike`.

## Current And Archived Documents

A document is current when it is stored directly under `.worklog/`.

A document is archived when it is stored under `.worklog/archive/`.

There are no committed filename states named `draft`, `changed`, `deleted`, `outdated`, or `superseded`. Archived history is represented by location under `.worklog/archive/`, not by a filename state. Drafts live outside the main branch or remain uncommitted.

The agent must use only numbered current documents in `.worklog/` as the default work context.

The agent must not use recursive `.worklog/**/*.md` search as current context, because it includes archived documents and support files.

Implementation completion does not automatically archive a spec. A spec may stay current after implementation if it still describes current intended behavior, requirements, decisions, or constraints.

Archived documents are historical. Read them only when:

- a current document references them through `Replaces`;
- the user explicitly asks to inspect history;
- the task is to explain why a decision was made;
- `worklog-reconcile` compares old and new meaning;
- `worklog-index` updates links and replacement history.

If an edit changes intended behavior, architectural meaning, scope, non-goals, constraints, compatibility, or done criteria, it is semantic.

Do not hide semantic changes as cleanup.

When a current document needs substantial semantic changes:

1. Move the old file from `.worklog/` to `.worklog/archive/`.
2. Create one or more new current documents in `.worklog/` with new numbers.
3. In the new document, explain that it replaces or supersedes the old document.
4. Do not rewrite the semantic content of the archived document.

Do not archive and replace a current document only because duplicated text was extracted into a shared spec.

Archive and replace only when the intended behavior, decision, scope, non-goals, constraints, or done criteria changed substantially.

Minor factual completion of a current work document is allowed only when it records durable context about that same document without changing its requirement or decision. Do not change old requirements in place to mean something new.

If a feature is removed from scope, archive the old feature spec and create a new current spec describing the current requirement that the feature is out of scope.

References use document numbers only:

```md
Replaces:
- 0008

Related:
- 0002
- 0007
```

Do not reference work documents by filename in `Replaces` or `Related`; filenames can change when a document is archived.

## Non-semantic Edits To Existing Work Documents

A work document may be edited in place when the edit is non-semantic.

A non-semantic edit preserves the required behavior, accepted decision, scope, non-goals, constraints, and done criteria.

Non-semantic edits are behavior-preserving changes that improve the document without changing what the system is expected to do.

Examples:

- removing duplicated behavior after extracting a shared spec;
- adding a plain-text reference to a shared spec;
- improving wording without changing meaning;
- fixing terminology;
- restructuring sections;
- fixing typos or incorrect names;
- aligning the document with already existing implemented behavior, when this does not introduce a new requirement;
- clarifying intent that was already implied by the existing document and implementation.

When a shared spec is extracted from duplicated behavior, existing specs may be edited in place to remove duplicated common behavior and reference the new shared spec, as long as their intended feature behavior does not change.

Do not create replacement documents for purely non-semantic edits.

Before editing an existing work document in place, ask:

After this edit, should a coding agent implement the same behavior as before?

If yes, the edit is probably non-semantic and may be done in place.

If no, or if the answer is unclear, treat the edit as semantic or ask the user.

If the edit changes what future coding agents should build, preserve, avoid, or verify, it is probably semantic.

If the edit only changes how existing intent is organized or referenced, it is probably non-semantic.

When a shared spec is created from behavior duplicated across existing specs, the existing specs may be cleaned up in place.

The cleanup may:

- remove duplicated common behavior;
- add a plain-text reference to the new shared spec by number;
- keep only feature-specific details;
- clarify that the shared spec owns the common behavior.

This cleanup must not change the feature-specific behavior of the existing specs.

Example:

```md
## Shared behavior

Common dialog window behavior is defined by 0027.

This specification only defines:

- conflict-specific message text;
- available conflict actions;
- default selected action;
- result returned to the copy operation.
```

Do not use non-semantic cleanup to change behavior.

The following changes are semantic and require a new work document or explicit user confirmation:

- changing user-visible behavior;
- changing keyboard or mouse semantics;
- changing validation rules;
- changing default actions;
- changing compatibility expectations;
- changing scope or non-goals;
- changing done criteria;
- changing accepted ADR decisions;
- removing a requirement as obsolete;
- adding a new requirement;
- changing a constraint that future agents must follow.

Example:

```text
Old behavior:
Esc closes the dialog.

New behavior:
Esc is ignored when the dialog has validation errors.
```

This is a semantic change. It must not be presented as simple spec cleanup.

## Required Sections

Every work document must have:

- title;
- type;
- related documents, or `none`.

Specs must have:

- goal;
- context;
- scope;
- non-goals;
- acceptance criteria;
- verification.

ADRs must have:

- context;
- options considered;
- decision;
- consequences.

Spikes must have:

- goal;
- hypothesis;
- experiment;
- constraints;
- done criteria;
- result;
- recommendation.

When a new document replaces an older one, it must include:

- `Replaces`.

## Implementation result

Work documents describe durable project intent, decisions, and investigations.
They are not task records.

Do not add implementation status, completion notes, execution summaries, or
routine verification output to `spec` or `adr` documents.

After implementation:

1. If the current work document still describes the intended behavior or accepted
   decision, leave it unchanged.
2. If the requirement or decision changed substantially, archive the old document
   and create a new current document with `Replaces:`.
3. If implementation introduced a new durable architectural decision, create an ADR.
4. If implementation revealed uncertainty that needs research, create a spike.
5. Keep execution details, completed work summaries, routine verification output,
   and local implementation notes in commits, pull requests, or issue tracker.

Spikes are different: they must record `Result` and `Recommendation`, because
their purpose is to preserve investigation output.

## Agent Workflow

Before significant implementation:

1. Read numbered current `.worklog/NNNN.*.md` documents relevant to the task.
2. Decide whether a new work document is needed.
3. If needed, create the next numbered document from the matching `.worklog/_templates` file.
4. Confirm the required sections for that document type.
5. For specs, confirm scope, non-goals, acceptance criteria, and verification.
6. Implement only the described scope.

After significant implementation or investigation:

1. Run verification commands.
2. Reconcile the work document with the actual result.
3. Decide whether no worklog update is needed, a replacement work document is
   needed, or an additional spec, ADR, or spike is needed.
4. Do not add implementation status or completion notes to specs or ADRs.
5. Do not silently rewrite existing requirements or accepted ADRs.
