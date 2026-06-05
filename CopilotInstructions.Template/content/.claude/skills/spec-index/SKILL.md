---
name: spec-index
description: Rebuild or update `.specs/INDEX.md` from current Spec Guided Dev documents, archived history, and temporary deviations.
---

# Spec index

You are helping maintain the Spec Guided Dev index.

Read:

1. `docs/spec-guided-dev-method.md`
2. `.specs/README.md` if it exists
3. numbered current spec documents directly under `.specs/`
4. `.specs/archive/` only for archived document references, replacements, and index consistency checks
5. `.specs/deviations/` for unresolved temporary implementation deviations

Do not treat `.specs/INDEX.md`, `.specs/README.md`, `.specs/archive/README.md`, `.specs/deviations/README.md`, `.specs/_templates/*.md`, or `.specs/deviations/*.md` as current product intent.

Current spec documents are files matching:

```text
.specs/[0-9][0-9][0-9][0-9].*.md
```

Archived spec documents are files matching:

```text
.specs/archive/[0-9][0-9][0-9][0-9].*.md
```

Deviation documents are files matching:

```text
.specs/deviations/[0-9][0-9][0-9][0-9].deviation-*.md
```

Task:

1. Build or update `.specs/INDEX.md`.
2. Include current documents in the main product-intent table.
3. Include archived documents only in a compact history/replacement section.
4. Include deviations in a separate temporary deviations section.
5. Do not treat deviations as product intent.
6. Preserve document numbers exactly.
7. Extract type from the filename: `spec`, `adr`, `spike`, or `deviation`.
8. Extract title from the document heading or filename.
9. Summarize each current document in one short sentence.
10. Include `Related`, `Replaces`, `Replaced by`, and deviation references when present.
11. Detect broken document number references.
12. Detect unresolved deviations.
13. Detect shared specs and specs that reference them in prose.
14. Report inconsistencies instead of silently guessing.

Rules:

- The index is not the source of truth.
- Do not change numbered spec documents unless the user explicitly asks.
- Do not rewrite requirements, accepted ADRs, or archived documents.
- Do not include micro-changes, local task notes, or commit-level details.
- Do not use archived documents as current product intent.
- Do not use deviations as product intent.
- If shared behavior was extracted, the index may reflect the new shared spec without requiring replacement documents for specs that were cleaned up in place.
- The index is not proof that behavior changed.
- Do not infer semantic changes only from references being added or duplicated text being removed.
- If a document is hard to summarize safely, write `TBD` in the summary and report it.

Output:

- Update `.specs/INDEX.md`.
- Report:
  - number of current documents indexed;
  - number of archived documents observed;
  - number of unresolved deviations;
  - broken references, if any;
  - documents with missing required sections, if noticed.
