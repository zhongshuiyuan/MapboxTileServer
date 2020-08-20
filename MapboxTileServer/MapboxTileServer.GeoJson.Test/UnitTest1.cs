using NUnit.Framework;
using System;
using System.IO;

namespace MapboxTileServer.GeoJson.Test
{
    public class Tests
    {

        private readonly string[] testFileList = new[]
        {
            "addresses.kml",
            "cdata.kml",
            "extended_data.kml",
            "gxmultitrack.kml",
            "gxtrack.kml",
            "inline_style.kml",
            "linestring.kml",
            "literal_color.kml",
            "multigeometry.kml",
            "multigeometry_discrete.kml",
            "multitrack.kml",
            "nogeomplacemark.kml",
            "non_gx_multitrack.kml",
            "noname.kml",
            "opacity_override.kml",
            "point.kml",
            "point_id.kml",
            "polygon.kml",
            "selfclosing.kml",
            "simpledata.kml",
            "style.kml",
            "style_url.kml",
            "timespan.kml",
            "twopoints.kml"
        };

        [Test]
        public void RunFiles()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            foreach(var filename in testFileList)
            {
                var filePath = Path.Combine(basePath, "Resources", "Kml", filename);

                var xml = File.ReadAllText(filePath);

                var json = GeoJsonConverter.FromKml(xml);
            }
        }
    }
}