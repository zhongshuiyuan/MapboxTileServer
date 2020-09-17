export const mapboxMarkerOptions = {
  id: {
    type: String,
    required: true,
  },
  lnglat: {
    type: Array,
    required: true,
  },
  draggable: {
    type: Boolean,
    required: false,
    default: true,
  },
  rotation: {
    type: Number,
    required: false,
  },
};
