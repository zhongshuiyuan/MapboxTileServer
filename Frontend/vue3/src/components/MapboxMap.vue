<template>
  <div ref="root" style="width: 100%; height: 100%">
    <slot v-if="ready"></slot>
  </div>
</template>

<script>
import mapboxgl from "mapbox-gl";
import { onMounted, onBeforeUnmount, ref, computed } from "vue";
import { mapboxMapOptions } from "../model/mapbox-map-options";

export default {
  props: {
    ...mapboxMapOptions,
  },
  setup(props) {
    const root = ref(null);
    const ready = ref(false);
    const mapRef = ref(null);

    const options = {
      center: props.center,
      zoom: props.zoom,
      style: props.mapStyle,
    };

    onMounted(async () => {
      mapRef.value = new mapboxgl.Map({
        container: root.value,
        style: options.style,
        center: options.center,
        zoom: options.zoom,
      });

      mapRef.value.on("styledata", function () {
        if (!ready.value) {
          ready.value = true;
        }
      });
    });

    onBeforeUnmount(() => {
      if (mapRef.value) {
        mapRef.value.remove();
      }
    });

    const mapObject = computed(() => mapRef.value);

    return { root, ready, mapObject };
  },
};
</script>
