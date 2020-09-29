<template>
  <MapboxMap id="map" v-bind="mapOptions">
    <MapboxLine v-bind="lineData"></MapboxLine>
    <MapboxMarker v-bind="markerData" @dragend="onMarkerDragEnd"></MapboxMarker>
  </MapboxMap>
  <pre id="coordinates" class="coordinates"></pre>
</template>
<script>
import "mapbox-gl/dist/mapbox-gl.css";
import MapboxMap from "./components/MapboxMap";
import MapboxLine from "./components/MapboxLine";
import MapboxMarker from "./components/MapboxMarker";
import { ref } from "vue";
import {
  LINE_WALK_THROUGH_MUENSTER,
  LNGLAT_MUENSTER,
} from "./model/sample-data";

export default {
  components: {
    MapboxMap,
    MapboxLine,
    MapboxMarker,
  },
  setup() {
    const mapOptions = ref({
      mapStyle:
        "http://localhost:9000/static/style/osm_liberty/osm_liberty.json",
      zoom: 14,
      center: [7.628202, 51.961563],
    });

    const lineData = ref({
      id: 1,
      ...LINE_WALK_THROUGH_MUENSTER,
    });

    const markerData = ref({
      id: 2,
      lnglat: LNGLAT_MUENSTER,
      draggable: true,
      rotation: 0,
    });

    const onMarkerDragEnd = (event) => {
      console.log(event);
    };

    return {
      mapOptions,
      lineData,
      markerData,
      onMarkerDragEnd,
    };
  },
};
</script>

<style>
body {
  margin: 0;
}

#map {
  position: absolute;
  top: 0;
  bottom: 0;
  height: 100%;
  width: 100%;
}

.coordinates {
  background: rgba(0, 0, 0, 0.5);
  color: #fff;
  position: absolute;
  bottom: 40px;
  left: 10px;
  padding: 5px 10px;
  margin: 0;
  font-size: 11px;
  line-height: 18px;
  border-radius: 3px;
  display: none;
}
</style>
