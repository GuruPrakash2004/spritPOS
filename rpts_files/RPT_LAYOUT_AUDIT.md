# Print formats vs legacy .rpt files: audit

Checked on 2026-09-28 against the legacy report files in `backend/Rpts/` (the gitignored copy of the desktop app's `Rpts` folder).
The web print layouts in `frontend/src/components/print/layouts/` were **not** built from these files: every mapped format differs from its `.rpt`.

## How the .rpt files were read

`backend/tools/rpt_inspect.py` reads a Crystal Reports file without Crystal. For the older file format it recovers, exactly:
the areas and sections in order with their heights, and every object in each section: the exact text of text objects,
the database field / formula / parameter each field shows, lines and boxes, and each object's width and height.
It cannot recover **left/top positions** (where an object sits across the row) or fonts, and objects are stored in creation order, not visual order.
25 of the 36 files use the newer Crystal format, whose layout stream is encrypted: nothing can be read from those without Crystal Reports.

## Mapped formats

| Registry id | .rpt file | Registry name / paper | Web layout used | .rpt readable | Real width | Verdict |
|---|---|---|---|---|---|---|
| fmtSPInvPrint | fmtSPInvPrint.rpt | POS 80mm Thermal Receipt (Standard) / 80mm Roll (3-Inch) | ThermalPosLayout | readable | 71 mm | Paper matches, content does not match (see below). |
| fmtSPInvPrint_GST | fmtSPInvPrint_GST.rpt | POS 80mm Thermal (with GST Recap) / 80mm Roll (3-Inch) | ThermalPosLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtSInv_GST | fmtSInv_GST.rpt | A4 GST Detailed Tax Invoice / A4 Portrait | A4GstTaxInvoiceLayout | readable | 71 mm | **Paper wrong**: .rpt is a ~71 mm roll, registry says A4 Portrait. Content does not match (see below). |
| fmtSInv | fmtSInv.rpt | A5 Half-Page Retail Invoice Slip / A5 Half-Sheet | A5SlipLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtSPInvPrint_watermark | fmtSPInvPrint_watermark.rpt | A4 / Slip Watermarked Duplicate Copy / A4 Sheet | A4GstTaxInvoiceLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtSInv_DMP | fmtSInv.rpt (DMP) | Dot Matrix 80-Column Continuous Slip / 8.5" Continuous Fanfold | DotMatrixLayout | file missing | — | **No such .rpt file.** The entry is invented; remove it or point it at a real format. |
| RptDSR | RptDSR.rpt | A4 Daily Summary Report (Executive Audit) / A4 Sheet | DsrAuditLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| RptDSR_S | RptDSR_S.rpt | POS 80mm DSR Summary Slip / 80mm Roll | ThermalPosLayout (DSR) | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| RptDSR_8.5 | RptDSR_8.5.rpt | Dot-Matrix 8.5" Continuous DSR Ledger / 8.5" Continuous Roll | DsrAuditLayout | readable | 72 mm | **Paper wrong**: .rpt is a ~72 mm slip, registry says an 8.5" (216 mm) dot-matrix roll. Content does not match (see below). |
| RptDropsDSR | RptDropsDSR.rpt | Cash Drops & Denomination Slip / 80mm Roll | ThermalPosLayout (DSR) | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| RptStkBal | RptStkBal.rpt | Stock Balance Matrix (Standard A4) / A4 Landscape | StockBalancePrintLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| RptStkBal_Without_CLvalue | RptStkBal_Without CLvalue.rpt | Physical Stock Count Sheet (Quantities Only) / A4 Portrait | StockBalancePrintLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| RptStkBal_with_Mrp_inv_rate | RptStkBal_with Mrp and inv rate.rpt | Dual Valuation (Cost vs Realizable MRP) / A4 Landscape | StockBalancePrintLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| RptStkBal_Biggsize | RptStkBal_Biggsize.rpt | Wide Stock Ledger (132-Column Continuous) / 15" Wide Continuous Stationery | StockBalancePrintLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtPB_A4 | fmtPB_A4.rpt | A4 Purchase Inward Goods Note / A4 Portrait | A4GstTaxInvoiceLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtPB | fmtPB.rpt | Purchase Bill Voucher Slip (A5 / 80mm) / A5 Half-Sheet | A5SlipLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtPI | fmtPI.rpt | KSBCL Purchase Indent Order Sheet / A4 Portrait | A4GstTaxInvoiceLayout | readable | 75 mm | **Paper wrong**: .rpt is a ~75 mm roll, registry says A4 Portrait. Content does not match (see below). |
| fmtSTKDED_A4 | fmtSTKDED_A4.rpt | Stock Deduction Voucher (A4 Formal) / A4 Portrait | A4GstTaxInvoiceLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtSTKDED | fmtSTKDED.rpt | POS 80mm Stock Deduction Slip / 80mm Roll | ThermalPosLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| fmtSR | fmtSR.rpt | Sales Return & Refund Slip (80mm Thermal) / 80mm Roll | ThermalPosLayout | readable | 77 mm | Paper matches, content does not match (see below). |
| fmtExp | fmtExp.rpt | Petty Cash Payment Voucher (80mm Thermal) / 80mm Roll | ThermalPosLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |
| UDR_A4_Landscape | UDRSalesItemwise.rpt | Analytical Register (A4 Landscape) / A4 Landscape | AnalyticalReportLayout | readable | 75 mm | **Paper wrong**: .rpt is a ~75 mm roll, registry says A4 Landscape. Content does not match (see below). |
| UDR_A4_Portrait | RptItemSalesDay.rpt | Standard Register (A4 Portrait) / A4 Portrait | AnalyticalReportLayout | encrypted | — | Cannot verify: newer encrypted Crystal format. Web layout is not derived from it. |

## Content differences in the readable formats

**fmtSPInvPrint (thermal sales bill) vs ThermalPosLayout**
- The .rpt prints: company name; address lines 1-4 on one line; "PH No." phone; "GSTIN :" number; "CASH BILL" in a box; "Bill No :", "Bill Dt:", "Sales Type:", " Time:" (print time);
  columns "#", "Item Name", "Qty", "Rate", "Amount" between two lines; rows SlNo, item name, qty, rate, amount;
  a box with "Total QTY:" and "SUB Total:"; a box with "Grand Total   :"; a line; "Thank You For Shopping"; "NOTE : No Return, No Exchange,No Claims on Breakages"; "vLQMS-2020 by Softvent.com".
- The web layout invents: licence line, "Counter / By / Customer / Mode" lines, item codes under names, litres in the total, discount and points lines, "NET PAYABLE", the amount in words,
  "Cash Paid / Card / UPI" lines, a barcode, "Liquor once sold cannot be returned or exchanged.", "Please Drink Responsibly • Don't Drink & Drive", "Softvent vLQMS • Powered by Cloud POS".
- It omits: the CASH BILL box, Sales Type, SUB Total, the NOTE and Thank You lines, the vLQMS footer.
- The GST variant computes and prints "CGST @ 9% / SGST @ 9%" on the bill total. No format charges that; fmtSInv_GST prints "GST EXEMPTED ON LIQUOR" and the bill's own CGST/SGST totals.
- When company details are missing it prints made-up ones ("THE LIQUOR HOUSE", "No. 42, Brigade Road", "+91 98450 12345", "29AAAAA0000A1Z5"), and an empty bill prints sample items.

**fmtSInv_GST vs A4GstTaxInvoiceLayout**: the .rpt is a ~71 mm roll bill (company, "PH NO:", "GSTIN No:", CASH BILL box, Bill No/Bill Dt/Time/Sales Type, per item: HSN, MRP, "GST@", "Cess @", "Item Total Amt :"; footer: "Item Sold :", "Total CGST:", "Total SGST:", "Total Cess:", "Grand Total :", "Receieved Amount:", "Pay Back Amount:", "GST EXEMPTED ON LIQUOR", "Thank You", NOTE, vLQMS footer). The web renders an A4 "Tax Invoice" with invented sections (Place of Supply, Original for Recipient, Terms & Conditions, Subject to Bangalore jurisdiction, amount in words).

**RptDSR_8.5 vs DsrAuditLayout**: the .rpt is a ~72 mm slip: company and address, "PhNo.", "GSTIN:", "Number of Invoices", "Total Invoice Value", "Cash Sales", "Card Sales", "Total Return Value", "Total Expenses", "Net Amount", "Sales of the Day" by "Item Group" / "Qty" / "Amount", "For" company, "Authorised Signatory", print date and time. The web renders an A4 audit sheet with invented rows and signature boxes (Chief Cashier, Store Manager, Licensee / Partner, "Excise Book Entry Checked").

**fmtPI vs A4GstTaxInvoiceLayout**: the .rpt is a ~75 mm slip titled "PURCHASE INDENT" with "PI No.", "PI Dt.", columns SN / Item Name / Qty / Rate / Amount and "Total :". The registry calls it an A4 "KSBCL Purchase Indent Order Sheet" and the web prints it with the sales Tax Invoice layout.

**fmtSR vs ThermalPosLayout**: the .rpt is titled "SALES RETURN" with "Ret No.", "Ret Dt.", "Time:", columns # / Item Name / Qty / Rate / Amount, "Total :" and "Grand Total". The web prints it as a sales bill.

**UDRSalesItemwise vs AnalyticalReportLayout**: the .rpt is ~75 mm wide; the registry calls it "Analytical Register (A4 Landscape)".

## What is needed for an exact match

Positions and fonts are not in the readable part of the files, and 25 files are encrypted. To reproduce each format exactly, export each one from the desktop app
(print preview → Export → PDF) with a sample document; the PDF gives every object's exact position, font and size.

## Decoded layouts (readable files)

### RptDSR_8.5.rpt

- **Report Header** (Area1)
  - Section1: height 2070 twips (36.5 mm)
    - Text Text20 (2175x226 twips): "Total Expenses"
    - Text Text24 (2175x240 twips): "Card Sales"
    - Text Text26 (2175x224 twips): "Cash Sales"
    - Text Text30 (2172x255 twips): "Number of Invoices "
    - Text Text29 (2175x226 twips): "Total Invoice Value"
    - Text Text3 (84x240 twips): ":"
    - Text Text2 (84x240 twips): ":"
    - Text Text4 (84x240 twips): ":"
    - Text Text5 (84x240 twips): ":"
    - Text Text6 (70x226 twips): ":"
    - Text Text7 (84x240 twips): ":"
    - Text Text10 (2175x226 twips): "Net Amount"
    - Text Text15 (2175x226 twips): "Total Return Value"
    - Text Text16 (84x226 twips): ":"
    - Field Field1 (1424x226 twips): {?mNoOfInv}
    - Field Field3 (1424x226 twips): {?mCashSales}
    - Field Field4 (1424x226 twips): {?mCardSales}
    - Field Field8 (1424x226 twips): {?mTotInvValue}
    - Field Field13 (1424x226 twips): {?mSalesRet}
    - Field Field16 (1424x226 twips): {?mTotExp}
    - Field Field18 (1424x226 twips): {?mNetAmt}
    - Line Line1 (4063 wide twips): line/box
  - Section6: height 299 twips (5.3 mm)
    - Text Text11 (1728x226 twips): "Item Group"
    - Text Text13 (765x240 twips): "Qty"
    - Text Text14 (1265x240 twips): "Amount"
    - Line Line2 (4035 wide twips): line/box
  - Section7: height 304 twips (5.4 mm)
    - Text Text12 (1658x240 twips): "Sales of the Day" " "
    - Line Line3 (1376 wide twips): line/box
- **Page Header** (Area2)
  - Section2: height 0 twips (0.0 mm), empty
- **Details** (Area3)
  - Section9: height 638 twips (11.3 mm)
    - Text Text1 (3881x240 twips): {?Add1} {?Add2}
    - Box Box1 (4110x4123 twips): line/box
    - Text Text19 (3926x241 twips): {?CompName}
  - Section10: height 242 twips (4.3 mm)
    - Field Field14 (3881x234 twips): {?Add3}
  - Section11: height 298 twips (5.3 mm)
    - Field Field15 (3911x234 twips): {?Add4}
  - Section3: height 319 twips (5.6 mm)
    - Text Text17 (3882x255 twips): "PhNo" "." {?phno}
  - Section8: height 334 twips (5.9 mm)
    - Text Text18 (3893x255 twips): "GSTIN" ":" {?gstin}
- **Report Footer** (Area4)
  - Section4: height 1338 twips (23.6 mm)
    - Text Text8 (3988x234 twips): "For " {?CompName}
    - Text Text9 (3900x240 twips): "Authorised Signatory"
    - Field Field9 (1056x213 twips): {Print Date}
    - Field Field7 (1072x213 twips): {Print Time}
    - Line Line4 (3990 wide twips): line/box
- **Page Footer** (Area5)
  - Section5: height 314 twips (5.5 mm)
    - Field Field10 (2070x226 twips): {RptUDR.Fld1}
    - Field Field11 (784x234 twips): {RptUDR.Fld2}
    - Field Field12 (1155x234 twips): {RptUDR.Fld3}
- **Area6** (Area6)

### RptDisSchmSales .rpt

- **Page Header** (Area2)
  - Section2: height 360 twips (6.3 mm)
    - Line Line1 (4464 wide twips): line/box
    - Text Text1 (1820x240 twips): " Total Disc Amt"
    - Text Text2 (2058x240 twips): "Item Name "
    - Line Line2 (4374 wide twips): line/box
- **Page Footer** (Area5)
  - Section5: height 234 twips (4.1 mm), empty
- **Report Header** (Area1)
  - Section1: height 1302 twips (23.0 mm)
    - Field Field13 (4278x372 twips): {?compname}
    - Field Field14 (4278x234 twips): {?add1}
    - Field Field15 (4278x234 twips): {?add2}
    - Field Field16 (4216x234 twips): {?add3}
- **Report Footer** (Area4)
  - Section4: height 486 twips (8.6 mm)
    - Line Line3 (4427 wide twips): line/box
- **Details** (Area3)
  - Section3: height 241 twips (4.3 mm)
    - Field Field7 (1824x226 twips): {RptUDR.Fld3}
    - Field Field8 (2170x234 twips): {RptUDR.Fld2}

### RptStkOPBal.rpt

- **Report Header** (Area1)
  - Section1: height 390 twips (6.9 mm)
    - Field Field5 (5520x226 twips): {RptHdrStk.PriceVariation}
    - Text Text6 (1680x270 twips): "Selected Variation"
    - Line Line20 (0x427 twips): line/box
    - Box Box4 (11490x1905 twips): line/box
    - Field Field16 (1695x226 twips): {Page N of M}
  - Section7: height 326 twips (5.8 mm)
    - Field Field4 (7314x236 twips): {RptHdrStk.HdrText}
    - Line Line2 (11475 wide twips): line/box
  - Section6: height 560 twips (9.9 mm)
    - Field Field10 (915x240 twips): {RptHdrStk.Fld5}
    - Field Field9 (990x240 twips): {RptHdrStk.Fld4}
    - Field Field8 (990x240 twips): {RptHdrStk.Fld3}
    - Field Field7 (4575x211 twips): {RptHdrStk.Fld2}
    - Field Field6 (2460x240 twips): {RptHdrStk.Fld1}
    - Line Line3 (11460 wide twips): line/box
    - Field Field18 (915x240 twips): {RptHdrStk.Fld6}
    - Line Line1 (11460 wide twips): line/box
- **Page Header** (Area2)
  - Section2: height 552 twips (9.7 mm)
    - Box Box1 (0x8 twips): line/box
    - Text Text1 (1679x240 twips): "Continued" " "
- **Details** (Area3)
  - Section3: height 1440 twips (25.4 mm)
    - Field Field2 (11265x360 twips): {RptHdrStk.Compname}
    - Field Field3 (11295x720 twips): {RptHdrStk.Address}
  - Section10: height 390 twips (6.9 mm)
    - Field Field1 (11295x285 twips): {RptHdrStk.RptName}
- **Report Footer** (Area4)
  - Section4: height 90 twips (1.6 mm), empty
- **Page Footer** (Area5)
  - Section5: height 360 twips (6.3 mm)
    - Field Field11 (2475x240 twips): {RptDtlStk.Fld1}
    - Field Field12 (4590x211 twips): {RptDtlStk.Fld2}
    - Field Field13 (975x240 twips): {RptDtlStk.Fld3}
    - Field Field14 (990x240 twips): {RptDtlStk.Fld4}
    - Field Field15 (915x240 twips): {RptDtlStk.Fld5}
    - Field Field17 (915x240 twips): {RptDtlStk.Fld6}
- **Area6** (Area6)

### RptStkPkgBal.rpt

- **Report Header** (Area1)
  - Section1: height 390 twips (6.9 mm)
    - Field Field5 (5520x226 twips): {RptHdrStk.PriceVariation}
    - Text Text6 (1680x270 twips): "Selected Variation"
    - Line Line20 (0x427 twips): line/box
    - Box Box4 (23370x1680 twips): line/box
    - Field Field16 (1695x226 twips): {Page N of M}
  - Section7: height 331 twips (5.8 mm)
    - Field Field4 (11490x236 twips): {RptHdrStk.HdrText}
    - Line Line2 (23362 wide twips): line/box
  - Section6: height 360 twips (6.3 mm)
    - Field Field10 (945x233 twips): {RptHdrStk.Fld5}
    - Field Field9 (945x233 twips): {RptHdrStk.Fld4}
    - Field Field8 (780x233 twips): {RptHdrStk.Fld3}
    - Field Field7 (3180x233 twips): {RptHdrStk.Fld2}
    - Field Field6 (2145x233 twips): {RptHdrStk.Fld1}
    - Line Line3 (23370 wide twips): line/box
    - Field Field17 (945x233 twips): {RptHdrStk.Fld6}
    - Field Field18 (945x233 twips): {RptHdrStk.Fld7}
    - Field Field19 (945x233 twips): {RptHdrStk.Fld8}
    - Field Field20 (945x233 twips): {RptHdrStk.Fld9}
    - Field Field21 (945x233 twips): {RptHdrStk.Fld10}
    - Field Field22 (945x233 twips): {RptHdrStk.Fld11}
    - Field Field23 (945x233 twips): {RptHdrStk.Fld12}
    - Field Field24 (945x233 twips): {RptHdrStk.Fld13}
    - Field Field25 (945x233 twips): {RptHdrStk.Fld14}
    - Field Field38 (945x233 twips): {RptHdrStk.Fld15}
    - Field Field26 (945x233 twips): {RptHdrStk.Fld16}
    - Field Field36 (945x233 twips): {RptHdrStk.Fld17}
    - Field Field42 (945x233 twips): {RptHdrStk.Fld18}
    - Field Field46 (945x233 twips): {RptHdrStk.Fld19}
    - Field Field47 (945x233 twips): {RptHdrStk.Fld20}
    - Line Line4 (23430 wide twips): line/box
- **Page Header** (Area2)
  - Section2: height 552 twips (9.7 mm)
    - Box Box1 (0x8 twips): line/box
    - Text Text1 (1679x240 twips): "Continued" " "
- **Details** (Area3)
  - Section3: height 1440 twips (25.4 mm)
    - Field Field2 (23250x360 twips): {RptHdrStk.Compname}
    - Field Field3 (23250x720 twips): {RptHdrStk.Address}
  - Section10: height 434 twips (7.7 mm)
    - Field Field1 (23235x285 twips): {RptHdrStk.RptName}
- **Report Footer** (Area4)
  - Section4: height 248 twips (4.4 mm), empty
- **Page Footer** (Area5)
  - Section5: height 323 twips (5.7 mm)
    - Field Field11 (2145x233 twips): {RptDtlStk.Fld1}
    - Field Field12 (3180x233 twips): {RptDtlStk.Fld2}
    - Field Field15 (945x233 twips): {RptDtlStk.Fld5}
    - Field Field14 (945x233 twips): {RptDtlStk.Fld4}
    - Field Field13 (780x233 twips): {RptDtlStk.Fld3}
    - Field Field27 (945x233 twips): {RptDtlStk.Fld6}
    - Field Field28 (945x233 twips): {RptDtlStk.Fld7}
    - Field Field29 (945x233 twips): {RptDtlStk.Fld8}
    - Field Field30 (945x233 twips): {RptDtlStk.Fld9}
    - Field Field31 (945x233 twips): {RptDtlStk.Fld10}
    - Field Field33 (945x233 twips): {RptDtlStk.Fld12}
    - Field Field34 (945x233 twips): {RptDtlStk.Fld13}
    - Field Field35 (945x233 twips): {RptDtlStk.Fld14}
    - Field Field39 (945x233 twips): {RptDtlStk.Fld15}
    - Field Field37 (945x233 twips): {RptDtlStk.Fld16}
    - Field Field40 (945x233 twips): {RptDtlStk.Fld17}
    - Field Field41 (945x233 twips): {RptDtlStk.Fld18}
    - Field Field44 (945x233 twips): {RptDtlStk.Fld19}
    - Field Field45 (945x233 twips): {RptDtlStk.Fld20}
    - Field Field32 (945x233 twips): {RptDtlStk.Fld11}
- **Area6** (Area6)

### UDRPurItem.rpt

- **Report Header** (Area1)
  - Section3: height 0 twips (0.0 mm), empty
- **Page Header** (Area2)
  - Section2: height 212 twips (3.7 mm), empty
- **Details** (Area3)
  - Section11: height 1099 twips (19.4 mm)
    - Field Field1 (3936x233 twips): {?compname}
    - Text Text1 (4016x236 twips): {?add3} " " {?add4}
    - Field Field19 (4000x226 twips): {?add2}
    - Field Field2 (4016x226 twips): {?add1}
  - Section13: height 274 twips (4.8 mm)
    - Text Text7 (2258x240 twips): "Item wise Purchase Report"
  - Section12: height 222 twips (3.9 mm)
    - Field Field3 (3938x192 twips): {RptUDRHeading.HdrText}
  - Section14: height 355 twips (6.3 mm)
    - Text Text4 (840x240 twips): "Tot Qty"
    - Text Text6 (885x240 twips): "Tot Amt"
    - Text Text3 (1658x234 twips): "Item Name"
    - Line Line3 (4085 wide twips): line/box
- **Report Footer** (Area4)
  - Section6: height 312 twips (5.5 mm)
    - Field Field5 (791x184 twips): {@SumofFld6}
    - Text Text5 (1650x208 twips): " Total "
    - Line Line4 (4085 wide twips): line/box
    - Line Line5 (4085 wide twips): line/box
    - Field Field8 (855x184 twips): {@SumofFld9}
  - Section4: height 212 twips (3.7 mm)
    - Text Text2 (1679x170 twips): "UDRPurItem.rpt"
- **Area7** (Area7)
  - Section8: height 281 twips (5.0 mm)
    - Field Field7 (3808x234 twips): (not decoded)
    - Line Line6 (4085 wide twips): line/box
    - Line Line7 (4085 wide twips): line/box
- **Area8** (Area8)
  - Section9: height 347 twips (6.1 mm)
    - Field Field4 (816x184 twips): (not decoded)
    - Text Text8 (1650x210 twips): " SUB Total "
    - Field Field6 (829x184 twips): (not decoded)
    - Line Line8 (4085 wide twips): line/box
- **Page Footer** (Area5)
  - Section5: height 319 twips (5.6 mm)
    - Field Field13 (2288x212 twips): {RptUDR.Fld5}
    - Field Field9 (746x192 twips): {@Fld9}
    - Field Field11 (704x192 twips): {@Fld6}
- **Area6** (Area6)

### UDRSalesItemwise.rpt

- **Report Header** (Area1)
  - Section3: height 0 twips (0.0 mm), empty
- **Page Header** (Area2)
  - Section2: height 0 twips (0.0 mm), empty
- **Details** (Area3)
  - Section11: height 88 twips (1.6 mm), empty
  - Section12: height 1049 twips (18.5 mm)
    - Field Field1 (3936x233 twips): {?compname}
    - Field Field19 (4000x226 twips): {?add2}
    - Field Field2 (4016x226 twips): {?add1}
    - Text Text1 (4016x236 twips): {?add3} " " {?add4}
  - Section13: height 269 twips (4.7 mm)
    - Text Text7 (2258x240 twips): "Item wise Sales Report"
  - Section14: height 247 twips (4.4 mm)
    - Field Field3 (3938x192 twips): {RptUDRHeading.HdrText}
  - Section15: height 310 twips (5.5 mm)
    - Text Text3 (1658x234 twips): "Item Name1"
    - Text Text6 (885x240 twips): "Tot Amt"
    - Text Text4 (840x240 twips): "Tot Qty"
    - Line Line2 (4085 wide twips): line/box
    - Line Line3 (4085 wide twips): line/box
- **Report Footer** (Area4)
  - Section4: height 343 twips (6.1 mm)
    - Line Line4 (4085 wide twips): line/box
    - Line Line5 (4245 wide twips): line/box
    - Field Field21 (858x227 twips): (not decoded)
    - Text Text8 (1650x256 twips): " Total "
    - Field Field9 (858x227 twips): (not decoded)
  - Section6: height 288 twips (5.1 mm)
    - Text Text2 (1844x248 twips): "UDRSalesItemwise.rpt"
- **Area7** (Area7)
  - Section8: height 234 twips (4.1 mm)
    - Field Field10 (3940x234 twips): (not decoded)
- **Area8** (Area8)
  - Section9: height 320 twips (5.6 mm)
    - Field Field18 (834x227 twips): (not decoded)
    - Line Line6 (4035 wide twips): line/box
    - Line Line7 (4035 wide twips): line/box
    - Text Text5 (1650x256 twips): "SUB Total "
    - Field Field8 (858x227 twips): (not decoded)
- **Page Footer** (Area5)
  - Section5: height 298 twips (5.3 mm)
    - Field Field4 (2160x234 twips): {RptUDR.Fld4}
    - Field Field6 (894x234 twips): {RptUDR.Fld5}
    - Field Field11 (858x234 twips): {@Fld10}
- **Area6** (Area6)

### fmtPI.rpt

- **Page Header** (Area2)
  - Section6: height 260 twips (4.6 mm)
    - Text Text8 (280x240 twips): "SN"
    - Text Text9 (1738x210 twips): "Item Name"
    - Text Text10 (490x210 twips): "Qty"
    - Text Text12 (714x240 twips): "Amount
" "
"
    - Line Line2 (3967 wide twips): line/box
    - Text Text5 (736x240 twips): "Rate"
- **Page Footer** (Area5)
  - Section14: height 240 twips (4.2 mm), empty
- **Report Header** (Area1)
  - Section3: height 0 twips (0.0 mm), empty
  - Section5: height 984 twips (17.4 mm)
    - Text Text11 (3720x255 twips): {?Add1} {?Add2} {?Add3} {?Add4} {?PHNO}
    - Text Text17 (3720x240 twips): "PURCHASE INDENT"
    - Text Text7 (3720x241 twips): {?CompName}
    - Line Line3 (4255 wide twips): line/box
  - Section4: height 307 twips (5.4 mm)
    - Text Text3 (1804x255 twips): "PI Dt. " {TrnHdrInv.PIDate} " "
    - Line Line1 (3927 wide twips): line/box
    - Line Line4 (4195 wide twips): line/box
    - Text Text1 (2100x255 twips): "PI No. " {TrnHdrInv.PINO}
- **Report Footer** (Area4)
  - Section2: height 283 twips (5.0 mm)
    - Line Line5 (3879 wide twips): line/box
    - Text Text14 (1078x240 twips): "Total "
    - Text Text23 (70x240 twips): ": "
    - Line Line6 (3901 wide twips): line/box
    - Field Field9 (858x234 twips): {TrnHdrInv.GRNDTOT}
- **Details** (Area3)
  - Section7: height 241 twips (4.3 mm)
    - Text Text13 (1724x213 twips): {MstItem.ITEMNAME}
    - Field Field2 (280x216 twips): {TrnDtlInv.SlNo}
    - Field Field3 (445x213 twips): {TrnDtlInv.PIQty}
    - Field Field1 (734x213 twips): {TrnDtlInv.PIRate}
    - Field Field4 (707x213 twips): {TrnDtlInv.PIAmt}

### fmtQTN.rpt

- **Page Header** (Area2)
  - Section6: height 300 twips (5.3 mm)
    - Text Text8 (280x223 twips): "SN"
    - Text Text9 (1700x210 twips): "Item Name"
    - Text Text10 (490x210 twips): "Qty"
    - Text Text12 (719x240 twips): "Amount
" "
"
    - Line Line2 (3879 wide twips): line/box
    - Text Text5 (658x240 twips): "Rate"
- **Page Footer** (Area5)
  - Section14: height 240 twips (4.2 mm), empty
- **Report Header** (Area1)
  - Section3: height 0 twips (0.0 mm), empty
  - Section5: height 1329 twips (23.4 mm)
    - Text Text7 (3870x241 twips): {?ADD1} {?ADD2} {?ADD3} {?ADD4}
    - Field Field9 (3900x234 twips): {?CompName}
    - Field Field10 (3900x234 twips): {?PHNO}
  - Section11: height 310 twips (5.5 mm)
    - Text Text17 (4005x240 twips): "QUOTATION"
    - Line Line3 (3990 wide twips): line/box
    - Line Line4 (3990 wide twips): line/box
  - Section4: height 654 twips (11.5 mm)
    - Text Text1 (2100x255 twips): "QTN No. " {TrnHdrInv.QTNNO}
    - Text Text3 (1815x255 twips): "QTN Dt. " {TrnHdrInv.QTNDate} " "
    - Line Line1 (3879 wide twips): line/box
    - Field Field14 (1157x234 twips): {Print Time}
    - Text Text11 (638x240 twips): "Time :"
- **Report Footer** (Area4)
  - Section2: height 313 twips (5.5 mm)
    - Line Line5 (3879 wide twips): line/box
    - Text Text23 (70x240 twips): ": "
    - Text Text14 (1078x240 twips): "Total "
    - Field Field6 (858x234 twips): {TrnHdrInv.TOTAL}
  - Section1: height 262 twips (4.6 mm)
    - Text Text2 (70x240 twips): ": "
    - Text Text6 (1083x240 twips): "Card Amt"
    - Field Field12 (858x234 twips): {TrnHdrInv.CRAMT}
  - Section9: height 300 twips (5.3 mm)
    - Text Text4 (70x240 twips): ": "
    - Text Text22 (1078x240 twips): "Cash Amt"
    - Field Field7 (858x234 twips): {TrnHdrInv.CASHAMT}
  - Section10: height 270 twips (4.8 mm)
    - Text Text18 (70x240 twips): ": "
    - Text Text16 (1078x240 twips): "Grand Total"
    - Field Field8 (858x234 twips): {TrnHdrInv.GRNDAMT}
  - Section8: height 892 twips (15.7 mm)
    - Text Text20 (3696x240 twips): "*** Thank You !! Visit Again ***"
    - Line Line6 (3921 wide twips): line/box
    - Text Text24 (560x224 twips): "NOTE :"
    - Text Text25 (3898x240 twips): "No Return , No Exchange , No Claims on Breakage"
- **Details** (Area3)
  - Section7: height 306 twips (5.4 mm)
    - Field Field1 (286x213 twips): {TrnDtlInv.SlNo}
    - Field Field2 (1720x213 twips): {MstItem.ITEMNAME}
    - Field Field3 (436x213 twips): {TrnDtlInv.QTNQty}
    - Field Field4 (573x213 twips): {TrnDtlInv.QTNRate}
    - Field Field5 (683x213 twips): {TrnDtlInv.QTNAmt}

### fmtSInv_GST.rpt

- **Page Header** (Area2)
  - Section6: height 256 twips (4.5 mm)
    - Text Text8 (128x223 twips): "#"
    - Text Text9 (912x210 twips): "Item Name"
    - Text Text10 (476x210 twips): "Qty"
    - Text Text12 (632x240 twips): "Amount
" "
"
    - Line Line2 (3872 wide twips): line/box
    - Line Line1 (3840 wide twips): line/box
    - Box Box4 (3912x255 twips): line/box
    - Text Text26 (429x208 twips): "Rate"
- **Page Footer** (Area5)
  - Section14: height 240 twips (4.2 mm), empty
- **Report Header** (Area1)
  - Section5: height 0 twips (0.0 mm), empty
  - Section25: height 165 twips (2.9 mm), empty
  - Section24: height 1059 twips (18.7 mm)
    - Field Field5 (3870x377 twips): {?compname}
    - Text Text11 (3888x234 twips): {?Add1} " " {?Add2} " " {?Add3} " " {?Add4}
  - Section22: height 241 twips (4.3 mm)
    - Text Text7 (3885x241 twips): "PH NO: " {?Phno}
  - Section23: height 240 twips (4.2 mm)
    - Text Text23 (3825x220 twips): "GSTIN No:" {?GSTIN}
  - Section9: height 340 twips (6.0 mm)
    - Line Line3 (3752 wide twips): line/box
    - Line Line4 (3912 wide twips): line/box
    - Text Text32 (1658x304 twips): "CASH BILL "
    - Box Box3 (3912x405 twips): line/box
  - Section4: height 340 twips (6.0 mm)
    - Text Text1 (716x248 twips): "Bill No. "
    - Text Text3 (642x225 twips): "Bill Dt:      " " "
    - Field Field8 (992x220 twips): {TrnHdrSInv.SINVDate}
    - Field Field9 (1426x186 twips): {TrnHdrSInv.SINVNO}
  - Section1: height 282 twips (5.0 mm)
    - Text Text15 (1912x186 twips): "Time:     " {Print Time}
    - Text Text2 (1908x220 twips): "Sales Type: " {TrnHdrSInv.SALTYP}
- **Report Footer** (Area4)
  - Section3: height 284 twips (5.0 mm)
    - Text Text14 (1072x176 twips): "Item Sold :"
    - Field Field1 (828x184 twips): {TrnHdrSInv.TOTQTY}
    - Box Box2 (3898x283 twips): line/box
  - Section19: height 228 twips (4.0 mm)
    - Text Text17 (1388x180 twips): "Total CGST:"
    - Field Field12 (1628x184 twips): {TrnHdrSInv.TOTCGSTAMT}
  - Section18: height 248 twips (4.4 mm)
    - Text Text29 (1376x180 twips): "Total SGST:"
    - Field Field13 (1647x184 twips): {TrnHdrSInv.TOTSGSTAMT}
  - Section17: height 244 twips (4.3 mm)
    - Text Text30 (1408x180 twips): "Total Cess:"
  - Section2: height 416 twips (7.3 mm)
    - Text Text6 (2000x315 twips): "Grand Total :"
    - Box Box1 (3744x400 twips): line/box
    - Field Field16 (1831x369 twips): {TrnHdrSInv.INVVALUE}
    - Box Box5 (3936x416 twips): line/box
  - Section20: height 242 twips (4.3 mm)
    - Text Text18 (1834x210 twips): "Receieved Amount:"
    - Field Field14 (1520x220 twips): {TrnHdrSInv.RCVDAMT}
  - Section11: height 242 twips (4.3 mm)
    - Text Text16 (1764x210 twips): "Pay Back Amount:"
    - Field Field15 (1527x220 twips): {TrnHdrSInv.PAYBACKAMT}
  - Section10: height 233 twips (4.1 mm)
    - Text Text21 (3606x218 twips): "GST EXEMPTED ON LIQUOR"
  - Section8: height 256 twips (4.5 mm)
    - Text Text20 (3672x240 twips): "Thank You"
  - Section27: height 480 twips (8.5 mm)
    - Text Text24 (4003x480 twips): "NOTE" " : No Return, No Exchange,No Claims on Breakage
"
  - Section21: height 210 twips (3.7 mm)
    - Text Text22 (3660x210 twips): "vLQMS-2020 by " "Softvent.com"
- **Details** (Area3)
  - Section7: height 269 twips (4.7 mm)
    - Field Field2 (297x170 twips): {TrnDtlSInv.SlNo}
    - Field Field3 (482x192 twips): {TrnDtlSInv.SINVQty}
    - Field Field7 (622x192 twips): {TrnDtlSInv.SINVRate}
    - Field Field6 (602x192 twips): {@Amt}
    - Field Field4 (1780x213 twips): {MstItem.ITEMNAME}
  - Section12: height 258 twips (4.6 mm)
    - Text Text4 (1596x180 twips): "HSN: " {MstItem.HSNCode}
    - Text Text5 (1554x191 twips): "MRP:  " {MstItem.MRP}
  - Section13: height 258 twips (4.6 mm)
    - Text Text13 (1918x210 twips): "GST@  " {@GSTPer}
    - Field Field10 (1176x184 twips): {@GSTAmt}
  - Section15: height 273 twips (4.8 mm)
    - Text Text25 (1918x225 twips): "Cess   @" " "
  - Section16: height 240 twips (4.2 mm)
    - Text Text27 (1679x192 twips): "Item Total Amt :"
    - Field Field17 (1185x199 twips): {TrnDtlSInv.NETTOT}
    - Line Line5 (3884 wide twips): line/box

### fmtSPInvPrint.rpt

- **Page Header** (Area2)
  - Section6: height 335 twips (5.9 mm)
    - Text Text8 (280x255 twips): "#"
    - Text Text9 (1711x240 twips): "Item Name"
    - Text Text10 (432x225 twips): "Qty"
    - Text Text12 (739x240 twips): "Amount
" "
"
    - Line Line2 (3990 wide twips): line/box
    - Line Line1 (3947 wide twips): line/box
    - Text Text16 (530x225 twips): "Rate"
- **Page Footer** (Area5)
  - Section14: height 51 twips (0.9 mm), empty
- **Report Header** (Area1)
  - Section1: height 0 twips (0.0 mm), empty
  - Section5: height 220 twips (3.9 mm), empty
  - Section9: height 480 twips (8.5 mm)
    - Field Field6 (3885x435 twips): {mSP_PrintInv;1.compname}
  - Section13: height 220 twips (3.9 mm)
    - Text Text11 (3870x220 twips): {mSP_PrintInv;1.add1} " " {mSP_PrintInv;1.add2} " " {mSP_PrintInv;1.add3} " " {mSP_PrintInv;1.add4}
  - Section3: height 220 twips (3.9 mm)
    - Text Text7 (3825x220 twips): "PH No." {mSP_PrintInv;1.PHNO}
  - Section8: height 286 twips (5.0 mm)
    - Text Text14 (3780x241 twips): "GSTIN :" {mSP_PrintInv;1.GSTNO}
  - Section12: height 337 twips (5.9 mm)
    - Text Text32 (1658x304 twips): "CASH BILL "
    - Box Box3 (3962x337 twips): line/box
  - Section4: height 555 twips (9.8 mm)
    - Text Text2 (2100x240 twips): "Sales Type: " {mSP_PrintInv;1.SALTYP} " "
    - Text Text3 (1830x240 twips): "Bill Dt:" "  " {mSP_PrintInv;1.SINVDate}
    - Text Text15 (1847x240 twips): " Time:   " {Print Time}
    - Text Text1 (2280x240 twips): "Bill No :" {mSP_PrintInv;1.SINVNO}
- **Report Footer** (Area4)
  - Section11: height 510 twips (9.0 mm)
    - Text Text21 (1410x225 twips): "Total QTY:"
    - Field Field5 (915x213 twips): {mSP_PrintInv;1.TOTQTY}
    - Text Text13 (1410x240 twips): "SUB Total:"
    - Box Box2 (3986x510 twips): line/box
    - Field Field10 (923x213 twips): {mSP_PrintInv;1.INVVALUE}
  - Section2: height 386 twips (6.8 mm)
    - Text Text6 (2228x315 twips): "Grand Total   :"
    - Box Box1 (3892x333 twips): line/box
    - Field Field9 (1575x326 twips): {mSP_PrintInv;1.INVVALUE}
  - Section10: height 945 twips (16.7 mm)
    - Line Line6 (4013 wide twips): line/box
    - Text Text20 (3672x234 twips): "Thank You For Shopping"
    - Text Text4 (3897x387 twips): "NOTE :" " No Return, No Exchange,No Claims on Breakages
"
    - Text Text5 (3853x195 twips): "vLQMS-2020 by " "Softvent.com"
- **Details** (Area3)
  - Section7: height 240 twips (4.2 mm)
    - Field Field1 (1770x184 twips): {mSP_PrintInv;1.itemname}
    - Field Field2 (455x184 twips): {mSP_PrintInv;1.SINVQty}
    - Field Field3 (645x184 twips): {mSP_PrintInv;1.SINVRate}
    - Field Field4 (735x184 twips): {mSP_PrintInv;1.SINVAmt}
    - Field Field7 (262x184 twips): {mSP_PrintInv;1.SlNo}

### fmtSR.rpt

- **Page Header** (Area2)
  - Section6: height 330 twips (5.8 mm)
    - Text Text8 (280x240 twips): "#"
    - Text Text9 (1695x210 twips): "Item Name"
    - Text Text10 (435x250 twips): "Qty"
    - Text Text12 (794x240 twips): "Amount
" "
"
    - Line Line2 (4340 wide twips): line/box
    - Text Text5 (658x240 twips): "Rate"
- **Page Footer** (Area5)
  - Section14: height 240 twips (4.2 mm), empty
- **Report Header** (Area1)
  - Section3: height 0 twips (0.0 mm), empty
  - Section1: height 1443 twips (25.5 mm)
    - Field Field8 (3870x300 twips): {?CompName}
    - Field Field9 (3861x234 twips): {?ADD1}
    - Field Field10 (3861x234 twips): {?ADD2}
    - Field Field11 (3861x234 twips): {?ADD3}
    - Field Field12 (3861x234 twips): {?PHNO}
  - Section5: height 370 twips (6.5 mm)
    - Text Text17 (3930x270 twips): "SALES RETURN"
    - Line Line3 (4037 wide twips): line/box
    - Line Line4 (4125 wide twips): line/box
  - Section4: height 525 twips (9.3 mm)
    - Text Text1 (2100x241 twips): "Ret No. " {TrnHdrSR.SRNO}
    - Text Text3 (1804x241 twips): "Ret Dt. " {TrnHdrSR.SRDate}
    - Text Text15 (1792x270 twips): "Time:" {Print Time}
    - Line Line1 (4020 wide twips): line/box
- **Report Footer** (Area4)
  - Section2: height 320 twips (5.6 mm)
    - Line Line5 (4125 wide twips): line/box
    - Text Text14 (1078x240 twips): "Total "
    - Text Text23 (171x240 twips): ": "
    - Line Line6 (4095 wide twips): line/box
    - Field Field7 (765x212 twips): {TrnHdrSR.TOTAMT}
  - Section10: height 407 twips (7.2 mm)
    - Text Text16 (2088x345 twips): "Grand Total"
    - Field Field13 (1410x277 twips): {TrnHdrSR.TOTAMT}
- **Details** (Area3)
  - Section7: height 306 twips (5.4 mm)
    - Field Field1 (1680x213 twips): {MstITEM.ITEMNAME}
    - Field Field3 (450x213 twips): {TrnDtlSR.SRQty}
    - Field Field4 (600x213 twips): {TrnDtlSR.SRRate}
    - Field Field5 (795x213 twips): {TrnDtlSR.SRAmt}
    - Field Field6 (273x213 twips): {TrnDtlSR.SlNo}
