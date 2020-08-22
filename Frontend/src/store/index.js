import Vue from "vue";

export const store = Vue.observable({
  activeMap: null,
  baseUrl: null,
  mbglIsReady: false
});

export const mutations = {
  setMap(map) {
    store.mbglIsReady = false;
    store.activeMap = map;
  },
  setBaseUrl(val) {
      store.baseUrl = val;
  },
  setMbglIsReady(val) {
    store.mbglIsReady = val;
  }
};