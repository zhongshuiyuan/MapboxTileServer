<script>
export default {
  props: {
    map: {
      type: Object,
      required: true
    },
    id: {
      type: String,
      required: true
    },
    geojson: {
      type: Object,
      required: true
    }
  },
  mounted() {
    this.map.addSource(`geojson_${this.id}`, {
      type: 'geojson',
      data: this.geojson });

    this.map.addLayer({
      id: `geojson_polygons_${this.id}`,
      type: 'fill',
      source: `geojson_${this.id}`,
      paint: {
        'fill-outline-color': ['case', ['has', 'stroke'], ['get', 'stroke'], '#088'],
        'fill-color': ['case', ['has', 'fill'], ['get', 'fill'], '#088'],
        'fill-opacity': ['case', ['has', 'fill-opacity'], ['get', 'fill-opacity'], 0.8]
      },
      'filter': ['==', '$type', 'Polygon']
    });
    
    this.map.addLayer({
      id: `geojson_points_${this.id}`,
      type: 'circle',
      source: `geojson_${this.id}`,
      paint: {
        'circle-radius': ['case', ['has', 'circle-radius'], ['get', 'circle-radius'], 6],
        'circle-color': ['case', ['has', 'circle-color'], ['get', 'circle-color'], "#088"]
      },
      'filter': ['==', '$type', 'Point']
    });

        this.map.addLayer({
      id: `geojson_lines_${this.id}`,
      type: 'line',
      source: `geojson_${this.id}`,
      paint: {
        'line-width': 3,
        // Use a get expression (https://docs.mapbox.com/mapbox-gl-js/style-spec/#expressions-get)
        // to set the line-color to a feature property value.
        'line-color': ['case', ['has', 'color'], ['get', 'color'], "#088"]
      },
      'filter': ['==', '$type', 'LineString']
    });
  },
  render() {}
};
</script>
