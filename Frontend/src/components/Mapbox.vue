<template>
  <div ref="mapboxgl" />
</template>

<script>
import 'mapbox-gl/dist/mapbox-gl.css';
import mapboxgl from 'mapbox-gl';
import { store, mutations } from '../store';
import { options } from '../model/mapbox-options';

export default {
  name: 'Mapbox',
  props: {
    ...options
  },
  mounted() {
    let m = new mapboxgl.Map({
      container: this.$refs.mapboxgl,
      style: `${store.baseUrl}/${this.mapStyle}`, //"http://localhost:9000/static/style/osm_liberty/osm_liberty.json",
      center: [this.lng, this.lat],
      zoom: this.zoom
    });

    mutations.setMap(m);

    m.on('load', function () {});

    m.on('styledata', function () {
      if (!store.mbglIsReady) {
        mutations.setMbglIsReady(true);
      }
    });
  },

  beforeDestroy() {
    this.$nextTick(() => {
      if (this.map) {
        this.map.remove();
      }
    });
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped></style>
