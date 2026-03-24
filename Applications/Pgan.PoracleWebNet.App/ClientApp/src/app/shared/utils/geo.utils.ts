/**
 * Ray-casting point-in-polygon check.
 * @param point [lat, lng]
 * @param polygon array of [lat, lng] pairs forming a closed polygon
 */
export function pointInPolygon(point: [number, number], polygon: [number, number][]): boolean {
  const [y, x] = point;
  let inside = false;

  for (let i = 0, j = polygon.length - 1; i < polygon.length; j = i++) {
    const [yi, xi] = polygon[i];
    const [yj, xj] = polygon[j];

    const intersect = yi > y !== yj > y && x < ((xj - xi) * (y - yi)) / (yj - yi) + xi;
    if (intersect) inside = !inside;
  }

  return inside;
}

/**
 * Compute centroid of a polygon (simple average of all points).
 * @param polygon array of [lat, lng] pairs
 * @returns [lat, lng] centroid
 */
export function polygonCentroid(polygon: [number, number][]): [number, number] {
  if (polygon.length === 0) return [0, 0];

  let latSum = 0;
  let lngSum = 0;

  for (const [lat, lng] of polygon) {
    latSum += lat;
    lngSum += lng;
  }

  return [latSum / polygon.length, lngSum / polygon.length];
}

/**
 * Calculate polygon area in square kilometers using the spherical excess formula.
 * @param polygon array of [lat, lng] pairs
 * @returns area in km²
 */
export function polygonAreaSqKm(polygon: [number, number][]): number {
  if (polygon.length < 3) return 0;

  const toRad = (deg: number) => (deg * Math.PI) / 180;
  const R = 6371; // Earth radius in km

  // Spherical excess (Girard's theorem) for each triangle with vertex at origin
  let total = 0;
  for (let i = 0, j = polygon.length - 1; i < polygon.length; j = i++) {
    const [lat1, lng1] = polygon[j];
    const [lat2, lng2] = polygon[i];
    total += (toRad(lng2) - toRad(lng1)) * (2 + Math.sin(toRad(lat1)) + Math.sin(toRad(lat2)));
  }

  return Math.abs((total * R * R) / 2);
}

/**
 * Auto-detect which region contains a polygon's centroid.
 * @param polygon the drawn polygon coordinates
 * @param regions array of region geofences with their polygon paths
 * @returns the matching region or null
 */
export function detectRegion(
  polygon: [number, number][],
  regions: { id: number; name: string; displayName: string; path: [number, number][] }[],
): { id: number; name: string; displayName: string } | null {
  const centroid = polygonCentroid(polygon);
  for (const region of regions) {
    if (pointInPolygon(centroid, region.path)) {
      return { id: region.id, name: region.name, displayName: region.displayName };
    }
  }
  return null;
}
