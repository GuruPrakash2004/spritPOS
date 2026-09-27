#!/usr/bin/env python3
"""UI audit: checks a React/Tailwind codebase against the ui_production_skill laws.

Runs the checks from SKILL.md that can be found statically (emojis, icon
libraries, pure black/white, missing dark variants, off-grid spacing, harsh
effects, border weight, radius sprawl, missing hover/press feedback, native
alert/confirm, numbers without tabular-nums). The rest of the checklist
(user flow, redundancy, KPIs, pricing, ...) needs a human or Claude to judge;
it is printed at the end as a manual review list.

Usage:
    python3 ui_audit.py [PATH ...] [--json] [--min-severity info|warn|error]
                        [--fail-on warn|error] [--max-per-rule N]

PATH defaults to ./frontend/src if it exists, else ./src, else the current dir.
Standard library only.
"""
from __future__ import annotations

import argparse
import json
import re
import sys
from collections import Counter, defaultdict
from dataclasses import asdict, dataclass
from pathlib import Path

EXTS = {".tsx", ".jsx", ".ts", ".js", ".html", ".css", ".vue", ".svelte"}
SKIP_DIRS = {"node_modules", "dist", "build", ".git", ".next", "coverage", "models"}
SEVERITY = {"info": 0, "warn": 1, "error": 2}

EMOJI = re.compile(
    "[\U0001F300-\U0001FAFF\U00002600-\U000027BF\U0001F000-\U0001F2FF\U0001F900-\U0001F9FF]"
)
ICON_IMPORT = re.compile(
    r"""from\s+['"](lucide-react|@phosphor-icons/[^'"]+|phosphor-react|@heroicons/[^'"]+|"""
    r"""react-feather|feather-icons|react-icons(?:/[^'"]+)?|@fortawesome/[^'"]+|@mui/icons-material[^'"]*)['"]"""
)
PURE_BW = re.compile(
    r"(?<![\w-])(?:bg|text|border|from|to|via|fill|stroke)-(?:black|white)(?![\w/-])"
    r"|#(?:000000|000|ffffff|fff)\b",
    re.I,
)
ARBITRARY_COLOR = re.compile(r"(?<![\w:-])((?:bg|text|border|ring|from|to|via)-\[#[0-9a-fA-F]{3,8}\])")
ARBITRARY_PX = re.compile(r"(?<![\w-])(?:-?(?:p|px|py|pt|pb|pl|pr|m|mx|my|mt|mb|ml|mr|gap|gap-x|gap-y|space-x|space-y))-\[(\d+(?:\.\d+)?)px\]")
HALF_STEP = re.compile(r"(?<![\w-])(?:p|px|py|pt|pb|pl|pr|m|mx|my|mt|mb|ml|mr|gap)-(\d+\.5)(?![\w])")
HARSH_SHADOW = re.compile(r"(?<![\w-])shadow-(?:xl|2xl|\[[^\]]*rgba?\(0,\s*0,\s*0,\s*0?\.[5-9][^\]]*\]|black[\w/]*)")
GRADIENT = re.compile(r"(?<![\w-])bg-(?:gradient|linear|radial|conic)-|linear-gradient\(|radial-gradient\(")
GLOW = re.compile(r"(?<![\w-])(?:drop-shadow-\[|shadow-\w+-500|animate-pulse|blur-3xl)")
THICK_BORDER = re.compile(r"(?<![\w-])border(?:-[trblxy])?-(?:2|4|8)(?![\w])")
ROUNDED = re.compile(r"(?<![\w-])rounded(?:-(?:none|sm|md|lg|xl|2xl|3xl|full|\[[^\]]+\]))?(?![\w-])")
BUTTON_OPEN = re.compile(r"<button\b")
# `await confirm(...)` is a promise-based in-app dialog (e.g. a useConfirm hook), not the browser one.
NATIVE_DIALOG = re.compile(r"(?<![\w.])(?<!await )(?:window\.)?(alert|confirm|prompt)\s*\(")
NUMBER_RENDER = re.compile(r"\{\s*(?:money|formatMoney|fmt\w*|toFixed|\w+\.toLocaleString)\s*\(|\.toFixed\(\d\)\s*\}")
CLASS_ATTR = re.compile(r"""className\s*=\s*(?:"([^"]*)"|'([^']*)'|\{`([^`]*)`\})""")


@dataclass
class Finding:
    rule: str
    law: str
    severity: str
    file: str
    line: int
    message: str
    snippet: str


def iter_files(roots: list[Path]):
    for root in roots:
        if root.is_file():
            yield root
            continue
        for p in root.rglob("*"):
            if p.is_file() and p.suffix in EXTS and not SKIP_DIRS.intersection(p.parts):
                yield p


def is_comment(line: str) -> bool:
    s = line.strip()
    return s.startswith(("//", "/*", "*", "#!", "<!--"))


def audit_file(path: Path, root: Path, stats: dict) -> list[Finding]:
    try:
        text = path.read_text(encoding="utf-8")
    except (UnicodeDecodeError, OSError):
        return []
    rel = str(path.relative_to(root)) if path.is_relative_to(root) else str(path)
    lines = text.splitlines()
    out: list[Finding] = []

    def add(rule, law, sev, i, msg):
        out.append(Finding(rule, law, sev, rel, i + 1, msg, lines[i].strip()[:140]))

    for i, line in enumerate(lines):
        if is_comment(line):
            continue

        if EMOJI.search(line):
            add("emoji", "LAW 1 Icons", "error", i, "Emoji in UI source; use a Lucide icon instead.")

        m = ICON_IMPORT.search(line)
        if m:
            stats["icon_libs"][m.group(1).split("/")[0]] += 1

        for m in PURE_BW.finditer(line):
            tok = m.group(0)
            # White text on a filled accent button is normal; flag it as info only.
            sev = "info" if tok.lower() == "text-white" else "warn"
            add("pure-black-white", "Mistake 8 / Layer 1", sev, i,
                f"`{tok}`: use near-black / near-white (e.g. 98-99% white, 11% text) instead.")

        for m in ARBITRARY_COLOR.finditer(line):
            stats["colors"][m.group(1).split("[")[1].rstrip("]").lower()] += 1

        for m in ARBITRARY_PX.finditer(line):
            px = float(m.group(1))
            if px % 4:
                add("off-grid-spacing", "LAW 10 Spacing", "warn", i,
                    f"{m.group(0)} is off the 4/8px grid.")
        for m in HALF_STEP.finditer(line):
            add("off-grid-spacing", "LAW 10 Spacing", "info", i,
                f"{m.group(0)} ({float(m.group(1)) * 4:g}px) is off the 8px rhythm.")

        for m in HARSH_SHADOW.finditer(line):
            add("harsh-shadow", "Mistake 2 Effects", "warn", i,
                f"`{m.group(0)}`: prefer a light, high-blur shadow or none.")
        if GRADIENT.search(line):
            add("gradient", "Mistake 2 Effects", "info", i,
                "Gradient: keep only if it carries meaning (not decoration).")
        for m in GLOW.finditer(line):
            add("glow-effect", "Mistake 2 Effects", "info", i, f"`{m.group(0)}`: check this effect is not noise.")

        for m in THICK_BORDER.finditer(line):
            add("thick-border", "LAW 10 Borders", "warn", i, f"`{m.group(0)}`: borders should be 1px.")

        for m in ROUNDED.finditer(line):
            stats["radii"][m.group(0)] += 1

        m = NATIVE_DIALOG.search(line)
        if m and "." not in line[max(0, m.start() - 1):m.start()]:
            add("native-dialog", "Interactive Feedback", "warn", i,
                f"Native {m.group(1)}(): use Toast / Modal for feedback and red confirmations.")

    # Multi-line checks on className attributes.
    for m in CLASS_ATTR.finditer(text):
        cls = next(g for g in m.groups() if g is not None)
        i = text.count("\n", 0, m.start())
        if "${" in cls:
            continue
        colored = [c for c in ARBITRARY_COLOR.findall(cls)]
        if colored and "dark:" not in cls:
            add("missing-dark-variant", "DARK MODE", "warn", i,
                f"Hard-coded colour ({colored[0]}) with no dark: variant.")

    # Buttons without hover / press / disabled feedback.
    for m in BUTTON_OPEN.finditer(text):
        end = text.find(">", m.end())
        tag = text[m.start(): end if end != -1 else m.end() + 400]
        cls_m = re.search(r"className\s*=\s*(\{`[^`]*`\}|\{[^}]*\}|\"[^\"]*\"|'[^']*')", tag)
        if not cls_m:
            continue
        cls = cls_m.group(1)
        # Shared class constants (e.g. {primaryButton}) are audited in ui.ts itself.
        if re.fullmatch(r"\{\s*[A-Za-z_]\w*\s*\}", cls) or "${" in cls and "hover:" not in cls:
            continue
        i = text.count("\n", 0, m.start())
        if "hover:" not in cls:
            add("no-hover-state", "FEEDBACK", "warn", i, "Button has no hover: state.")
        elif "active:" not in cls and "disabled" not in tag:
            add("no-press-state", "Micro-interactions", "info", i,
                "Button has no active: press state (e.g. active:scale-[0.97]).")

    # Numbers rendered without tabular figures.
    if NUMBER_RENDER.search(text) and "tabular-nums" not in text and path.suffix in {".tsx", ".jsx"}:
        i = text.count("\n", 0, NUMBER_RENDER.search(text).start())
        add("tabular-nums", "LAW 10 Typography", "info", i,
            "Formats numbers but never uses tabular-nums; columns of figures will jitter.")

    return out


MANUAL_CHECKLIST = [
    ("USER FLOW", "Sketch the flow; look for missing search, back, save, empty states."),
    ("REDUNDANCY", "Each metric appears once; delete cards with no interaction or unique data."),
    ("LAYOUT", "Tight left sidebar, actions collapsed to a triple-dot menu, two-column settings."),
    ("FORMS", "Sparse forms on large pages become modals; advanced options collapsed."),
    ("KPIs", "Label small, value large, trend arrow + %, sparkline, comparison context."),
    ("CHARTS", "OKLCH palette, clear axes, readable values."),
    ("DARK MODE", "Separate palette, doubled distance, dimmed text, brighter borders."),
    ("FEEDBACK", "Loading (spinner/skeleton), success toast, red destructive confirmation."),
    ("PRICING / LANDING / PLAYFUL", "Apply only if the product has these surfaces."),
]


def default_roots() -> list[Path]:
    for cand in ("frontend/src", "src"):
        if Path(cand).is_dir():
            return [Path(cand)]
    return [Path(".")]


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("paths", nargs="*", type=Path)
    ap.add_argument("--json", action="store_true", help="machine-readable output")
    ap.add_argument("--min-severity", choices=SEVERITY, default="info")
    ap.add_argument("--fail-on", choices=SEVERITY, help="exit 1 if any finding is at/above this level")
    ap.add_argument("--max-per-rule", type=int, default=15, help="cap listed findings per rule (0 = no cap)")
    args = ap.parse_args()

    roots = [p.resolve() for p in (args.paths or default_roots())]
    base = Path.cwd().resolve()
    stats = {"icon_libs": Counter(), "colors": Counter(), "radii": Counter()}
    findings: list[Finding] = []
    nfiles = 0
    for f in iter_files(roots):
        nfiles += 1
        findings += audit_file(f.resolve(), base, stats)

    # Codebase-wide checks.
    if len(stats["icon_libs"]) > 1:
        findings.append(Finding("mixed-icon-libs", "LAW 1 Icons", "error", "-", 0,
                                f"Several icon libraries in use: {dict(stats['icon_libs'])}. Pick one.", ""))
    if len(stats["radii"]) > 5:
        findings.append(Finding("radius-sprawl", "COMPONENTS", "warn", "-", 0,
                                f"{len(stats['radii'])} different corner radii: {dict(stats['radii'].most_common())}. "
                                "Use one radius per component size.", ""))
    if len(stats["colors"]) > 24:
        findings.append(Finding("colour-sprawl", "LAW 2 Colours", "warn", "-", 0,
                                f"{len(stats['colors'])} distinct hard-coded hex colours; move them to tokens.", ""))

    min_sev = SEVERITY[args.min_severity]
    findings = [f for f in findings if SEVERITY[f.severity] >= min_sev]
    findings.sort(key=lambda f: (-SEVERITY[f.severity], f.rule, f.file, f.line))
    counts = Counter(f.severity for f in findings)
    # Score by findings per file so large codebases are not penalised for size.
    weighted = counts["error"] * 5 + counts["warn"] + counts["info"] * 0.2
    score = max(0, round(100 - 10 * weighted / max(nfiles, 1)))

    if args.json:
        print(json.dumps({
            "files_scanned": nfiles, "score": score, "counts": dict(counts),
            "icon_libraries": dict(stats["icon_libs"]), "radii": dict(stats["radii"]),
            "hex_colours": dict(stats["colors"].most_common()),
            "findings": [asdict(f) for f in findings],
            "manual_checklist": [{"item": a, "check": b} for a, b in MANUAL_CHECKLIST],
        }, indent=2))
    else:
        by_rule: dict[str, list[Finding]] = defaultdict(list)
        for f in findings:
            by_rule[f.rule].append(f)
        print(f"UI audit: {nfiles} files scanned  |  score {score}/100  |  "
              f"{counts['error']} error, {counts['warn']} warn, {counts['info']} info\n")
        for rule, items in by_rule.items():
            head = items[0]
            print(f"[{head.severity.upper()}] {rule}  ({head.law})  x{len(items)}")
            shown = items if args.max_per_rule == 0 else items[: args.max_per_rule]
            for f in shown:
                loc = f"{f.file}:{f.line}" if f.line else f.file
                print(f"   {loc}  {f.message}")
                if f.snippet:
                    print(f"      {f.snippet}")
            if len(shown) < len(items):
                print(f"   ... {len(items) - len(shown)} more (use --max-per-rule 0)")
            print()
        print("Palette in use (top hex colours):",
              ", ".join(f"{c}x{n}" for c, n in stats["colors"].most_common(10)) or "none")
        print("Icon libraries:", dict(stats["icon_libs"]) or "none")
        print("\nManual review (not checkable statically):")
        for item, check in MANUAL_CHECKLIST:
            print(f"   [ ] {item:<28} {check}")

    if args.fail_on and any(SEVERITY[f.severity] >= SEVERITY[args.fail_on] for f in findings):
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
