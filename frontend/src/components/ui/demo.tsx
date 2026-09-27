import { Pattern as CChart21 } from "@/components/ui/c-chart-21";
import RunwayLoader from "@/components/ui/runway-loader";

export function RunwayLoaderDemo() {
  return (
    <div className="w-full h-screen">
      <RunwayLoader />
    </div>
  );
}

export default function CChart21Demo() {
  return (
    <div className="p-6 flex items-center justify-center">
      <CChart21 />
    </div>
  );
}
