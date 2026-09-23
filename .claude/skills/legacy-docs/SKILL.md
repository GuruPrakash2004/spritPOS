---
name: legacy-docs
description: Read the legacy vLQMS reference material in MAIN_Root/vLQMSAPP on this Mac. Covers extracting text from the help PDF, reading xlsx/docx specs, pulling screen names and captions out of vLQMS.exe, and listing the fields in auto-saved transaction drafts. Use when you need to know what a legacy screen did.
---

# Reading the legacy vLQMS files

The files live in `MAIN_Root/vLQMSAPP`. This Mac has no poppler, `pypdf` or `strings -e`, so use the recipes below.

**Never open or print** `*.ini` / `RECENT.INI`: they hold plaintext DB credentials. Write extracted output to the session scratchpad, not the repo.

## Help PDF → text (PDFKit via Swift)

```bash
swift .claude/skills/legacy-docs/pdf-text.swift "MAIN_Root/vLQMSAPP/Docs/VLQMS Help Doc.pdf" > "$SCRATCH/help.txt"
```

- The first run compiles the script, which takes about a minute. The Bash tool's default timeout is enough; macOS has no `timeout` command.
- Screenshots in the PDF come out as `￼` placeholders. The text around them explains the callouts.

## xlsx / docx specs (zip archives of XML)

```python
import zipfile, re
z = zipfile.ZipFile('MAIN_Root/vLQMSAPP/Docs/WebDash.xlsx')
print(re.findall(r'<sheet [^>]*name="([^"]+)"', z.read('xl/workbook.xml').decode()))
print(re.findall(r'<t[^>]*>([^<]*)</t>', z.read('xl/sharedStrings.xml').decode()))
# .docx: z.read('word/document.xml') and collect <w:t> runs
```

## Screen names and captions from vLQMS.exe

.NET stores form classes as ASCII and UI captions as UTF-16LE:

```python
import re
b = open('MAIN_Root/vLQMSAPP/App/vLQMS.exe', 'rb').read()
forms = sorted({m.decode() for m in re.findall(rb'\b[fF]rm[A-Za-z0-9]+', b)})
captions = sorted({m.decode('utf-16le') for m in re.findall(rb'(?:[\x20-\x7e]\x00){4,}', b)})
```

Form prefixes:
- `frmMst*`: masters.
- `frmTrn*`: transactions.
- `frmRpt*`: reports.
- `frmUtl*`: utilities and settings.

`frmTrnVch` and `frmMstGeneral` are generic engines, so many screens have no dedicated form.

## Real screen fields from auto-saved drafts

`App/AutoSave/<Module>/Doc_No_*.xml` are serialized C1FlexGrids:
- `*_vsfHdr.xml`: header.
- `*_vsfTot.xml`: totals.
- `*_vsfFtr.xml`: footer.
- the plain `Doc_No_*.xml`: the item grid.

Collect the `<Data>` values; the captions come first, e.g. `Inv No | SUPPINVNO | Text | …`.

## Reference images
`Docs/UI/*.jpg` are splash screens, not screen captures. For real layouts, use `frontend/VLQM/src/images/`.
