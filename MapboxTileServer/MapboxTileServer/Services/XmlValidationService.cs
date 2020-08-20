// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MapboxTileServer.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MapboxTileServer.Services
{
    public interface IXmlValidationService
    {
        Task<bool> ValidateAsync(string xml, XmlSchemaSet schemas, CancellationToken cancellationToken);
    }

    public class XmlValidationService : IXmlValidationService
    {
        private readonly ApplicationOptions applicationOptions;
        private readonly ILogger<XmlValidationService> logger;

        public XmlValidationService(ILogger<XmlValidationService> logger, IOptions<ApplicationOptions> applicationOptions)
        {
            this.logger = logger;
            this.applicationOptions = applicationOptions.Value;
        }

        public async Task<bool> ValidateAsync(string xml, XmlSchemaSet schemas, CancellationToken cancellationToken)
        {
            try
            {
                return await InternalValidateAsync(xml, schemas, cancellationToken).ConfigureAwait(false);
            } 
            catch(Exception e)
            {
                logger.LogError($"Failed to validate XML against Schemas", e);

                return false;
            }
        }

        private async Task<bool> InternalValidateAsync(string xml, XmlSchemaSet schemas, CancellationToken cancellationToken)
        {
            using (var stringReader = new StringReader(xml))
            {
                XDocument doc = await XDocument
                    .LoadAsync(stringReader, LoadOptions.None, cancellationToken)
                    .ConfigureAwait(false);

                bool isValidXml = true;

                doc.Validate(schemas, (sender, args) =>
                {
                    if (args.Severity == XmlSeverityType.Error)
                    {
                        isValidXml = false;
                    }
                });


                return isValidXml;
            }
        }

    }

}
}
