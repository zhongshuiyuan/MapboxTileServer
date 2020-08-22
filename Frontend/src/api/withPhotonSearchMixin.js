import { store, mutations } from '../store';
import { Functional } from '../utils/core';
import { OSM } from '../utils/osm';

function asSearchResult(feature) {
  return {
    caption: OSM.featureToString(feature),
    item: feature
  };
}

function asDistinctFeatures(features) {
  const groupedFeatures = Functional.groupBy(features, (x) => OSM.featureToString(x));
  const distinctFeatures = Array.from(groupedFeatures, ([key, value]) => value[0]);

  return distinctFeatures;
}

export default {
  methods: {
    searchPhotonAsync: async function (url, query) {
      const endpoint = new URL(url);
      const searchParams = new URLSearchParams({ q: query });

      endpoint.search = searchParams.toString();

      var response = await fetch(endpoint, { method: 'GET', mode: 'cors', cache: 'no-cache' });

      if (response.status === 200) {
        var data = await response.json();
        if (!!data.features) {
          return asDistinctFeatures(data.features).map((x) => asSearchResult(x));
        }
      }

      return [];
    }
  }
};
