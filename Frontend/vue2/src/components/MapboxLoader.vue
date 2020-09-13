<template>
  <div>
    <div ref="mapboxMap" class="mapbox-map" />
    <template v-if="isReady">
      <slot :map="map" />
    </template>
  </div>
</template>

<script>
import 'mapbox-gl/dist/mapbox-gl.css';
import mapboxgl from 'mapbox-gl';
import { mapboxMapOptions } from '../model/mapbox-map-options';
import { store } from '../store';

export default {
  props: {
    ...mapboxMapOptions
  },

  data: function() {
    return {
      map: null,
      isReady: false
    };
  },
  computed: {
    mapCenter() {
      return store.mbglCenter;
    }
  },
  watch: {
    mapCenter(coordinates) {
      this.map.jumpTo({ center: coordinates });
    }
  },
  mounted() {
    this.initializeMap();
  },
  methods: {
    initializeMap() {
      var vm = this;

      const mapContainer = this.$refs.mapboxMap;

      this.map = new mapboxgl.Map({
        container: mapContainer,
        style: this.mapStyle,
        center: this.center,
        zoom: this.zoom
      });

      this.map.on('styledata', function () {
        if(!vm.isReady) {
          vm.isReady = true;
        }
      });
    }
  }
};
</script>

<style scoped>
.mapbox-map {
  width: 100%;
  height: 100%;
}
</style>
