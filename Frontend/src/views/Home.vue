<template>
  <div id="home">
    <Search id="search" :items="searchResults" @input="search" @selected="onItemSelected" />
    <FloatingActionButton id="fab" @click="onFloatingActionButtonClicked" />
    <MapboxLoader id="map" v-bind="mapOptions">
      <template slot-scope="{ map }">
        <MapboxMarker v-for="marker in markers" :key="marker.id" :marker="marker" :map="map" />
        <MapboxLine v-for="line in lines" :id="line.id" :key="line.id" :map="map" :color.sync="line.color" :path.sync="line.path" />
        <MapboxLayer v-for="layer in layers" :id="layer.id" :key="layer.id" :map="map" :geojson="layer.geojson" />
      </template>
    </MapboxLoader>
    <input ref="file" type="file" style="display: none;" @change="handleFileUpload" />
  </div>
</template>

<script>
import { store, mutations } from '../store';
import { MapboxLoader, MapboxLine, MapboxMarker, MapboxLayer, Search, FloatingActionButton } from '../components';
import { searchPhotonAsync } from '../api/search-service';
import { kmlToGeoJsonAsync } from '../api/geojson-service';
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
    FloatingActionButton,
    MapboxLoader,
    MapboxMarker,
    MapboxLine,
    MapboxLayer,
    Search
  },
  data: function () {
    return {
      mapOptions: {
        mapStyle: URL_MAPBOX_STYLE,
        zoom: 14,
        center: LNGLAT_MUENSTER
      },
      markers: [],
      lines: [],
      layers: [],
      searchResults: []
    };
  },
  mounted: function () {
    this.addLine(LINE_WALK_THROUGH_MUENSTER);
    this.addMarker(MARKER_MUENSTER_CITY_CENTER);
  },
  methods: {
    onFloatingActionButtonClicked() {
      this.$refs.file.click();
    },
    async handleFileUpload(event) {
      var result = await kmlToGeoJsonAsync('http://localhost:9000/kml/toGeoJson', event.target.files[0]);

      this.addLayer({
        geojson: result
      });
    },
    search: async function (val) {
      var endpoint = URL_PHOTON_SEARCH;
      var features = await searchPhotonAsync(endpoint, val);

      this.searchResults = features;
    },
    onItemSelected: function (item) {
      if (isNullOrUndefined(item)) {
        return;
      }

      this.addMarker({
        lnglat: item.coordinates
      });
    },
    addLayer(layer) {
      var layer_id = this.getLastLayerId() + 1;

      this.layers.push({
        layer_id: layer_id,
        id: `Layer_${layer_id}`,
        ...layer
      });
    },
    addLine(line) {
      var line_id = this.getLastLineId() + 1;

      this.lines.push({
        line_id: line_id,
        id: `Line_${line_id}`,
        ...line
      });
    },
    addMarker(marker) {
      const marker_id = this.getLastMarkerId() + 1;

      this.markers.push({
        marker_id: marker_id,
        id: `mbgl_marker_${marker_id}`,
        ...marker
      });

      mutations.setMbglCenter(marker.coordinates);
    },
    getLastLineId: function () {
      if (isNullOrUndefined(this.lines)) {
        return 0;
      }

      if (this.lines.length == 0) {
        return 0;
      }

      return this.lines
        .map((x) => x.line_id)
        .reduce((a, b) => {
          return Math.max(a, b);
        });
    },

    getLastMarkerId: function () {
      if (isNullOrUndefined(this.markers)) {
        return 0;
      }

      if (this.markers.length == 0) {
        return 0;
      }

      return this.markers
        .map((x) => x.marker_id)
        .reduce((a, b) => {
          return Math.max(a, b);
        });
    },
    getLastLayerId: function () {
      if (isNullOrUndefined(this.layers)) {
        return 0;
      }

      if (this.layers.length == 0) {
        return 0;
      }

      return this.layers
        .map((x) => x.layer_id)
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

#fab {
  z-index: 1;
}
</style>
