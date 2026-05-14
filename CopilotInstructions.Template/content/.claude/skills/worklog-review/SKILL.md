---
name: worklog-review
description: Review whether current changes follow the worklog-driven development rules. Use before commits or when checking worklog structure.
---

# Worklog review

Read:

1. `docs/worklog-method.md`
2. relevant `.worklog/*.active.md` files
3. current git diff
4. `AGENTS.md`
5. `CLAUDE.md` if present

Check:

1. Does this change require a work document?
2. If yes, does the document exist?
3. Is the type correct: `spec`, `adr`, or `spike`?
4. Is the numeric sequence correct?
5. Does the document have clear Goal, Context, and done criteria?
6. For `spec` and `adr`, if `Outcome` exists, does it add durable engineering context instead of task status?
7. For `spike`, are `Result` and `Recommendation` present and meaningful?
8. Does every work document use `.active.md` or `.retired.md`?
9. Are retired documents excluded from current requirements unless referenced through `Replaces` or explicitly needed for history?
10. If an old requirement or ADR changed, was the old active document renamed to `.retired.md` and the new active document given `Replaces:` with the old document number?
11. Are accepted ADRs and old requirements left semantically immutable?
12. Are micro-changes and product-neutral small changes kept out of `.worklog/` and left to commit messages?

Output:

- `OK` if the structure is fine.
- Otherwise list concrete fixes.
- Do not modify files unless the user asks.
