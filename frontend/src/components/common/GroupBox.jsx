// A titled group of fields, like the group boxes of the desktop's forms: a header bar with the section's name over a
// bordered body. Used to separate the sections of a master or entry form (Item Details, Classification, Rates & Taxes ...).
//   <GroupBox title="Rates & Taxes"> ...fields... </GroupBox>
// Without a title it renders its children as they are.

export default function GroupBox({ title, children, className = '' }) {
  if (!title) return <div className={className}>{children}</div>;
  return (
    <fieldset className={`min-w-0 overflow-hidden rounded-lg border border-[#C9BCAB] bg-[#FFFFFF] dark:border-[#3A3A3A] dark:bg-[#141414] ${className}`}>
      <legend className="sr-only">{title}</legend>
      <div aria-hidden="true" className="border-b border-[#C9BCAB] bg-[#EADFCF] px-3 py-1.5 text-xs font-bold tracking-wide text-[#1B2B4B] dark:border-[#3A3A3A] dark:bg-[#1E1E1E] dark:text-[#D4A847]">{title}</div>
      <div className="p-3.5">{children}</div>
    </fieldset>
  );
}
