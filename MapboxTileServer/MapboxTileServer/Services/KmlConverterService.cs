using MapboxTileServer.GeoJson;
using Microsoft.Extensions.Logging;
using System;

namespace MapboxTileServer.Services
{
    public interface IKmlConverterService
    {
        bool ToGeoJson(string xml, out string json);
    }

    public class KmlConverterService : IKmlConverterService
    {
        private readonly ILogger<KmlConverterService> logger;

        public KmlConverterService(ILogger<KmlConverterService> logger)
        {
            this.logger = logger;
        }

        public bool ToGeoJson(string xml, out string json)
        {
            json = null;

            try
            {
                json = GeoJsonConverter.FromKml(xml);

                return true;
            } 
            catch(Exception e)
            {
                logger.LogError(e, "Failed to convert Kml to GeoJson");

                return false;
            }
        }
    }
}
