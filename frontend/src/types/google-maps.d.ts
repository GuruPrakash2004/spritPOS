// Types for the Google Maps pieces the Company Master place picker uses (components/masters/ShopPlacePicker.tsx):
// the browser key from frontend/.env.local, the web components of @googlemaps/extended-component-library, and the
// small part of the Maps JavaScript API it calls. Only what the picker uses is typed.
import type { DetailedHTMLProps, HTMLAttributes } from 'react';

type GmpElementProps = DetailedHTMLProps<HTMLAttributes<HTMLElement>, HTMLElement> & {
  center?: string; zoom?: string; placeholder?: string; slot?: string;
  'map-id'?: string; 'solution-channel'?: string;
};

declare module 'react' {
  namespace JSX {
    interface IntrinsicElements {
      'gmpx-api-loader': GmpElementProps;
      'gmp-map': GmpElementProps;
      'gmp-advanced-marker': GmpElementProps;
      'gmpx-place-picker': GmpElementProps;
    }
  }
}

declare global {
  interface ImportMetaEnv {
    /** Google Maps browser key (Maps JavaScript API + Places API (New), restricted to this site). */
    readonly VITE_GOOGLE_MAPS_BROWSER_KEY?: string;
  }

  /** A place as the picker and Place.fetchFields give it. */
  interface GmpPlace {
    id: string;
    displayName?: string | null;
    formattedAddress?: string | null;
    location?: unknown;
    viewport?: unknown;
    fetchFields?: (req: { fields: string[] }) => Promise<unknown>;
  }
}
