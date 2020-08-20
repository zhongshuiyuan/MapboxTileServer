// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using MapboxTileServer.GeoJson.Model;

namespace MapboxTileServer.GeoJson
{
    /// <summary>
    /// This is a shameless copy from: https://github.com/tmcw/togeojson/blob/master/lib/kml.js, so there is 
    /// no need for taking additional dependencies on third-party libraries. Updates to kml.js should be 
    /// reflected here.
    /// </summary>
    public partial class GeoJsonConverter
    {
        private static readonly string[] Geotypes = new[] { "Polygon", "LineString", "Point", "Track", "gx:Track" };

        public string FromKml(XDocument document)
        {
            var styleMapIndex = new Dictionary<string, Dictionary<string, string>>();
            var styleByHash = new Dictionary<string, XElement>();
            var styleIndex = new Dictionary<string, string>();

            var placemarks = document
                .Elements("Placemark")
                .ToList();

            var styles = document
                .Elements("Style")
                .ToList();

            var styleMaps = document
                .Elements("StyleMap")
                .ToList();

            foreach (var style in styles)
            {
                var hash = GetMD5Hash(style.ToString());
                styleIndex["#" + style.Attribute("id").Value] = hash;

                styleByHash[hash] = style;
            }

            foreach (var styleMap in styleMaps)
            {
                styleIndex['#' + styleMap.Attribute("id").Value] = GetMD5Hash(styleMap.ToString());

                var pairs = styleMap.Elements("Pair").ToList();

                var pairsMap = new Dictionary<string, string>();

                foreach (var pair in pairs)
                {
                    pairsMap[pair.Element("key").Value] = pair.Element("styleUrl").Value;
                }

                styleMapIndex['#' + styleMap.Attribute("id").Value] = pairsMap;
            }

            List<Feature> features = new List<Feature>();

            foreach (var placemark in placemarks)
            {
                var feature = GetPlacemark(
                  placemark,
                  styleIndex,
                  styleMapIndex,
                  styleByHash
                );

                if (feature != null)
                {
                    features.Add(feature);
                }
            }

            return JsonSerializer.Serialize(features);
        }


        private float[] GetCoordinates(string value)
        {
            return Regex.Replace(value, @"s", "")
                .Split(",")
                .Select(x => float.Parse(x))
                .ToArray();
        }

        private float[][] GetCoordinatesArray(string value)
        {
            return Regex.Replace(value, @"s", "")
                .Split(" ")
                .Select(x => GetCoordinates(x))
                .ToArray();
        }

        private GeometryAndTimes GetGeometries(XElement root)
        {
            var geometries = new List<IGeometryNode>();
            var coordTimes = new List<string[]>();

            if (root.Element("MultiGeometry") != null)
            {
                return GetGeometries(root.Element("MultiGeometry"));
            }
            if (root.Element("MultiTrack") != null)
            {
                return GetGeometries(root.Element("MultiTrack"));
            }
            if (root.Element("gx:MultiTrack") != null)
            {
                return GetGeometries(root.Element("gx:MultiTrack"));
            }

            foreach (var geotype in Geotypes)
            {
                var geometryNodes = root
                    .Elements(geotype)
                    .ToList();

                if (geometryNodes.Count > 0)
                {
                    foreach (var geomNode in geometryNodes)
                    {
                        if (geotype == "Point")
                        {
                            var coordinates = GetCoordinates(geomNode.Element("coordinates").Value);
                            var point = new Point(coordinates);

                            geometries.Add(point);
                        }
                        else if (geotype == "LineString")
                        {
                            var coordinates = GetCoordinatesArray(geomNode.Element("coordinates").Value);
                            var lineString = new LineString(coordinates);

                            geometries.Add(lineString);
                        }
                        else if (geotype == "Polygon")
                        {
                            var rings = geomNode.Elements("LinearRing");

                            var coordinates = new List<float[]>();

                            foreach (var ring in rings)
                            {
                                var coordinatesOfRing = GetCoordinates(ring.Element("coordinates").Value);
                                coordinates.Add(coordinatesOfRing);
                            }

                            var polygon = new Polygon(coordinates.ToArray());

                            geometries.Add(polygon);
                        }
                        else if (geotype == "Track" || geotype == "gx:Track")
                        {
                            var track = GetGxCoords(geomNode);

                            var lineString = new LineString(track.Coordinates);
                            var times = track.Times;

                            geometries.Add(lineString);

                            if (times.Any())
                            {
                                coordTimes.Add(track.Times);
                            }

                        }
                    }
                }
            }

            return new GeometryAndTimes(geometries.ToArray(), coordTimes.ToArray());
        }

        private GxCoords GetGxCoords(XElement root)
        {
            var elements = root
                .Elements("coord")
                .ToList();

            var coordinates = new List<float[]>();
            var times = new List<string>();

            // Maybe this is a gx:coord node:
            if (elements.Count == 0)
            {
                elements = root
                    .Elements("gx:coord")
                    .ToList();
            }

            foreach (var element in elements)
            {
                var coordinatesForElement = element.Value
                    .Split(" ")
                    .Select(x => float.Parse(x))
                    .ToArray();

                coordinates.Add(coordinatesForElement);
            }

            var timeElems = root.Elements("when");

            foreach (var timeElem in timeElems)
            {
                times.Add(timeElem.Value);
            }

            return new GxCoords(coordinates.ToArray(), times.ToArray());
        }

        public void SetKmlColor(Dictionary<string, object> properties, XElement elem, string prefix)
        {
            string v = elem.Element("color") != null ? elem.Element("color").Value : "";

            string colorProp = (prefix == "stroke" || prefix == "fill") ? prefix : prefix + "-color";

            if (v.Substring(0, 1) == "#")
            {
                v = v.Substring(1);
            }

            if (v.Length == 6 || v.Length == 3)
            {
                properties[colorProp] = v;
            }
            else if (v.Length == 8)
            {
                properties[prefix + "-opacity"] = int.Parse(v.Substring(0, 2)) / 255.0f;
                properties[colorProp] = "#" + v.Substring(6, 2) + v.Substring(4, 2) + v.Substring(2, 2);
            }
        }

        private void SetNumericProperty(Dictionary<string, object> properties, XElement elem, string source, string target)
        {
            XElement node = elem.Element(source);

            if (node == null)
            {
                return;
            }

            var nodeValue = node.Value;

            if (string.IsNullOrWhiteSpace(nodeValue))
            {
                return;
            }

            if (float.TryParse(nodeValue, out float result))
            {
                if (!float.IsNaN(result))
                {
                    properties[target] = result;
                }
            }
        }

        private Feature GetPlacemark(XElement root, Dictionary<string, string> styleIndex, Dictionary<string, Dictionary<string, string>> styleMapIndex, Dictionary<string, XElement> styleByHash)
        {
            var geomsAndTimes = GetGeometries(root);
            var properties = new Dictionary<string, object>();

            // Values:
            var name = root.Element("name").Value;
            var address = root.Element("address").Value;
            var styleUrl = root.Element("styleUrl").Value;
            var description = root.Element("description").Value;

            // Nodes:
            var timeSpan = root.Element("'TimeSpan");
            var timeStamp = root.Element("TimeStamp");
            var iconStyle = root.Element("IconStyle");
            var labelStyle = root.Element("LabelStyle");
            var extendedData = root.Element("ExtendedData");
            var lineStyle = root.Element("LineStyle");
            var polyStyle = root.Element("PolyStyle");
            var visibility = root.Element("visibility");

            if (!geomsAndTimes.GeometryNodes.Any())
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                properties["name"] = name;
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                properties["address"] = address;
            }

            if (!string.IsNullOrWhiteSpace(styleUrl))
            {
                if (styleUrl[0] != '#')
                {
                    styleUrl = '#' + styleUrl;
                }

                properties["styleUrl"] = styleUrl;

                if (styleIndex.ContainsKey(styleUrl))
                {
                    properties["styleHash"] = styleIndex[styleUrl];
                }

                if (styleMapIndex.ContainsKey(styleUrl))
                {
                    properties["styleMapHash"] = styleMapIndex[styleUrl];
                    properties["styleHash"] = styleIndex[styleMapIndex[styleUrl]["normal"]];
                }

                // Try to populate the lineStyle or polyStyle since we got the style hash
                if (styleByHash.ContainsKey(properties["styleHash"].ToString()))
                {
                    var style = styleByHash.GetValueOrDefault<string, XElement>(properties["styleHash"].ToString(), null);

                    if (iconStyle == null)
                    {
                        iconStyle = style.Element("IconStyle");
                    }

                    if (labelStyle == null)
                    {
                        iconStyle = style.Element("LabelStyle");
                    }

                    if (lineStyle == null)
                    {
                        lineStyle = style.Element("LineStyle");
                    }

                    if (polyStyle == null)
                    {
                        polyStyle = style.Element("PolyStyle");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                properties["description"] = description;
            }

            if (timeSpan != null)
            {
                var begin = timeSpan.Element("begin").Value;
                var end = timeSpan.Element("end").Value;

                properties["timespan"] = new Dictionary<string, string>() {
                    { "begin", begin },
                    { "end", end }
                };
            }

            if (timeStamp != null)
            {
                properties["timestamp"] = timeStamp.Element("when").Value;
            }

            if (iconStyle != null)
            {
                SetKmlColor(properties, iconStyle, "icon");

                SetNumericProperty(properties, iconStyle, "scale", "icon-scale");
                SetNumericProperty(properties, iconStyle, "heading", "icon-heading");

                XElement hotspot = iconStyle.Element("hotSpot");

                if (hotspot != null)
                {
                    float left = float.Parse(hotspot.Attribute("x").Value);
                    float top = float.Parse(hotspot.Attribute("y").Value);
                    if (!float.IsNaN(left) && !float.IsNaN(top))
                    {
                        properties["icon-offset"] = new float[] { left, top };
                    }
                }

                var icon = iconStyle.Element("Icon");
                if (icon != null)
                {
                    var href = icon.Element("href")?.Value;
                    if (!string.IsNullOrWhiteSpace(href))
                    {
                        properties["icon"] = href;
                    }
                }
            }

            if (labelStyle != null)
            {
                SetKmlColor(properties, labelStyle, "label");
                SetNumericProperty(properties, labelStyle, "scale", "label-scale");
            }

            if (lineStyle != null)
            {
                SetKmlColor(properties, lineStyle, "stroke");
                SetNumericProperty(properties, lineStyle, "width", "stroke-width");
            }

            if (polyStyle != null)
            {
                SetKmlColor(properties, polyStyle, "fill");

                var fill = polyStyle.Element("fill").Value;
                var outline = polyStyle.Element("outline").Value;

                if (!string.IsNullOrWhiteSpace(fill))
                {
                    properties["fill-opacity"] = fill == "1" ? 1 : 0;
                }
                if (!string.IsNullOrWhiteSpace(outline))
                    properties["stroke-opacity"] = outline == "1" ? 1 : 0;
            }

            if (extendedData != null)
            {
                var datas = extendedData.Elements("Data");
                var simpleDatas = extendedData.Elements("SimpleData");

                foreach (var data in datas)
                {
                    properties[data.Attribute("name").Value] = data.Element("value").Value;
                }

                foreach (var simpleData in simpleDatas)
                {
                    properties[simpleData.Attribute("name").Value] = simpleData.Value;
                }
            }

            if (visibility != null)
            {
                properties["visibility"] = visibility.Value;
            }

            if (geomsAndTimes.GeometryNodes.Length > 0)
            {
                if (geomsAndTimes.Times.Length == 1)
                {
                    properties["coordTimes"] = geomsAndTimes.Times[0];
                }
                else
                {
                    properties["coordTimes"] = geomsAndTimes.Times;
                }
            }

            var id = root.Attribute("id") != null ? root.Attribute("id").Value : default;

            if (geomsAndTimes.GeometryNodes.Length == 1)
            {
                return new Feature(id, geomsAndTimes.GeometryNodes[0], properties);
            }

            var geometryCollection = new GeometryCollection(geomsAndTimes.GeometryNodes);

            return new Feature(id, geometryCollection, properties);
        }

        private static string GetMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}