// The simple masters: each is MasterCrud with a title, API path and field list. Add new
// code/name style masters here (see the add-master skill).

import MasterCrud from './MasterCrud';
import {
  BRAND_FIELDS, CUSTOMER_FIELDS, ITEM_GROUP_FIELDS, LEDGER_FIELDS, LEDGER_GROUP_FIELDS, PACKAGE_FIELDS, SALES_PERSON_FIELDS, UOM_FIELDS,
} from './masterConfigs';

export const ItemGroupMaster = () => <MasterCrud title="Item Group Master" subtitle="Item groups, liquor flag and loyalty eligibility." path="item-groups" fields={ITEM_GROUP_FIELDS} />;
export const UOMMaster = () => <MasterCrud title="UOM Master" subtitle="Units of measure." path="uoms" fields={UOM_FIELDS} />;
export const PackageMaster = () => <MasterCrud title="Package Master" subtitle="Bottle sizes and litres per bottle for excise litre totals." path="packages" fields={PACKAGE_FIELDS} />;
export const BrandMaster = () => <MasterCrud title="Brand Master" subtitle="Brands used in brand-wise sales reports." path="brands" fields={BRAND_FIELDS} />;
export const CustomerMaster = () => <MasterCrud title="Customer Master" subtitle="Customers, loyalty card numbers and SMS preference." path="customers" fields={CUSTOMER_FIELDS} />;
export const LedgerGroupMaster = () => <MasterCrud title="Ledger Group Master" subtitle="Groups for ledgers (expenses, cash, bank…)." path="ledger-groups" fields={LEDGER_GROUP_FIELDS} />;
export const SalesPersonMaster = () => <MasterCrud title="Sales Person Master" subtitle="Sales persons chosen on each bill." path="sales-persons" fields={SALES_PERSON_FIELDS} />;
export const LedgerMaster = () => <MasterCrud title="Ledger Master" subtitle="Ledgers that Expense Entry posts against." path="ledgers" fields={LEDGER_FIELDS} />;
