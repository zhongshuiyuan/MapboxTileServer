<template>
  <div id="app">
    <Mapbox />
  </div>
</template>

<script>
import "mapbox-gl/dist/mapbox-gl.css";
import Mapbox from "./components/Mapbox.vue";

export default {
  name: "App",
  components: {
    Mapbox,
  },
  computed: {},
  methods: {
    data: function () {
      return {};
    },
  },
};
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
