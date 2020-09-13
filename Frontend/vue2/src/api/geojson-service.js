export async function kmlToGeoJsonAsync(url, file) {
  var formData = new FormData();

  formData.append('file', file);

  var response = await fetch(url, {
    method: 'POST',
    body: formData
  });

  return await response.json();
}
