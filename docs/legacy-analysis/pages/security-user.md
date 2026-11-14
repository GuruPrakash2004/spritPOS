# User master: `frmMstUser` (1,560 lines, read in full) and `frmMstUserAdmPwdChange` (48) vs Admin Master → Users

Web: `AdminUsersTab.jsx`, `AdminMaster.jsx`, `routers/auth.py` (`/admin/users`, `/admin/adnl-admin-password`), `schemas.UserIn`.
Not yet audited: `frmMstRole` (973), `frmMstRights` (835), `frmMstFldRights` (278): next pass.

## Done in this pass
| Legacy | Web now |
|---|---|
| Confirm Password required and equal: "Confirm Password should be same as password!!" (353-364) | Confirm password box on the user form (checked in the browser before the password is encrypted) |

## Already matching
Code required (322), duplicate code refused (337-344), e-mail checked when typed "EmailID is not valid!!" (381-386), inactive needs a reason "Reason for InActive!" (376), role must exist and be active (388-399), DOB not in the future "Not a valid DOB" (1475), department / mobile / DOB / password reset date fields (283-293, 552-559), edit keeps the code, delete asks first and one Administrator must remain, Change Adnl. ADMIN Password asks the current password and refuses an identical new one (frmMstUserAdmPwdChange 8-22).

## Differences kept on purpose
- Passwords: the exe stores them reversibly (`svtCrypto.Encrypt`) and shows them (eye icon 1494-1510); the web hashes them (PBKDF2), never shows or reads them back, and checks the Login Settings rules (min length 8, mix) instead of `UtlMisc.PwdMin*` (409-510).
- Per-user rights button (`btnRights`, only when `gblnUserBased`, 109-115, 1357-1364): the web gives rights by role only (Admin Master design).
- Additional ADMIN password lives in the web's own store, not `MstUser.AdmLoginPwd` of user 1.
- Search box + list (900-1000), First / Previous / Next / Last, Master Approvals / Approve (1394-1428): the web's user list and the Master Approvals feature cover these.

## Open (not built)
1. User photo: click the box, jpg only, under 3 MB, saved in `MstUser.UserImg`; Remove Image (1172-1310).
2. Name follows Code when leaving Code (1133); user name length / character rules (web: 2-20 letters, digits, `_ . -`).
3. Delete reference check `blnDeleteOK` (710-713): the web only blocks deleting yourself or the last Administrator.
4. Role link button to open Role Master (1160-1171).
