using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MapboxTileServer.GeoJson.Extensions
{
    public static class XmlExtensions
    {
        public static IEnumerable<XElement> ElementsL(this XElement source, string localName)
        {
            return source.Elements()
                .Where(e => e.Name.LocalName == localName);
        }

        public static XElement ElementL(this XElement source, string localName)
        {
            return source
                .ElementsL(localName)
                .FirstOrDefault();
        }
    }
}
