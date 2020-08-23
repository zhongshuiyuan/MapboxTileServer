import { groupBy } from '../utils/core';
import { featureToString } from '../utils/osm';

function transformFeature(feature) {
  return {
    caption: featureToString(feature),
    coordinates: feature.geometry.coordinates,
    properties: feature.properties
  };
}

export class SearchService {
  static async searchAsync(url, query) {
    const endpoint = new URL(url);
    const searchParams = new URLSearchParams({ q: query });

    endpoint.search = searchParams.toString();

    var response = await fetch(endpoint, { method: 'GET', mode: 'cors', cache: 'no-cache' });

    if (response.status === 200) {
      var data = await response.json();
      if (!!data && !!data.features) {
        const allSearchResults = data.features.map(x => transformFeature(x));
        const groupedSearchResults = groupBy(allSearchResults, (x) => x.caption);
        const distinctSearchResults = Array.from(groupedSearchResults, ([key, value]) => value[0]);

        return distinctSearchResults;
      }
    }

    return [];
  }
}
