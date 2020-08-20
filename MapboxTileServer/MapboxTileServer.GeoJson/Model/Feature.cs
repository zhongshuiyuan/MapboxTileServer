// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace MapboxTileServer.GeoJson.Model
{
    public class Feature : IGeoJson
    {
        public readonly string Type = "Feature";

        public readonly string Id;

        public readonly IGeometryNode Geometries;

        public readonly Dictionary<string, object> Properties;

        public Feature(string id, IGeometryNode geometries, Dictionary<string, object> properties)
        {
            Id = id;
            Geometries = geometries;
            Properties = properties;
        }
    }
}
