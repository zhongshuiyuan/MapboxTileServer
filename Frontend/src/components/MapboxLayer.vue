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
      id: `geojson_${this.id}`,
      type: 'fill',
      source: `geojson_${this.id}`,
      paint: {
        'fill-outline-color': ['get', 'stroke'],
        'fill-color': ['get', 'fill'],
        'fill-opacity': ['get', 'fill-opacity'],
        
      }
    });
  },
  render() {}
};
</script>
