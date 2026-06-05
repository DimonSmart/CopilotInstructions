# Spec Guided Dev

Spec Guided Dev is an AI-assisted development method where a living
specification guides implementation without replacing engineering judgment.

In AI development, the key skill is no longer just writing code, but describing
intent precisely enough that both humans and AI agents can act on it.

The specification is not a task list. It is the durable description of the
product: behavior, contracts, architecture, constraints, non-goals, and
verification rules. If the implementation is deleted, the product should be
rebuildable from the specification.

## Goal

Keep durable product intent, decisions, experiments, important constraints, and
temporary implementation deviations in repository files instead of chat history.

Specification describes durable product intent.

Small product-neutral changes belong in commit messages, not in `.specs`.

## Directory

All spec documents live in `.specs`.

Current spec documents live directly in `.specs/`.

Archived spec documents live in `.specs/archive/`.

Temporary implementation deviations live in `.specs/deviations/`.

Use one increasing numeric sequence across both current and archived documents:

`NNNN.type-short-title.md`

Examples:

- `.specs/0001.spec-initial-product-shape.md`
- `.specs/0002.adr-rendering-architecture.md`
- `.specs/0003.spike-terminal-input-model.md`
- `.specs/archive/0004.spec-old-dialog-behavior.md`

Do not include lifecycle markers such as `active` or `retired` in file names.

Do not split documents into `/specs`, `/adr`, or `/spikes`. The sequence matters
more than classification folders.

When finding the next number, scan numbered spec files in both `.specs/` and
`.specs/archive/`, then use the maximum `NNNN` prefix plus one.

Ignore support files such as `.specs/README.md`, `.specs/INDEX.md`,
`.specs/_templates/*.md`, `.specs/deviations/README.md`, and
`.specs/archive/README.md`.

## Why Specifications Are Numbered

Projects evolve. At the beginning, the full product intent is rarely known.

Numbered specifications give each piece of intent a stable identity while the
system changes. Titles and file names may change, documents may be archived or
replaced, but references by number remain stable.

The sequence records how product understanding evolved without turning
specifications into task logs.

## Document Types

### spec

Use `spec` to describe durable product intent.

A spec defines what the system should be: behavior, scenarios, contracts,
constraints, stable patterns, compatibility expectations, non-goals, and
verification rules.

A spec is not a task list.

Use `spec` when the change affects what future coding agents should build,
preserve, avoid, depend on, or verify.

Do not use `spec` for product-neutral technical chores such as dependency
updates, formatting, small refactoring, or framework idiom alignment when
product behavior, domain contracts, architecture decisions, and durable
implementation patterns stay the same.

If the product is conceptually built around a specific library, framework,
storage model, protocol, UI architecture, or integration pattern, that choice
belongs in the specification or ADR.

Replacing that choice with another one is a semantic change.

Updating a logger version without changing product behavior, architecture,
contracts, or durable project decisions is not a spec change.

### adr

Use `adr` for architectural or long-lived technical decisions.

An ADR records why a decision was made, which alternatives were considered,
why the chosen option was accepted, and what consequences it has.

ADRs may also record decisions that later turn out to be wrong. The value of
the ADR is the preserved reasoning, not only the final correctness.

An ADR is not a task list.

Accepted ADRs must not be rewritten semantically. If the decision changes,
archive the old ADR and create a new ADR with `Replaces`.

### spike

Use `spike` for experiments, feasibility checks, technical uncertainty, and
research before making a spec or ADR decision.

A spike answers: what must be checked before deciding?

A spike can lead to:

- a new specification;
- a new ADR;
- rejection of an idea;
- follow-up research.

### deviation

Use `deviation` for temporary implementation-state notes.

A deviation records a known gap between the current implementation and the
current specification.

Deviation documents are not specifications. They must not change product
intent, weaken requirements, or redefine what should be built.

Deviation documents live under `.specs/deviations/`:

```text
.specs/deviations/
  0007.deviation-mouse-input-temporarily-disabled.md
```

The `0007` prefix is the number of the specification the deviation relates to.

Deviation documents are temporary.

When the implementation is brought back in sync with the specification, delete
the deviation document from the current tree. Git history is enough to preserve
that the deviation existed.

Do not create `.specs/deviations/archive/` by default.

Do not keep resolved deviations in the active specs tree.

Use a deviation when:

- the specification remains correct;
- the current implementation temporarily does not match it;
- the reason is important for the next coding agent;
- the gap must not change product intent.

Do not use a deviation when:

- the requirement really changed;
- the behavior is no longer needed;
- the specification is obsolete;
- an old specification must be replaced by a new one.

In those cases, archive the old specification and create a new one.

## Shared Specifications

Do not create a separate document type for shared behavior.

Shared behavior is a normal `spec`.

Example:

```text
.specs/0027.spec-dialog-window-behavior.md
```

That document can describe:

- common dialog window structure;
- keyboard behavior;
- mouse behavior;
- validation behavior;
- accessibility constraints;
- default buttons;
- close/cancel semantics.

Other specifications reference it by number:

```md
## Shared behavior

Common dialog window behavior is defined by 0027.

This specification only defines:

- conflict-specific message text;
- available conflict actions;
- default selected action;
- result returned to the copy operation.
```

Existing specs may be edited in place to remove duplicated shared behavior and
replace it with a reference to the shared spec when feature-specific behavior
does not change.

## Semantic And Non-Semantic Edits

A change is semantic when it changes what future coding agents should build,
preserve, avoid, depend on, or verify.

A change is non-semantic when it only improves wording, structure, terminology,
or references without changing what should be built.

Semantic changes include:

- changing product behavior;
- changing domain contracts;
- changing supported input/output;
- changing keyboard or mouse semantics;
- changing validation rules;
- changing compatibility expectations;
- changing scope or non-goals;
- changing accepted ADR decisions;
- replacing a library/framework that defines the product shape;
- removing a requirement;
- adding a requirement;
- changing a constraint future agents must follow.

Non-semantic changes include:

- improving wording without changing meaning;
- fixing terminology;
- fixing typos;
- restructuring sections;
- removing duplicated shared behavior after extracting a shared spec;
- adding a reference to a shared spec;
- aligning wording with already implemented behavior when this does not add a
  new requirement.

Do not hide semantic changes as cleanup.

## Current And Archived Documents

A spec document is current when it is stored directly under `.specs/`.

A spec document is archived when it is stored under `.specs/archive/`.

Archived documents are historical. They are not current product intent.

The agent must use only numbered current documents in `.specs/` as the default
product-intent context.

The agent must not use recursive `.specs/**/*.md` search as current context,
because it includes archived documents, deviations, templates, and support
files.

Implementation completion does not automatically archive a spec. A spec may
stay current after implementation if it still describes current product intent,
requirements, decisions, or constraints.

Archive when:

- the essence of a requirement changed;
- product intent changed;
- a new architectural decision replaces an old one;
- a feature is removed from scope;
- the old document no longer describes what should be built.

Do not archive when:

- implementation is complete;
- the document was cleaned up;
- duplicated shared behavior was extracted into a separate spec;
- wording was fixed;
- a shared spec reference was added;
- current implementation temporarily differs from the specification.

For temporary implementation mismatch, use `deviation`, not archiving.

When a current document needs semantic replacement:

1. Move the old file from `.specs/` to `.specs/archive/`.
2. Create one or more new current documents in `.specs/` with new numbers.
3. In the new document, explain that it replaces or supersedes the old document.
4. Do not rewrite the semantic content of the archived document.

References use document numbers only:

```md
Replaces:
- 0008

Related:
- 0002
- 0007
```

Do not reference spec documents by filename in `Replaces` or `Related`; file
names can change when a document is archived.

## Required Sections

Every spec document must have:

- title;
- type;
- related documents, or `none`.

### spec

Specs must have:

- goal;
- context;
- scope;
- non-goals;
- specification;
- acceptance criteria;
- verification.

Optional:

- shared behavior;
- compatibility;
- implementation constraints.

### adr

ADRs must have:

- context;
- options considered;
- decision;
- consequences.

Optional:

- rejected options;
- follow-up;
- superseded by.

### spike

Spikes must have:

- goal;
- hypothesis;
- experiment;
- constraints;
- done criteria;
- result;
- recommendation.

### deviation

Deviation documents must have:

- related spec;
- spec expectation;
- current implementation state;
- reason;
- temporary decision;
- revisit condition;
- resolution.

## Outcome

New `spec` and `adr` templates do not contain `## Outcome`.

After implementation, add `## Outcome` only when the actual result contains
durable engineering context that should remain attached to this document:
deviations, limitations, partial verification, important implementation notes,
or follow-up work.

Do not use Outcome as task status.

Do not use Outcome to redefine the specification.

If product intent changed, create a new spec document and archive the old one.

If implementation temporarily differs from the specification, create a
temporary deviation document instead.

Outcome is not used for temporary implementation deviations. Use
`.specs/deviations/` for that.

Spikes are different: they must record `Result` and `Recommendation`, because
their purpose is to preserve investigation output.

## Agent Workflow

Before significant implementation:

1. Read relevant current `.specs/NNNN.*.md` documents.
2. Read related ADRs and spikes when they explain the selected approach.
3. Read related deviation documents only when current implementation state
   matters.
4. Decide whether the requested change needs a new spec document.
5. If needed, create the next numbered document from `.specs/_templates`.
6. Confirm the required sections for the selected document type.
7. Implement only within the current specification and explicit user request.

After significant implementation or investigation:

1. Run verification commands.
2. Compare implementation with relevant specs.
3. If implementation matches the spec, no spec update is needed.
4. If durable product intent changed, archive the old spec document and create
   a new one.
5. If implementation temporarily differs from the spec, create or update a
   deviation document.
6. If a deviation was resolved, delete the deviation document.
7. Do not silently rewrite existing specifications or accepted ADRs.

## Migration From Worklog

1. Move `.worklog/` to `.specs/`.
2. Move `.worklog/archive/` to `.specs/archive/`.
3. Replace `docs/worklog-method.md` with `docs/spec-guided-dev-method.md`.
4. Replace `docs/ai-context/packs/worklog.md` with
   `docs/ai-context/packs/spec-guided-dev.md`.
5. Rename `worklog-*` skills to `spec-*` skills.
6. Keep document numbers unchanged.
7. Do not change document meaning during migration.
8. Do not delete existing Outcome sections automatically.
9. Use `.specs/deviations/` only for temporary current implementation gaps.
