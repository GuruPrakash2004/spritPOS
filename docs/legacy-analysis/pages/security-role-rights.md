# Role master, Rights and Field Rights: `frmMstRole` (973 lines), `frmMstRights` (835), `frmMstFldRights` (278), all read in full

Web: Admin Master (Security) tabs **Roles & rights**, **Desktop rights** (`DesktopRightsTab.jsx`, `/admin/desktop-rights`), **Field rights** (`/admin/field-rights`), `routers/auth.py`.

## Role master (`frmMstRole`)
A generic code / name / inactive master (+ Doc Designer fields) with a Rights button (ADMIN only, 96-100) that opens `frmMstRights` for the role.
| Legacy | Web |
|---|---|
| Role code and name required, duplicate refused "<Role Name> already exists.." (260-279) | Role name required; duplicate refused ("A role named ... already exists") |
| Inactive flag + remarks "Please Enter Remarks For Why the Record is Inactive!" (285-289); the user form lists only active roles (frmMstUser 175) | **Open**: web roles have no Inactive flag |
| Delete asks first, `blnDeleteOK` reference check (449-478) | Match in effect: a role with users cannot be deleted ("Move this role's users to another role first"); Administrator cannot be deleted |
| Code is not editable after save; name copies the code on leaving it (869-871) | By design: web edits the role name (renames users along with it) |
| Approve (Master Approvals), search, First / Last, attachments | By design: web uses Master Approvals and the role pick-list |

## Rights (`frmMstRights`) vs Desktop rights tab
Already built on the exe's tables (`MstUserRights` / `MstRoleRights`, string of 0/1 flags: Add, Edit, Delete, Print, View, Auth, Auth1, Opt-1..5): choose user / role, type filter, name filter, click a column header to flip every visible row, Copy From another user (nothing saved until Save), Save rewrites the owner's rows (101-142, 744-792).
| Legacy | Web |
|---|---|
| Copy From `ADMIN` ticks every box (754-759) | Verify in the browser: Copy From lists the other owners; ADMIN's rights come from "always every right" |
| Transpose panel: pick a menu, tick the rights of every user for that menu at once, Save per menu (356-440, 564-635); ADMIN row hidden | **Open**: not built (the web edits one user / role at a time) |
| Editing the screen needs the ADMIN user or View-bit of menu 201 (451-472) | By design: Admin Master is Administrator-only |
| Menu list = `MstMenu` where `menushow = 'Y'`, ordered by type then caption (193-195) | Match (the web reads the same table) |

## Field Rights (`frmMstFldRights`)
Pick a Master or Transaction document, load its fields (Standard columns marked `SR` in the column description, plus Doc Designer fields), give each field a level Opt 1 .. Opt 5 or none, Save to `UtlMstSpecialRights` (DocID, FldCode, Std, RightsNumber) (92-139, 160-226). The exe then makes a field editable only for users who hold the matching Opt-N right of that screen (`chkSplRights_Std`, `chkSplRights_DOC` on Edit).
Web: a different model, role -> master -> field -> hidden | read-only (`store.FIELD_RIGHTS`), enforced by the server (`security.apply_field_rights`). `UtlMstSpecialRights` is not read or written anywhere in the web.
**Open (needs owner's decision):** honour the exe's `UtlMstSpecialRights` (read the table; a field with level N is read-only unless the signed-in user's role / user has the Opt-N right of that screen in the Desktop rights), alongside the web's own Field rights. Cross-cutting: it belongs in the Doc Designer engine audit. Not started, as it can't be tested without a shop database that has rows in that table.
