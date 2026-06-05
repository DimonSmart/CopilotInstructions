---
name: worklog-review
description: Review whether current changes follow the worklog-driven development rules. Use before commits or when checking worklog structure.
---

# Worklog review

Read:

1. `docs/worklog-method.md`
2. relevant numbered current work documents directly under `.worklog/`
3. current git diff
4. `AGENTS.md`
5. `CLAUDE.md` if present

Check:

1. Does this change require a work document?
2. If yes, does the document exist?
3. Is the type correct: `spec`, `adr`, or `spike`?
4. Is the numeric sequence correct across `.worklog/` and `.worklog/archive/`?
5. Are current work documents stored directly under `.worklog/`?
6. Are archived work documents stored under `.worklog/archive/`?
7. Do work document filenames avoid old lifecycle markers?
8. Does `.worklog/INDEX.md` exist and reflect the current numbered documents?
9. Does every work document have a title, `Type:`, and `Related:` with document numbers or `none`?
10. For specs, are `Goal`, `Context`, `Scope`, `Non-goals`, `Acceptance criteria`, and `Verification` present and clear?
11. For ADRs, are `Context`, `Options considered`, `Decision`, and `Consequences` present and clear?
12. For spikes, are `Goal`, `Hypothesis`, `Experiment`, `Constraints`, done criteria, `Result`, and `Recommendation` present and meaningful?
13. Specs and ADRs must not contain `Outcome`.
14. If a spec or ADR contains `Outcome`, report it as a structure violation and recommend moving durable semantic changes into a replacement document, or moving execution notes to commit, PR, or issue.
15. Are archived documents excluded from current requirements unless referenced through `Replaces` or explicitly needed for history?
16. If an old requirement or ADR changed, was the old document moved to `.worklog/archive/` and the new current document given `Replaces:`?
17. Are accepted ADRs and old requirements left semantically immutable?
18. Are micro-changes and product-neutral small changes kept out of `.worklog/` and left to commit messages?
19. Are product-neutral dependency updates, library API migrations, executor/source-generator style changes, and framework idiom alignment kept out of `spec` documents when behavior, domain contracts, and architecture decisions stay the same?

Non-semantic edit checks:

1. Did the change modify existing work documents?
2. If yes, are those edits non-semantic?
3. Do they preserve required behavior, scope, non-goals, constraints, and done criteria?
4. Were duplicated shared behaviors removed without changing feature-specific behavior?
5. Were references to shared specs added in plain text without introducing new requirements?
6. If a document was archived and replaced, was there a real semantic change that justified replacement?
7. If there was only cleanup, was the document kept in place instead of archived?

Legacy migration check:

Flag files matching:

- `.worklog/*.active.md`
- `.worklog/*.retired.md`
- `.worklog/archive/*.active.md`
- `.worklog/archive/*.retired.md`

These should be renamed to the new lifecycle-free format.

Output:

- `OK` if the structure is fine.
- Otherwise list concrete fixes.
- If a semantic change appears hidden as cleanup, report it in this form:

```text
Potential semantic change hidden as cleanup:
- document: 0015.spec-search-options.md
- changed behavior: Esc handling changed from closing the dialog to ignoring Esc during validation
- suggested action: create a new work document or ask the user
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
