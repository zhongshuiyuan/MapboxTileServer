<template>
  <div id="map" />
</template>

<script>
import { store, mutations } from '../store';
import { options } from '../model/mapbox-options';
import mapboxgl from 'mapbox-gl';

export default {
  name: 'Mapbox',
  props: {
    ...options
  },
  mounted() {
    let m = new mapboxgl.Map({
      container: 'map',
      style: `${store.baseUrl}/${this.mapStyle}`, //"http://localhost:9000/static/style/osm_liberty/osm_liberty.json",
      center: [this.lng, this.lat],
      zoom: this.zoom
    });

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
<style scoped>
#map {
  position: absolute;
  top: 0;
  bottom: 0;
  width: 100%;
}
</style>
