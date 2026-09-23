#!/bin/bash
# Stop hook: keeps every source file under 500 lines (see .claude/rules/code-style.md "File size").
# Blocks Claude from finishing while a file is over the limit, so it splits it first.
# On the second attempt in a row (stop_hook_active) it lets Claude stop and only warns, to avoid a loop.
LIMIT=500
root="${CLAUDE_PROJECT_DIR:-$(pwd)}"
active=$(jq -r '.stop_hook_active // false' 2>/dev/null)
over=$(find "$root/frontend/src" "$root/backend/app" -type f \
  \( -name '*.ts' -o -name '*.tsx' -o -name '*.py' -o -name '*.css' \) \
  -not -path '*/node_modules/*' -not -path '*/__pycache__/*' -print0 2>/dev/null \
  | xargs -0 wc -l 2>/dev/null | awk -v l=$LIMIT '$2!="total" && $1>l {print $2" ("$1" lines)"}' \
  | sed "s|$root/||")
[ -z "$over" ] && exit 0
list=$(printf '%s\n' "$over" | paste -sd ';' - | sed 's/;/; /g')
if [ "$active" = "true" ]; then
  jq -n --arg m "Files still over $LIMIT lines: $list" '{systemMessage: $m}'
else
  jq -n --arg r "These files are over $LIMIT lines: $list. Split each into a folder named after the component (e.g. components/PurchaseBill/ with index.tsx plus sub-component, hook and data files), keep every file under $LIMIT lines, update imports, then run npm run build && npm run lint." \
    '{decision: "block", reason: $r}'
fi
