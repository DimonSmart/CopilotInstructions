---
id: pack-mcp
scope: [codex, copilot]
category: pack
requires: [dotnet-rules]
---
# MCP pack

- Keep tool contracts small, explicit, and predictable.
- For repo-defined MCP tools, use `snake_case` for MCP-exposed parameter names to keep contracts consistent across languages.
- Validate tool inputs and return structured results.
- Separate transport concerns from business logic.
- Do not hide required values behind placeholders or silent defaults.
