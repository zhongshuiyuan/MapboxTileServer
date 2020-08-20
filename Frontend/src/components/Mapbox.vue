<template>
  <div id="map"></div>
</template>

<script>
import { store, mutations } from "@/globals/store";
import mapboxgl from "mapbox-gl";

export default {
  name: "Mapbox",
  mounted() {
    // build the style url

    // instantiate Mapbox GL
    let m = new mapboxgl.Map({
      container: "map",
      style: "http://localhost:9000/static/style/osm_liberty/osm_liberty.json",
      center: [7.628202, 51.961563],
      zoom: 14,
    });

    m.on("load", function () {
      // TODO
    });

    m.on("styledata", function () {
      if (!store.mbglIsReady) {
        mutations.setMbglIsReady(true);
      }
    });
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
#map {
  position: absolute;
  top: 0;
  bottom: 0;
  width: 100%;
}
</style>
