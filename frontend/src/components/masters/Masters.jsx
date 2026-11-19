// The simple masters: each is MasterCrud with a title, API path and field list. Add new
// code/name style masters here (see the add-master skill).

import { useState } from 'react';
import MasterCrud from './MasterCrud';
import ArrangeOrderDialog from './ArrangeOrderDialog';
import { arrangePackageFields } from '../../lib/packageLayout';

import {
  BRAND_FIELDS, CUSTOMER_FIELDS, ITEM_GROUP_FIELDS, LEDGER_FIELDS, LEDGER_GROUP_FIELDS, PACKAGE_FIELDS, SALES_PERSON_FIELDS, UOM_FIELDS,
} from './masterConfigs';

/** Any master described in lib/simpleMasters.js. */
export const SimpleMasterScreen = ({ spec }) => <MasterCrud key={spec.path} title={spec.title} subtitle={spec.subtitle} path={spec.path} fields={spec.fields} />;

// Master + its "Arrange ... Slno" option (the exe's Options menu of the Item Group and Package masters).
const WithArrange = ({ title, subtitle, path, fields, label, short, arrange }) => {
  const [open, setOpen] = useState(false);
  return (
    <>
      <MasterCrud title={title} subtitle={subtitle} path={path} fields={fields} arrange={arrange} options={[{ label, onClick: () => setOpen(true) }]} />
      {open && <ArrangeOrderDialog path={path} title={short} onClose={() => setOpen(false)} />}
    </>
  );
};

export const ItemGroupMaster = () => <WithArrange title="Item Group Master" subtitle="Item groups, liquor flag and loyalty eligibility." path="item-groups" fields={ITEM_GROUP_FIELDS} label="Arrange Group Slno" short="Item Group" />;
const UOM_HIDDEN = ['decimals'];   // the UOM Master does not show the Decimals box
export const UOMMaster = () => <MasterCrud title="UOM Master" subtitle="Units of measure." path="uoms" fields={UOM_FIELDS} hideExtras hideKeys={UOM_HIDDEN} />;
export const PackageMaster = () => <WithArrange title="Package Master" subtitle="Bottle sizes and litres per bottle for excise litre totals." path="packages" fields={PACKAGE_FIELDS} label="Arrange Pkg Slno" short="Package" arrange={arrangePackageFields} />;
export const BrandMaster = () => <MasterCrud title="Brand Master" subtitle="Brands used in brand-wise sales reports." path="brands" fields={BRAND_FIELDS} />;
export const CustomerMaster = () => <MasterCrud title="Customer Master" subtitle="Customers, loyalty card numbers and SMS preference." path="customers" fields={CUSTOMER_FIELDS} />;
export const LedgerGroupMaster = () => <MasterCrud title="Ledger Group Master" subtitle="Groups for ledgers (expenses, cash, bank…)." path="ledger-groups" fields={LEDGER_GROUP_FIELDS} />;
export const SalesPersonMaster = () => <MasterCrud title="Sales Person Master" subtitle="Sales persons chosen on each bill." path="sales-persons" fields={SALES_PERSON_FIELDS} />;
export const LedgerMaster = () => <MasterCrud title="Ledger Master" subtitle="Ledgers that Expense Entry posts against." path="ledgers" fields={LEDGER_FIELDS} />;
