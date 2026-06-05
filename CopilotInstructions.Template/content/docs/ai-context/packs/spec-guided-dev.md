---
id: spec-guided-dev
scope: [codex, copilot]
category: process
requires: [ai-rules]
---

# Spec Guided Dev

This project uses Spec Guided Dev.

Spec Guided Dev is an AI-assisted development method where a living
specification guides implementation without replacing engineering judgment.

Before significant project work:

- read `docs/spec-guided-dev-method.md`;
- inspect relevant numbered current `.specs/NNNN.*.md` files;
- inspect `.specs/deviations/` only when current implementation state matters;
- do not inspect `.specs/archive/` unless explicitly needed;
- decide whether the change needs a new spec document;
- if yes, create the next `NNNN.type-short-title.md` file from
  `.specs/_templates/`.

Do not create spec documents for micro-changes, small product-neutral changes,
or fixes fully explained by a commit message.

Do not create a `spec` for product-neutral technical chores such as dependency
updates, library API migrations, executor/source-generator style changes, or
framework idiom alignment when behavior, domain contracts, and architecture
decisions stay the same.

Use only numbered files directly under `.specs/` as current product intent.
Files under `.specs/archive/` are history.

Deviation documents under `.specs/deviations/` are temporary implementation
state notes. They are not specifications. Delete them when resolved.

If product intent changed, create a new current spec document and archive the
old one.

If only wording, structure, terminology, or shared behavior references changed,
edit the current document in place as non-semantic cleanup.
