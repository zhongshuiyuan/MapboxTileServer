using MapboxTileServer.Clients.Http.Builder;
using MapboxTileServer.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MapboxTileServer.Clients
{
    public class PhotonSearchClient
    {
        private readonly HttpClient httpClient;
        private readonly ApplicationOptions applicationOptions;

        public PhotonSearchClient(IOptions<ApplicationOptions> applicationOptions)
            : this(applicationOptions, new HttpClient())
        {
        }

        public PhotonSearchClient(IOptions<ApplicationOptions> applicationOptions, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.applicationOptions = applicationOptions.Value;
        }

        public async Task<string> ReverseAsync(float lat, float lon, CancellationToken cancellationToken = default)
        {
            var url = $"{applicationOptions.Photon?.ApiUrl}/reverse";

            var httpRequestMessage = new HttpRequestMessageBuilder(url, HttpMethod.Get)
                .AddQueryString("lon", lon.ToString(CultureInfo.InvariantCulture))
                .AddQueryString("lat", lat.ToString(CultureInfo.InvariantCulture))
                .Build();

            var httpResponse = await httpClient
                .SendAsync(httpRequestMessage, cancellationToken)
                .ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var statusCode = httpResponse.StatusCode;
                var reason = httpResponse.ReasonPhrase;

                throw new Exception($"API Request failed with Status Code {statusCode} and Reason {reason}. For additional information, see the HttpResponseMessage in this Exception.");
            }

            return await httpResponse.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);
        }

        public async Task<string> Search(string q, string lang = default, int? limit = default, float? lat = default, float? lon = default, int? location_bias_scale = default, string bbox = default, string[] osm_tags = default, CancellationToken cancellationToken = default)
        {
            var url = $"{applicationOptions.Photon?.ApiUrl}";

            var httpRequestMessageBuilder = new HttpRequestMessageBuilder(url, HttpMethod.Get);

            httpRequestMessageBuilder.AddQueryString("q", q);

            if(!string.IsNullOrWhiteSpace(lang))
            {
                httpRequestMessageBuilder.AddQueryString("lang", lang);
            }

            if(limit.HasValue)
            {
                httpRequestMessageBuilder.AddQueryString("limit", limit.Value.ToString(CultureInfo.InvariantCulture));
            }

            if(lat.HasValue)
            {
                httpRequestMessageBuilder.AddQueryString("lat", lat.Value.ToString(CultureInfo.InvariantCulture));
            }

            if(lon.HasValue)
            {
                httpRequestMessageBuilder.AddQueryString("lon", lon.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (location_bias_scale.HasValue)
            {
                httpRequestMessageBuilder.AddQueryString("location_bias_scale", location_bias_scale.Value.ToString(CultureInfo.InvariantCulture));
            }

            if(!string.IsNullOrWhiteSpace(bbox))
            {
                httpRequestMessageBuilder.AddQueryString("bbox", bbox);
            }

            if(osm_tags != null)
            {
                foreach(var osm_tag in osm_tags)
                {
                    httpRequestMessageBuilder.AddQueryString("osm_tag", osm_tag);
                }
            }

            var httpRequestMessage = httpRequestMessageBuilder.Build();

            var httpResponse = await httpClient
                .SendAsync(httpRequestMessage)
                .ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var statusCode = httpResponse.StatusCode;
                var reason = httpResponse.ReasonPhrase;

                throw new Exception($"API Request failed with Status Code {statusCode} and Reason {reason}. For additional information, see the HttpResponseMessage in this Exception.");
            }

            return await httpResponse.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);
        }
    }
}
