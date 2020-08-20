// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class GxCoords : IGeometryNode
    {
        public readonly string Type = "gx:Coords";

        public readonly float[][] Coordinates;

        public readonly string[] Times;


        public GxCoords(float[][] coordinates, string[] times)
        {
            Coordinates = coordinates;
            Times = times;
        }
    }
}