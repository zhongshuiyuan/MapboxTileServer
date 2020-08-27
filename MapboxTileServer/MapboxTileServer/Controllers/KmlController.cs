// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MapboxTileServer.Extensions;
using MapboxTileServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MapboxTileServer.Controllers
{
    [ApiController]
    public class KmlController : ControllerBase
    {
        private readonly ILogger<KmlController> logger;
        private readonly IKmlConverterService kmlConverterService;

        public KmlController(ILogger<KmlController> logger, IKmlConverterService kmlConverterService)
        {
            this.logger = logger;
            this.kmlConverterService = kmlConverterService;
        }

        [HttpPost]
        [Route("/kml/toGeoJson")]
        public async Task<IActionResult> KmlToGeoJson([FromForm(Name = "file")] IFormFile file, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Uploaded a KML File ...");

            if (file == null)
            {
                return BadRequest();
            }

            var xml = await file.ReadAsStringAsync(); ;

            if (!kmlConverterService.ToGeoJson(xml, out string json))
            {
                return BadRequest();
            }

            return Content(json, "application/json");
        }

    }
}