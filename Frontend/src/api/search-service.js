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
      // First check if we got data and features:
      if (!!data && !!data.features) {
        // We first transform the OSM Feature returned by Photon into something 
        // simpler, that can be consumed by some other code.
        const allSearchResults = data.features.map((x) => transformFeature(x));
        // Then we group by the caption of the result. We are doing this, because
        // we can have multiple items with the same name, say: The Street and a
        // Point of Interest, which will both resolve to the same name.
        const groupedSearchResults = groupBy(allSearchResults, (x) => x.caption);
        // Now the grouped items looks like this:
        //
        //  [
        //    [ "A", [ searchResultA, searchResultB ] ],
        //    [ "B", [ searchResultC ] ],
        //    [ "C", [ searchResultD, searchResultE ] ]
        //  ]
        //
        // So to have distinct search results, we are only taking the first search
        // result and discard the rest.
        return Array.from(groupedSearchResults, ([key, value]) => value[0]);
      }
    }

    return [];
  }
}
