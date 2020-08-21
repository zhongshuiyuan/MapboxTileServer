<template>
  <div id="app">
    <Mapbox v-bind:style="style" v-bind:center="center" v-bind:zoom="zoom" />
  </div>
</template>

<script>
import 'mapbox-gl/dist/mapbox-gl.css'
import Mapbox from './components/Mapbox.vue'

export default {
  name: 'App',
  components: {
    Mapbox,
  },
  computed: {},
  methods: {
    data: function () {
      return {
        map: {
          style:
            'http://localhost:9000/static/style/osm_liberty/osm_liberty.json',
          center: [7.628202, 51.961563],
          zoom: 14,
        },
      }
    },
  },
}
</script>

<style>
body,
html {
  font-family: Helvetica, Arial, sans-serif;
  background-color: #000;
  color: #ccc;
  margin: 0;
}

#app {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>
