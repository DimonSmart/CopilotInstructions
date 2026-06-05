---
name: worklog-reconcile
description: Compare an existing work document with the actual implementation, git diff, and recent commits. Use after significant implementation or investigation to decide whether no worklog update, a replacement work document, an additional work document, or notes outside worklog are needed.
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
   Use this when the implementation still matches the current work document and
   there is no changed durable intent, decision, or investigation result.

2. Create a replacement work document.
   Use this when the requirement, decision, or intended behavior changed
   substantially.

3. Create an additional work document.
   Use this when implementation introduced a new durable decision, new
   requirement, or new uncertainty that should be tracked separately.

4. Keep notes outside worklog.
   Use this when the only new information is task status, completion summary,
   routine verification output, local implementation details, or commit-level
   information.

5. Non-semantic cleanup needed.
   Use this when the work document still means the same thing, but should be
   cleaned up because duplicated behavior was extracted, wording should be
   clarified, terminology should be fixed, or references to a shared spec should
   be added. This case may edit the existing document in place.

Rules:

- Do not treat implementation completion as a reason to archive a spec.
- Do not add `Outcome` to specs or ADRs.
- Do not use work documents as task status records.
- Do not document implementation completion in worklog.
- If the spec or ADR still describes the current intent, leave it unchanged.
- If only execution notes changed, report that they belong in commit, PR, or issue, not worklog.
- Do not rewrite old requirements to mean something new.
- Do not rewrite accepted ADR decisions.
- If the tracked requirement or decision changed substantially, move the old current document from `.worklog/` to `.worklog/archive/`, create a new current document in `.worklog/` with a new number, add `Replaces:` with the archived document number, do not rewrite the semantic content of the archived document, and update `.worklog/INDEX.md`.
- Distinguish semantic changes from non-semantic cleanup.
- Existing specs may be edited in place when the cleanup only removes duplicated common behavior, adds a plain-text reference to the shared spec, and preserves feature-specific behavior.
- Do not archive and replace specs only because shared behavior was extracted.
- Archive and replace only when the requirement, decision, or intended behavior changed substantially.
- Do not create follow-up work documents for product-neutral dependency updates, library API migrations, executor/source-generator style changes, or framework idiom alignment when behavior, domain contracts, and architecture decisions stay the same.
- If there are uncommitted changes, describe them based on the actual diff.
- If no durable intent, decision, or investigation result needs to be recorded, report that no worklog update is needed.

If implementation introduced reusable behavior that should be extracted into a shared spec, the extraction may be followed by cleanup edits to existing specs.

If the requirement, decision, or intended behavior changed substantially:

1. Move the old current document from `.worklog/` to `.worklog/archive/`.
2. Create a new current document in `.worklog/` with a new number.
3. Add `Replaces:` with the archived document number.
4. Do not rewrite the semantic content of the archived document.
5. Update `.worklog/INDEX.md`.
