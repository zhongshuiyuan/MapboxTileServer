// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class Polygon : IGeometryNode
    {
        public readonly string Type = "Polygon";

        public readonly float[][] Coordinates;

        public Polygon(float[][] coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
