// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MapboxTileServer.Options
{
    public class Tileset
    {
        /// <summary>
        /// Path to the Dataset.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The Content-Type to be served.
        /// </summary>
        public string ContentType { get; set; }
    }
}
