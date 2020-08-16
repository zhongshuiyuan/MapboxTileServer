// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace MapboxTileServer.Options
{

    public class ApplicationOptions
    {
        public PhotonSettings Photon { get; set; }

        public IDictionary<string, Tileset> Tilesets { get; set; }
    }
}
