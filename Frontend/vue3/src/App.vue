<template>
  <MapboxMap id="map" v-bind="mapOptions">
    <MapboxLine v-bind="lineData"></MapboxLine>
  </MapboxMap>
</template>
<script>
import "mapbox-gl/dist/mapbox-gl.css";
import MapboxMap from "./components/MapboxMap";
import MapboxLine from "./components/MapboxLine";
import { ref } from "vue";
import { LINE_WALK_THROUGH_MUENSTER } from "./model/sample-data";

export default {
  components: {
    MapboxMap,
    MapboxLine,
  },
  setup() {
    const mapOptions = ref({
      mapStyle:
        "http://localhost:9000/static/style/osm_liberty/osm_liberty.json",
      zoom: 14,
      center: [7.628202, 51.961563],
    });

    const lineData = ref({
      id: 1,
      ...LINE_WALK_THROUGH_MUENSTER,
    });

    return {
      mapOptions,
      lineData,
    };
  },
};
</script>

<style>
body {
  margin: 0;
}

#map {
  position: absolute;
  top: 0;
  bottom: 0;
  height: 100%;
  width: 100%;
}
</style>
