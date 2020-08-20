// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class GeometryAndTimes
    {
        public readonly IGeometryNode[] GeometryNodes;
        public readonly string[][] Times;

        public GeometryAndTimes(IGeometryNode[] geometryNodes, string[][] times)
        {
            GeometryNodes = geometryNodes;
            Times = times;
        }
    }
}
