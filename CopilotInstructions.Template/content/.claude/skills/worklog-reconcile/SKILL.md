---
name: worklog-reconcile
description: Compare an existing work document with the actual implementation, git diff, and recent commits. Use after significant implementation or investigation to decide whether an Outcome section, a follow-up work document, or no worklog update is needed.
---

# Worklog reconcile

Read:

1. `docs/worklog-method.md`
2. the work document mentioned by the user
3. current git diff
4. recent commits if needed
5. relevant tests or verification output if available

Task:

Compare the work document with the actual result.

Decide one of:

1. No worklog update needed.
   Use this when implementation followed the document and there are no durable
   deviations, verification notes, partial completion, or follow-up items.

2. Add or update `Outcome`.
   Use this when the actual result contains important implementation notes,
   deviations, partial completion, verification details, limitations, or
   follow-up work that should remain visible in the repository history.

3. Create a new work document and archive the old one.
   Use this when the requirement, decision, or intended behavior changed
   substantially.

4. Non-semantic cleanup needed.
   Use this when the work document still means the same thing, but should be
   cleaned up because duplicated behavior was extracted, wording should be
   clarified, terminology should be fixed, or references to a shared spec should
   be added. This case may edit the existing document in place.

Rules:

- Do not treat implementation completion as a reason to archive a spec.
- Do not use `Outcome` as task status.
- Do not add `Outcome` only to say that the work was completed.
- Do not rewrite old requirements to mean something new.
- Do not rewrite accepted ADR decisions.
- If implementation differs from the work document, document the difference instead of hiding it.
- If the tracked requirement or decision changed substantially, move the old current document from `.worklog/` to `.worklog/archive/`, create a new current document in `.worklog/` with a new number, add `Replaces:` with the archived document number, do not rewrite the semantic content of the archived document, and update `.worklog/INDEX.md`.
- Distinguish semantic changes from non-semantic cleanup.
- Existing specs may be edited in place when the cleanup only removes duplicated common behavior, adds a plain-text reference to the shared spec, and preserves feature-specific behavior.
- Do not archive and replace specs only because shared behavior was extracted.
- Archive and replace only when the requirement, decision, or intended behavior changed substantially.
- Do not create follow-up work documents for product-neutral dependency updates, library API migrations, executor/source-generator style changes, or framework idiom alignment when behavior, domain contracts, and architecture decisions stay the same.
- If there are uncommitted changes, describe them based on the actual diff.
- If no durable information needs to be recorded, report that no worklog update is needed.

If implementation introduced reusable behavior that should be extracted into a shared spec, the extraction may be followed by cleanup edits to existing specs.

If the requirement, decision, or intended behavior changed substantially:

1. Move the old current document from `.worklog/` to `.worklog/archive/`.
2. Create a new current document in `.worklog/` with a new number.
3. Add `Replaces:` with the archived document number.
4. Do not rewrite the semantic content of the archived document.
5. Update `.worklog/INDEX.md`.
