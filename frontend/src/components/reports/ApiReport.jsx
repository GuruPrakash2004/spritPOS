// Renders one report from reportDefs by its module name. key= resets the filters when switching reports.

import ReportShell from './ReportShell';
import { REPORTS } from './reportDefs';

// Renders one report from reportDefs by its sidebar name. key= resets the filters when switching reports.
export default function ApiReport({ name }) {
  const def = REPORTS[name];
  return <ReportShell key={name} {...def} />;
}
