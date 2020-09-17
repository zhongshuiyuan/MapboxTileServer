export const mapboxLineOptions = {
  id: {
    type: String,
    required: true,
  },
  path: {
    type: Array,
    required: true,
  },
  color: {
    type: String,
    required: false,
    default: "#33C9EB",
  },
};
