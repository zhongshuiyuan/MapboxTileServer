// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MapboxTileServer.Clients;
using MapboxTileServer.Options;
using MapboxTileServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace MapboxTileServer.Controllers
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<TilesController> logger;
        private readonly ApplicationOptions applicationOptions;
        private readonly PhotonSearchClient photonSearchClient;

        public SearchController(ILogger<TilesController> logger, IOptions<ApplicationOptions> applicationOptions, PhotonSearchClient photonSearchClient)
        {
            this.logger = logger;
            this.applicationOptions = applicationOptions.Value;
            this.photonSearchClient = photonSearchClient;
        }

        [HttpGet]
        [Route("/search")]
        public async Task<ActionResult> Search([FromQuery(Name = "q")] string query, CancellationToken cancellationToken)
        {
            var result = await photonSearchClient.Search(query, cancellationToken: cancellationToken);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Results for '{query}': {result}");
            }

            return Ok(result);
        }
    }
}
