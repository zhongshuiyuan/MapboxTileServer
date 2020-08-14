// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MapboxTileServer.Options;
using MapboxTileServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MapboxTileServer.Controllers
{
    [ApiController]
    public class TilesController : ControllerBase
    {
        private readonly ILogger<TilesController> logger;
        private readonly ApplicationOptions applicationOptions;
        private readonly IMapboxTileService mapboxTileService;

        public TilesController(ILogger<TilesController> logger, IOptions<ApplicationOptions> applicationOptions, IMapboxTileService mapboxTileService)
        {
            this.logger = logger;
            this.applicationOptions = applicationOptions.Value;
            this.mapboxTileService = mapboxTileService;
        }

        [HttpGet]
        [Route("/tiles/{tileset}/{z}/{x}/{y}")]
        public ActionResult Get([FromRoute(Name = "tileset")] string tiles, [FromRoute(Name = "z")] int z, [FromRoute(Name = "x")] int x, [FromRoute(Name = "y")] int y)
        {
            if(!applicationOptions.Tilesets.TryGetValue(tiles, out Tileset tileset))
            {
                return BadRequest();
            }

            var data = mapboxTileService.Read(tileset, z, x, y);
            
            if(data == null)
            {
                return Accepted();
            }

            if (tileset.ContentType == Constants.MimeTypes.ApplicationMapboxVectorTile)
            {
                Response.Headers.Add("Content-Encoding", "gzip");
            }

            return new FileContentResult(data, tileset.ContentType);
        }
    }
}
