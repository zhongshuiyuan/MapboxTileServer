<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>

export default {
  name: 'App',
  computed: {
    
  },
  methods: {
    data: function () {
      return {};
    }
  }
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
