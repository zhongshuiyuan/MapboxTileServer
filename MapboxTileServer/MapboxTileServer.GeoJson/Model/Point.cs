// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class Point : IGeometryNode
    {
        public readonly string Type = "Point";

        public readonly float[] Coordinates;

        public Point(float[] coordinates)
        {
            Coordinates = coordinates;
        }
    }
}