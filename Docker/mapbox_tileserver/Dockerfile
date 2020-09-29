# Get the dotnet Build Environment:
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# COPY Project Files:
COPY *.sln .
COPY ./MapboxTileServer/*.csproj ./MapboxTileServer/
COPY ./MapboxTileServer.GeoJson/*.csproj ./MapboxTileServer.GeoJson/
COPY ./MapboxTileServer.GeoJson.Test/*.csproj ./MapboxTileServer.GeoJson.Test/

# And restore the NuGet Packages:
RUN dotnet restore

# COPY 
COPY MapboxTileServer/. ./MapboxTileServer/
COPY MapboxTileServer.GeoJson/. ./MapboxTileServer.GeoJson/
COPY MapboxTileServer.GeoJson.Test/. ./MapboxTileServer.GeoJson.Test/

WORKDIR /source/MapboxTileServer
RUN dotnet publish -c release -o /app --no-restore

# Build the final image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal

# Copy Artifacts from Build:
WORKDIR /app
COPY --from=build /app ./

# And kick off the Server:
ENTRYPOINT ["dotnet", "MapboxTileServer.dll"]