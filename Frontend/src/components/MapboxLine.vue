<script>
import { mapboxLineOptions } from '../model/mapbox-line-options';

export default {
  props: {
    map: {
      type: Object,
      required: true
    },
    ...mapboxLineOptions
  },

  mounted() {
    this.map.addSource(`line_${this.id}`, {
      type: 'geojson',
      data: {
        type: 'FeatureCollection',
        features: [
          {
            type: 'Feature',
            properties: {
              color: this.color
            },
            geometry: {
              type: 'LineString',
              coordinates: this.path
            }
          }
        ]
      }
    });

    this.map.addLayer({
      id: `line_${this.id}`,
      type: 'line',
      source: `line_${this.id}`,
      paint: {
        'line-width': 3,
        // Use a get expression (https://docs.mapbox.com/mapbox-gl-js/style-spec/#expressions-get)
        // to set the line-color to a feature property value.
        'line-color': ['get', 'color']
      }
    });
  },
  render() {}
};
</script>
