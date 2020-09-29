<script>
import mapboxgl from "mapbox-gl";
import { onMounted, onBeforeUnmount, ref, inject } from "vue";
import { mapboxMarkerOptions } from "../model/mapbox-marker-options";

export default {
  props: {
    ...mapboxMarkerOptions,
  },
  setup(props, context) {
    
    const markerRef = ref(null);

    const mapObject = inject("mapObject");

    const options = {
      id: props.id,
      lnglat: props.lnglat,
      draggable: props.draggable,
      rotation: props.rotation,
    };

    onMounted(() => {
      markerRef.value = new mapboxgl.Marker()
        .setLngLat(options.lnglat)
        .setDraggable(options.draggable)
        .setRotation(options.rotation)
        .addTo(mapObject.value);

      markerRef.value.on("dragend", () =>
        context.emit("dragend", {
          markerId: options.id,
          lnglat: markerRef.value.getLngLat(),
        })
      );
    });

    onBeforeUnmount(() => {
      if (mapObject.value) {
        mapObject.value.removeLayer(this.markerLayer);
      }
    });

    return {};
  },
  render() {
    return null;
  },
};
</script>
