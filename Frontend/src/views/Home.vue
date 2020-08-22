<template>
  <div id="home">
    <Mapbox map-style="static/style/osm_liberty/osm_liberty.json" :lat="51.961563" :lng="7.628202" :zoom="14" />
  </div>
</template>

<script>
import 'mapbox-gl/dist/mapbox-gl.css';
import { store, mutations } from '../store';
import Mapbox from '../components/Mapbox.vue';

export default {
  name: 'Home',
  components: {
    Mapbox
  },
  computed: {},
  created: function () {
    mutations.setBaseUrl('http://localhost:9000');
  },
  methods: {
    data: function () {
      return {};
    }
  }
};
</script>

<style scoped></style>
