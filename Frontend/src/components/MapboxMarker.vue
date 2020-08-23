<script>
import mapboxgl from 'mapbox-gl';

export default {
  props: {
    map: {
      type: Object,
      required: true
    },
    marker: {
      type: Object,
      required: true
    }
  },
  mounted() {
    var marker = new mapboxgl.Marker()
      .setLngLat(this.marker.lnglat)
      .setDraggable(this.marker.draggable)
      .setRotation(this.marker.rotation)
      .addTo(this.map);
  },
  render() {
  }
};
</script>
