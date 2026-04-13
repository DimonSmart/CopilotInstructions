---
id: pack-tests
scope: [codex, copilot]
category: pack
requires: [core-rules]
applies-to: "**/*Tests.cs"
---
# Tests pack

- Test behavior, not implementation details.
- Keep Arrange, Act, Assert obvious from the test body.
- Add edge-case coverage when behavior changes.
- Prefer direct assertions over heavy helper layers.
- Avoid brittle tests that depend on timing, ordering, or shared mutable state.
