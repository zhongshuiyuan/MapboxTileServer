import Vue from "vue";

export const store = Vue.observable({
  activeMap: null,
  mbglIsReady: false,
});

export const mutations = {
  setMbglIsReady(val) {
    store.mbglIsReady = val;
  },
};
