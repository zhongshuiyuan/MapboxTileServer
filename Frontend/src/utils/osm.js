export class OSM {
  static featureToString(feature) {
    var components = [];

    if (feature.properties.name) {
      components.push(feature.properties.name);
    }

    if (feature.properties.street && feature.properties.housenumber) {
      components.push(`${feature.properties.street} ${feature.properties.housenumber}`);
    }

    if (feature.properties.street && !feature.properties.housenumber) {
      components.push(feature.properties.street);
    }

    if (!feature.properties.postcode && feature.properties.city) {
      components.push(`${feature.properties.city}`);
    }

    if (feature.properties.postcode && feature.properties.city) {
      components.push(`${feature.properties.postcode} ${feature.properties.city}`);
    }

    if (feature.properties.country) {
      components.push(feature.properties.country);
    }

    return components.join(', ');
  }
}
