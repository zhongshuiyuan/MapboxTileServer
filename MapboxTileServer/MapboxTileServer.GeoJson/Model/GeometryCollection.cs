﻿// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.GeoJson.Model
{
    public class GeometryCollection : IGeometryNode
    {
        public readonly string Type = "GeometryCollection";

        public readonly IGeometryNode[] Geometries;

        public GeometryCollection(IGeometryNode[] geometries)
        {
            Geometries = geometries;
        }
    }
}