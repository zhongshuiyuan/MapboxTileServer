// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MapboxTileServer.Extensions;
using MapboxTileServer.Options;
using MapboxTileServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MapboxTileServer.Controllers
{
    [ApiController]
    public class KmlController : ControllerBase
    {
        private readonly ILogger<TilesController> logger;
        private readonly ApplicationOptions applicationOptions;
        private readonly IWebHostEnvironment environment;
        private readonly IXmlValidationService xmlValidationService;

        public KmlController(ILogger<TilesController> logger, IWebHostEnvironment environment, IOptions<ApplicationOptions> applicationOptions, IXmlValidationService xmlValidationService)
        {
            this.logger = logger;
            this.environment = environment;
            this.applicationOptions = applicationOptions.Value;
            this.xmlValidationService = xmlValidationService;
        }

        [HttpPost]
        [Route("/kml/toGeoJson")]
        public async Task<IActionResult> KmlToGeoJson(IFormFile file, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Uploaded a KML File ...");

            if(file == null)
            {
                return BadRequest();
            }

            var xml = await file.ReadAsStringAsync(); ;


            var kmlSchemas = GetKmlSchemas();

            var kmlIsValid = await xmlValidationService.ValidateAsync(xml, kmlSchemas, cancellationToken);

            if(!kmlIsValid)
            {
                logger.LogError("Invalid KML has been sent to Server");

                return BadRequest();
            }

            var json = GeoJson.GeoJsonConverter.FromKml(xml);

            return Content(json, "application/json");
        }

        private XmlSchemaSet GetKmlSchemas()
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            
            schemas.Add("http://www.opengis.net/kml/2.2", GetLocalSchemaPath("ogckml22.xsd"));
            schemas.Add("http://www.google.com/kml/ext/2.2", "kml22gx.xsd");

            return schemas;
        }

        private string GetLocalSchemaPath(string file)
        {
            var baseDirectory = Path.Combine(environment.WebRootPath, applicationOptions.SchemaDirectory);

            return Path.Combine(baseDirectory, file);
        }
    }
}
