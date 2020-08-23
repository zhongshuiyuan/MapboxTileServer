<template>
  <div id="home">
    <Search id="search" :items="searchResults" @input="search" @selected="onItemSelected" />
    <MapboxLoader id="map" v-bind="mapOptions">
      <template slot-scope="{ map }">
        <MapboxMarker v-for="marker in markers" :key="marker.id" :marker="marker" :map="map" />
        <MapboxLine v-for="line in lines" :id="line.id" :key="line.id" :map="map" :color.sync="line.color" :path.sync="line.path" />
      </template>
    </MapboxLoader>
  </div>
</template>

<script>
import { store, mutations } from '../store';
import { MapboxLoader, MapboxLine, MapboxMarker, Search } from '../components';
import { searchPhotonAsync } from '../api/search-service';
import { isNullOrUndefined } from '../utils/core';
import {
  LNGLAT_MUENSTER,
  URL_MAPBOX_STYLE,
  URL_PHOTON_SEARCH,
  MARKER_MUENSTER_CITY_CENTER,
  LINE_WALK_THROUGH_MUENSTER
} from '../model/sample-data';

export default {
  name: 'Home',
  components: {
    MapboxLoader,
    MapboxMarker,
    MapboxLine,
    Search
  },
  data: function () {
    return {
      mapOptions: {
        mapStyle: URL_MAPBOX_STYLE,
        zoom: 14,
        center: LNGLAT_MUENSTER
      },
      markers: [MARKER_MUENSTER_CITY_CENTER],
      lines: [LINE_WALK_THROUGH_MUENSTER],
      searchResults: []
    };
  },
  methods: {
    search: async function (val) {
      var endpoint = URL_PHOTON_SEARCH;
      var features = await searchPhotonAsync(endpoint, val);

      this.searchResults = features;
    },
    onItemSelected: function (item) {
      if (isNullOrUndefined(item)) {
        return;
      }

      const markerId = this.getLastMarkerId() + 1;

      this.markers.push({
        id: `${markerId}`,
        lnglat: item.coordinates
      });

      mutations.setMbglCenter(item.coordinates);
    },
    getLastMarkerId: function () {
      if (isNullOrUndefined(this.markers)) {
        return 0;
      }

      return this.markers
        .map((x) => x.id)
        .reduce((a, b) => {
          return Math.max(a, b);
        });
    }
  }
};
</script>

<style scoped>
#map {
  position: absolute;
  top: 0;
  bottom: 0;
  height: 100%;
  width: 100%;
}

#search {
  position: relative;
  margin: 15px;
  z-index: 1;
}
</style>
