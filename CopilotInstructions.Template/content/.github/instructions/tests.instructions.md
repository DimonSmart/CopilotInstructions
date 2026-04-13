---
applyTo: "**/*Tests.cs"
id: instr-tests
requires: [pack-tests]
description: "Test file-level Copilot instructions"
---

- Tests must verify behavior, not implementation details.
- Prefer direct assertions over clever helpers.
- Cover edge cases when behavior changed.
- Do not add test-only complexity without need.
