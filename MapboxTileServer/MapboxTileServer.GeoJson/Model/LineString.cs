// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class LineString : IGeometryNode
    {
        public readonly string Type = "LineString";

        public readonly float[][] Coordinates;

        public LineString(float[][] coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
