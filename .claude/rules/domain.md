# Domain vocabulary

Terms used throughout the UI and expected to stay accurate.

## Store and regulation
- **KSBCL**: Karnataka State Beverages Corporation Ltd, the supplier of indents.
- **CL-2**: the retail liquor licence the store holds.
- **IML / BEER / IMPORTED**: item categories. The legacy **LIQTYPE** field uses IML / Beer / Wine / Others.
- **Indent**: an order raised on KSBCL. **GRN**: goods receipt note.
- **Excise**: liquor is outside GST in Karnataka, because excise is built into MRP. Liquor lines carry zero CGST/SGST; non-liquor items (soft drinks, cigarettes, snacks) do carry GST.
- **FY**: 01/04–31/03. Amounts are in ₹ with Indian digit grouping (lakh/crore).

## Legacy app terms
- **PB**: Purchase Bill. **SINV**: Sales Invoice. **CSE**: Closing Stock Entry. **DSR**: Daily Summary Report.
- **QOH**: quantity on hand.
- **PB Qty / PB Rate / PB Amt**: bottles purchased, rate per bottle, and Qty × Rate.
- **Cases / bottles**: stock is entered as cases plus loose bottles. Bottles/Case comes from Item Master and must be ≥ 1.
- **MRP**: maximum retail price. **ROL / ROQ**: reorder level / reorder quantity.
- **Litres per bottle**: from Package Master; drives the litre totals in stock and sales reports.
- **Counter**: a POS terminal (Sales Counter Master). A sales invoice cannot be raised without one.
- **Hold / New Bill**: park the current customer's bill and start another.
- **Copy From**: load lines from another document by doc no (e.g. Purchase Indent → Purchase Bill).
