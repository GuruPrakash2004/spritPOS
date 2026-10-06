// Company Master: the shop on Google Maps. Search with the box on the map (Google's place picker, from
// @googlemaps/extended-component-library) and pick the shop; its Google place id is saved with the company, so the
// Dashboard's Store Open / Closed (backend app/shop_hours.py) reads that exact listing. The saved place is shown with a
// marker when the screen opens.
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
type MapsGlobal = { maps: { importLibrary(name: string): Promise<{ Place: new (o: { id: string }) => GmpPlace }> } };

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

export default function ShopPlacePicker({ placeId, readOnly, onPick }: {
  placeId: string; readOnly: boolean; onPick: (place: PickedPlace) => void;
}) {
  const mapRef = useRef<MapElement>(null);
  const markerRef = useRef<MarkerElement>(null);
  const pickerRef = useRef<PickerElement>(null);
  const [error, setError] = useState('');
  const [ready, setReady] = useState(false);
  const pickRef = useRef(onPick);
  useEffect(() => { pickRef.current = onPick; }, [onPick]);

  const show = (place: GmpPlace) => {
    const map = mapRef.current, marker = markerRef.current;
    if (!map || !marker || !place.location) return;
    if (place.viewport && map.innerMap) map.innerMap.fitBounds(place.viewport);
    else { map.center = place.location; map.zoom = 17; }
    marker.position = place.location;
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
    mapRef.current?.innerMap?.setOptions({ mapTypeControl: false });
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

  // The saved place, shown when the screen opens.
  useEffect(() => {
    if (!ready || !placeId) return;
    const g = (window as unknown as { google?: MapsGlobal }).google;
    if (!g) return;
    g.maps.importLibrary('places')
      .then(async ({ Place }) => {
        const place = new Place({ id: placeId });
        await place.fetchFields?.({ fields: ['displayName', 'formattedAddress', 'location', 'viewport'] });
        show(place);
      })
      .catch(() => setError('The saved place could not be shown on the map.'));
  }, [ready, placeId]);

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
        <gmp-map ref={mapRef} center={BANGALORE} zoom="13" map-id="DEMO_MAP_ID" style={{ height: '100%' }}>
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
