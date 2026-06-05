---
name: spec-start
description: Create the next numbered Spec Guided Dev document before significant implementation work. Use when starting durable product intent, an ADR, a spike, or a temporary deviation.
---

# Spec start

You are helping maintain the project specifications.

Read:

1. `docs/spec-guided-dev-method.md`
2. `.specs/README.md` if it exists
3. relevant numbered current spec documents directly under `.specs/`
4. `.specs/deviations/` only when current implementation state matters
5. the relevant template from `.specs/_templates/`

Task:

1. Understand the requested change.
2. Decide whether it needs a `spec`, `adr`, `spike`, or `deviation`.
3. If it does not need one, explain briefly why.
4. If it needs one:
   - determine the next `NNNN` number by scanning both `.specs/NNNN.*.md` and `.specs/archive/NNNN.*.md`;
   - choose one type: `spec`, `adr`, `spike`, or `deviation`;
   - create `.specs/NNNN.type-short-title.md` for `spec`, `adr`, or `spike`;
   - create `.specs/deviations/NNNN.deviation-short-title.md` for temporary implementation gaps, where `NNNN` is the related spec number;
   - fill it using the matching template;
   - confirm the required sections for the selected document type.
5. If the new document replaces an existing current document:
   - move the old file from `.specs/` to `.specs/archive/`;
   - add `Replaces:` with the old document number to the new document;
   - do not rewrite the semantic content of the archived document.
6. After creating a document, update `.specs/INDEX.md` or tell the user that `spec-index` should be run.

Shared spec extraction:

- Shared behavior is a normal `spec`.
- If a shared spec is created from duplicated behavior, existing specs that duplicated that behavior may be edited in place to remove duplication and reference the new shared spec in plain text.
- Do not create replacement specs for existing documents when the cleanup preserves behavior.
- Ask the user before extracting shared behavior if the extraction changes ownership of behavior or affects how future specs should be written.

Rules:

- Do not implement code in this skill unless the user explicitly asks to continue after the document is created.
- Do not create documents for task-level changes.
- Do not create a spec document for micro-changes or small product-neutral changes or fixes that are fully captured by a commit message.
- Do not create a `spec` for product-neutral technical chores such as dependency updates, library API migrations, executor/source-generator style changes, or framework idiom alignment when behavior, domain contracts, and architecture decisions stay the same.
- Do not create `task` documents; small local tasks belong in commit messages unless they are really a `spec`, `adr`, `spike`, or `deviation`.
- Keep typos, formatting, obvious local fixes, small refactoring with no behavior or architecture meaning, dependency patches with no project-specific decision, and changes with no changed requirement out of `.specs/`.
- When unsure about a small change, do not create a spec document by default.
- Use only numbered files directly under `.specs/` as current product intent. Files under `.specs/archive/` are history.
- Inspect `.specs/deviations/` only when current implementation state matters.
- Do not include lifecycle markers such as `active` or `retired` in file names.
- Do not create current documents inside `.specs/archive/`.
- Before modifying an existing spec document in place, verify that the change is non-semantic.
- If the change would alter product behavior, domain contracts, scope, non-goals, verification rules, constraints, or architectural meaning, do not edit it as cleanup.
- Deviation documents are temporary. They must not change product intent or weaken requirements.
- Do not invent requirements.
- If important information is missing, mark it as `TBD` instead of guessing.
- Keep the document useful for a coding agent, not just for a human reader.
