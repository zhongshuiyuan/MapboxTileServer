// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class GxCoords
    {
        public string Type { get; private set; } = "gx:Coords";

        public float[][] Coordinates { get; set; }

        public string[] Times { get; set; }
    }
}