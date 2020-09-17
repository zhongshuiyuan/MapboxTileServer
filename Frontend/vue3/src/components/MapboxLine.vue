<script>
import { onMounted, onBeforeUnmount, ref, inject } from "vue";
import { mapboxLineOptions } from "../model/mapbox-line-options";

export default {
  props: {
    ...mapboxLineOptions,
  },
  setup(props) {
    const sourceRef = ref(null);
    const layerRef = ref(null);

    const mapObject = inject("mapObject");

    const options = {
      id: props.id,
      path: props.path,
      color: props.color,
    };

    onMounted(() => {
      sourceRef.value = mapObject.value.addSource(`line_${options.id}`, {
        type: "geojson",
        data: {
          type: "FeatureCollection",
          features: [
            {
              type: "Feature",
              properties: {
                color: options.color,
              },
              geometry: {
                type: "LineString",
                coordinates: options.path,
              },
            },
          ],
        },
      });

      layerRef.value = mapObject.value.addLayer({
        id: `line_${options.id}`,
        type: "line",
        source: `line_${options.id}`,
        paint: {
          "line-width": 3,
          // Use a get expression (https://docs.mapbox.com/mapbox-gl-js/style-spec/#expressions-get)
          // to set the line-color to a feature property value.
          "line-color": ["get", "color"],
        },
      });
    });

    onBeforeUnmount(() => {
      if (mapObject.value) {
        mapObject.value.removeLayer(`line_${mapObject.id}`);
        mapObject.value.removeSource(`line_${mapObject.id}`);
      }
    });

    return {};
  },
  render() {
    return null;
  },
};
</script>
