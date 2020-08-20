// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MapboxTileServer.Extensions
{
    public static class HttpFileExtensions
    {
        public static async Task<string> ReadAsStringAsync(this IFormFile file)
        {
            if(file == null)
            {
                return null;
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return await reader.ReadToEndAsync();
            }
}

    }
}
