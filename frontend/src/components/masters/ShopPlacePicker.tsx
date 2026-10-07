// Company Master: the shop on Google Maps. Search with the box on the map (Google's place picker, from
// @googlemaps/extended-component-library) and pick the shop; its Google place id is saved with the company, so the
// Dashboard's Store Open / Closed (backend app/shop_hours.py) reads that exact listing. The saved place is shown with a
// marker when the screen opens. With no place saved yet, the shop is looked up from its name and address (`query`)
// and shown as the default, without saving anything until a place is picked.
//
// It runs in the browser, so it uses a separate browser key: VITE_GOOGLE_MAPS_BROWSER_KEY in frontend/.env.local
// (Maps JavaScript API + Places API (New), restricted to this site). The backend's server key is never sent here.

import { useEffect, useRef, useState } from 'react';
import { MapPin } from 'lucide-react';

const LIBRARY = 'https://ajax.googleapis.com/ajax/libs/@googlemaps/extended-component-library/0.6.11/index.min.js';
const BROWSER_KEY = import.meta.env.VITE_GOOGLE_MAPS_BROWSER_KEY?.trim() ?? '';
const BANGALORE = '12.9716,77.5946';   // until a place is picked

export type PickedPlace = { id: string; name: string; address: string };

type MapElement = HTMLElement & { innerMap?: { setOptions(o: object): void; fitBounds(b: unknown): void }; center?: unknown; zoom?: number };
type MarkerElement = HTMLElement & { position?: unknown };
type PickerElement = HTMLElement & { value?: GmpPlace | null };
type PlaceClass = (new (o: { id: string }) => GmpPlace) & { searchByText?: (req: { textQuery: string; fields: string[]; maxResultCount?: number }) => Promise<{ places: GmpPlace[] }> };
type MapsGlobal = { maps: { importLibrary(name: string): Promise<{ Place: PlaceClass }> } };

// The component library is a module script loaded once for the whole app.
let loading: Promise<void> | null = null;
function loadLibrary(): Promise<void> {
  if (!loading) {
    loading = new Promise((resolve, reject) => {
      const s = document.createElement('script');
      s.type = 'module';
      s.src = LIBRARY;
      s.onload = () => resolve();
      s.onerror = () => { loading = null; reject(new Error('Google Maps could not be loaded')); };
      document.head.appendChild(s);
    });
  }
  return loading;
}

/** Warm up Google's script (and its Maps and Places libraries) in the background, after sign-in, so the map is ready. */
// eslint-disable-next-line react-refresh/only-export-components
export function prefetchMaps(): void {
  if (!BROWSER_KEY) return;
  loadLibrary().catch(() => { /* the screen shows the error if the map is opened */ });
}

// The shop's last known spot, kept for this browser tab so the pin shows at once on the next visit.
type Spot = { location: unknown; viewport?: unknown };
const spotKey = (k: string) => `vlqms_shop_spot:${k}`;
const readSpot = (k: string): Spot | null => {
  try { const v = sessionStorage.getItem(spotKey(k)); return v ? JSON.parse(v) as Spot : null; } catch { return null; }
};
const saveSpot = (k: string, place: GmpPlace) => {
  const j = (o: unknown) => (o && typeof (o as { toJSON?: () => unknown }).toJSON === 'function' ? (o as { toJSON: () => unknown }).toJSON() : o);
  try { sessionStorage.setItem(spotKey(k), JSON.stringify({ location: j(place.location), viewport: j(place.viewport) })); } catch { /* blocked */ }
};

export default function ShopPlacePicker({ placeId, query = '', readOnly, onPick }: {
  placeId: string; query?: string; readOnly: boolean; onPick: (place: PickedPlace) => void;
}) {
  const mapRef = useRef<MapElement>(null);
  const markerRef = useRef<MarkerElement>(null);
  const pickerRef = useRef<PickerElement>(null);
  const [error, setError] = useState('');
  const [ready, setReady] = useState(false);
  const pickRef = useRef(onPick);
  useEffect(() => { pickRef.current = onPick; }, [onPick]);

  const show = (place: Pick<GmpPlace, 'location' | 'viewport'>) => {
    const map = mapRef.current, marker = markerRef.current;
    if (!map || !marker || !place.location) return;
    try {
      if (place.viewport && map.innerMap) map.innerMap.fitBounds(place.viewport);
      else { map.center = place.location; map.zoom = 17; }
      marker.position = place.location;
    } catch (e) { console.warn('The shop could not be shown on the map', e); }
  };

  useEffect(() => {
    if (!BROWSER_KEY) return;
    let live = true;
    loadLibrary()
      .then(() => Promise.all([customElements.whenDefined('gmp-map'), customElements.whenDefined('gmpx-place-picker')]))
      .then(() => { if (live) setReady(true); })
      .catch(e => { if (live) setError(e instanceof Error ? e.message : 'Google Maps could not be loaded'); });
    return () => { live = false; };
  }, []);

  // A place picked in the search box.
  useEffect(() => {
    const picker = pickerRef.current;
    if (!ready || !picker) return;
    try { mapRef.current?.innerMap?.setOptions({ mapTypeControl: false }); } catch { /* cosmetic */ }
    const onChange = () => {
      const place = picker.value;
      if (!place?.location) { setError('Google Maps has no details for that place. Pick one from the list.'); return; }
      setError('');
      show(place);
      pickRef.current({ id: place.id, name: place.displayName ?? '', address: place.formattedAddress ?? '' });
    };
    picker.addEventListener('gmpx-placechange', onChange);
    return () => picker.removeEventListener('gmpx-placechange', onChange);
  }, [ready]);

  // The saved place, shown when the screen opens (the last known spot first, then Google's answer).
  useEffect(() => {
    if (!ready || !placeId) return;
    const cached = readSpot(placeId);
    if (cached) show(cached);
    const g = (window as unknown as { google?: MapsGlobal }).google;
    if (!g) return;
    g.maps.importLibrary('places')
      .then(async ({ Place }) => {
        const place = new Place({ id: placeId });
        await place.fetchFields?.({ fields: ['displayName', 'formattedAddress', 'location', 'viewport'] });
        show(place);
        saveSpot(placeId, place);
      })
      .catch(() => setError('The saved place could not be shown on the map.'));
  }, [ready, placeId]);

  // No place saved yet: show the shop found by its name and address as the default view (looked up once the typing
  // stops, and remembered for this tab).
  useEffect(() => {
    const q = query.trim();
    if (!ready || placeId || !q) return;
    const cached = readSpot('q:' + q);
    if (cached) { show(cached); return; }
    const g = (window as unknown as { google?: MapsGlobal }).google;
    if (!g) return;
    let live = true;
    const timer = window.setTimeout(() => {
      g.maps.importLibrary('places')
        .then(async ({ Place }) => {
          if (!Place.searchByText) return;
          const { places } = await Place.searchByText({ textQuery: q, fields: ['displayName', 'formattedAddress', 'location', 'viewport'], maxResultCount: 1 });
          if (live && places[0]) { show(places[0]); saveSpot('q:' + q, places[0]); }
        })
        .catch(() => { /* the map simply stays on its starting view */ });
    }, 600);
    return () => { live = false; window.clearTimeout(timer); };
  }, [ready, placeId, query]);

  if (!BROWSER_KEY) {
    return (
      <p className="flex items-start gap-2 rounded-lg bg-subtle px-3 py-2 text-xs text-stone-500 dark:text-stone-400">
        <MapPin size={14} className="mt-0.5 shrink-0" />
        The map needs a Google Maps browser key: VITE_GOOGLE_MAPS_BROWSER_KEY in frontend/.env.local (Maps JavaScript
        API and Places API (New), restricted to this site). Until then the shop is found on Google Maps by its name and address.
      </p>
    );
  }
  return (
    <div className="space-y-1.5">
      <div className="h-72 overflow-hidden rounded-xl border border-line">
        {/* "key" is React's own prop, so the loader's key attribute is set on the element directly */}
        <gmpx-api-loader ref={el => { el?.setAttribute('key', BROWSER_KEY); }} solution-channel="GMP_GE_mapsandplacesautocomplete_v2" />
        {/* center and zoom are set as attributes: React would assign them as properties, and the map wants a LatLng object there */}
        <gmp-map ref={el => { mapRef.current = el as MapElement | null; if (el && !el.hasAttribute('center')) { el.setAttribute('center', BANGALORE); el.setAttribute('zoom', '13'); } }} map-id="DEMO_MAP_ID" style={{ height: '100%' }}>
          {!readOnly && (
            <div slot="control-block-start-inline-start" className="p-3">
              <gmpx-place-picker ref={pickerRef} placeholder="Search for the shop on Google Maps" />
            </div>
          )}
          <gmp-advanced-marker ref={markerRef} />
        </gmp-map>
      </div>
      {error && <p role="alert" className="text-[11px] text-red-600 dark:text-red-400">{error}</p>}
    </div>
  );
}
