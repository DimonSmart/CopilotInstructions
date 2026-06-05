---
name: spec-review
description: Review whether current changes follow Spec Guided Dev rules. Use before commits or when checking spec structure.
---

# Spec review

Read:

1. `docs/spec-guided-dev-method.md`
2. relevant numbered current spec documents directly under `.specs/`
3. related `.specs/deviations/*.md` documents when current implementation state matters
4. current git diff
5. `AGENTS.md`
6. `CLAUDE.md` if present

Check:

1. Does this change require a spec document?
2. If yes, does the document exist?
3. Is the type correct: `spec`, `adr`, `spike`, or `deviation`?
4. Is the numeric sequence correct across `.specs/` and `.specs/archive/`?
5. Are current spec documents stored directly under `.specs/`?
6. Are archived spec documents stored under `.specs/archive/`?
7. Are deviation documents stored under `.specs/deviations/`?
8. Do spec document filenames avoid old lifecycle markers?
9. Does `.specs/INDEX.md` exist and reflect current numbered documents?
10. Does every spec document have a title, `Type:`, and `Related:` with document numbers or `none`?
11. Does the document have required sections for its type?
12. Are specs describing product intent rather than tasks?
13. Are product-neutral changes kept out of `.specs`?
14. Are deviations temporary and separate from specs?
15. Are resolved deviations deleted?
16. Are archived specs excluded from current product intent?
17. Are semantic changes not hidden as cleanup?
18. Are shared specs represented as normal spec documents?
19. If an old requirement or ADR changed, was the old document moved to `.specs/archive/` and the new current document given `Replaces:`?
20. Are accepted ADRs and old requirements left semantically immutable?

Required section checks:

- Specs must have `Goal`, `Context`, `Scope`, `Non-goals`, `Specification`, `Acceptance criteria`, and `Verification`.
- ADRs must have `Context`, `Options considered`, `Decision`, and `Consequences`.
- Spikes must have `Goal`, `Hypothesis`, `Experiment`, `Constraints`, `Done criteria`, `Result`, and `Recommendation`.
- Deviations must have related spec, `Spec expectation`, `Current implementation state`, `Reason`, `Temporary decision`, `Revisit condition`, and `Resolution`.

Outcome checks:

- New spec and ADR templates must not contain `## Outcome`.
- Existing imported spec or ADR documents may contain legacy `Outcome`, but it must not be used as task status or to redefine product intent.
- If a spec or ADR contains `Outcome`, report it as a structure issue when it contains task status, completion notes, or semantic changes that should be represented by replacement specs, ADRs, or deviations.

Non-semantic edit checks:

1. Did the change modify existing spec documents?
2. If yes, are those edits non-semantic?
3. Do they preserve product behavior, domain contracts, scope, non-goals, constraints, verification rules, and accepted decisions?
4. Were duplicated shared behaviors removed without changing feature-specific behavior?
5. Were references to shared specs added in plain text without introducing new requirements?
6. If a document was archived and replaced, was there a real semantic change that justified replacement?
7. If there was only cleanup, was the document kept in place instead of archived?

Legacy migration check:

Flag files matching:

- `.worklog/**`
- `.specs/*.active.md`
- `.specs/*.retired.md`
- `.specs/archive/*.active.md`
- `.specs/archive/*.retired.md`

These should be migrated to the Spec Guided Dev structure.

Output:

- `OK` if the structure is fine.
- Otherwise list concrete fixes.
- If a semantic change appears hidden as cleanup, report it in this form:

```text
Potential semantic change hidden as cleanup:
- document: 0015.spec-search-options.md
- changed behavior: Esc handling changed from closing the dialog to ignoring Esc during validation
- suggested action: create a new spec document or ask the user
```

- If cleanup-only work is valid, report it in this form when useful:

```text
Cleanup-only edit detected:
- document: 0010.spec-copy-conflict.md
- duplicated dialog behavior was replaced with a reference to 0027
- behavior appears preserved
- no replacement spec needed
```

- Do not modify files unless the user asks.
