<template>
  <div id="home">
    <Search id="search" :items="searchResults" @input="search" @selected="onItemSelected" />
    <Mapbox id="map" map-style="static/style/osm_liberty/osm_liberty.json" :lat="51.961563" :lng="7.628202" :zoom="14" />
  </div>
</template>

<script>
import { store, mutations } from '../store';
import mapboxgl from 'mapbox-gl';
import Mapbox from '../components/Mapbox.vue';
import Search from '../components/Search.vue';
import withPhotonSearchMixin from '../api/withPhotonSearchMixin';

export default {
  name: 'Home',
  components: {
    Mapbox,
    Search
  },
  mixins: [withPhotonSearchMixin],
  data: function () {
    return {
      results: []
    };
  },
  computed: {
    searchResults() {
      return store.searchResults;
    }
  },
  created: function () {
    mutations.setBaseUrl('http://localhost:9000');
  },
  methods: {
    search: async function (val) {
      var endpoint = `${store.baseUrl}/search`;
      var features = await this.searchPhotonAsync(endpoint, val);

      mutations.setSearchResults(features);
    },
    onItemSelected: function (val) {
      console.log(val);
      const map = store.activeMap;

      var marker = new mapboxgl.Marker().setLngLat(val.geometry.coordinates).addTo(map);

      map.jumpTo({ center: val.geometry.coordinates });
    },
    data: function () {
      return {};
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
