---
name: spec-reconcile
description: Compare implementation with Spec Guided Dev documents and decide whether no update, cleanup, replacement, or temporary deviation is needed.
---

# Spec reconcile

Read:

1. `docs/spec-guided-dev-method.md`
2. relevant current `.specs/NNNN.*.md` documents
3. related ADRs and spikes when they explain the selected approach
4. related `.specs/deviations/*.md` documents when current implementation state matters
5. current git diff
6. recent commits if needed
7. relevant tests or verification output if available

Task:

Compare the implementation with the relevant specs.

Decide one of:

1. No spec update needed.
   Use this when the implementation still matches current product intent and
   there is no changed durable intent, decision, or investigation result.

2. Non-semantic cleanup needed.
   Use this when the spec still means the same thing, but wording, structure,
   terminology, or references should be improved. This case may edit the
   existing document in place.

3. New spec or ADR needed because product intent changed.
   Use this when requirements, product behavior, domain contracts,
   architecture decisions, scope, non-goals, or durable constraints changed.

4. Temporary deviation needed.
   Use this when the current implementation temporarily differs from a still
   correct specification.

5. Existing deviation resolved and should be deleted.
   Use this when implementation is back in sync with the related spec.

Rules:

- Do not treat implementation completion as a reason to archive a spec.
- Do not use specs as task status records.
- Do not document implementation completion in `.specs`.
- If the spec or ADR still describes current product intent, leave it unchanged.
- If only execution notes changed, report that they belong in commit, PR, or issue, not `.specs`.
- Do not rewrite old requirements to mean something new.
- Do not rewrite accepted ADR decisions.
- If durable product intent changed, move the old current document from `.specs/` to `.specs/archive/`, create a new current document in `.specs/` with a new number, add `Replaces:` with the archived document number, do not rewrite the semantic content of the archived document, and update `.specs/INDEX.md`.
- Distinguish semantic changes from non-semantic cleanup.
- Existing specs may be edited in place when the cleanup only removes duplicated common behavior, adds a plain-text reference to the shared spec, and preserves feature-specific behavior.
- Do not archive and replace specs only because shared behavior was extracted.
- Do not create follow-up spec documents for product-neutral dependency updates, library API migrations, executor/source-generator style changes, or framework idiom alignment when behavior, domain contracts, and architecture decisions stay the same.
- If implementation temporarily differs from the spec, create or update a deviation under `.specs/deviations/`.
- If a deviation is resolved, delete the deviation document from the current tree. Git history is enough.
- Do not create `.specs/deviations/archive/`.
- If there are uncommitted changes, describe them based on the actual diff.
- If no durable intent, decision, investigation result, or temporary deviation needs to be recorded, report that no spec update is needed.

If implementation introduced reusable behavior that should be extracted into a shared spec, the extraction may be followed by cleanup edits to existing specs.

If product intent changed:

1. Move the old current document from `.specs/` to `.specs/archive/`.
2. Create a new current document in `.specs/` with a new number.
3. Add `Replaces:` with the archived document number.
4. Do not rewrite the semantic content of the archived document.
5. Update `.specs/INDEX.md`.
