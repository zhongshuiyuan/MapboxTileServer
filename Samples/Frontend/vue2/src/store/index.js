import Vue from "vue";

export const store = Vue.observable({
  mbglCenter: null
});

export const mutations = {
  setMbglCenter(val) {
    store.mbglCenter = val;
  }
};