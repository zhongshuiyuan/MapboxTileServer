<template>
  <MapboxMap id="map" v-bind="mapOptions"></MapboxMap>
</template>
<script>
import "mapbox-gl/dist/mapbox-gl.css";
import MapboxMap from "./components/MapboxMap";
import { ref } from "vue";

export default {
  components: {
    MapboxMap,
  },
  setup() {
    const mapOptions = ref({
      mapStyle:
        "http://localhost:9000/static/style/osm_liberty/osm_liberty.json",
      zoom: 14,
      center: [7.628202, 51.961563],
    });

    return {
      mapOptions,
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
