# A .NET MapboxTileServer #

Every project grows to a point it needs to support maps. With the current COVID-19 pandemic you 
are seeing maps basically everywhere. So how could we host maps ourselves without hitting Google 
Maps servers?

Some time ago I played with [baremaps] to generate Vector tiles and it gave me *a lot of ideas* for 
OpenStreetMap data and mapping in general. It is a great project and I wish [Bertil] success for his 
project.

Playing with [baremaps] has also taught me how complicated it is to design styles and how to preprocess 
OSM data for your needs. At some point I gave up, but instead of throwing away the code I adapted 
it to use the pre-built [OpenMapTiles]:

* [https://github.com/bytefish/MapboxTileServer](https://github.com/bytefish/MapboxTileServer)

[Bertil]: https://www.bertil.ch/
[baremaps]: https://github.com/baremaps/baremaps

## Description ##

This project implements a Tile Server in .NET for serving pre-built [OpenMapTiles]. It also integrates [Photon] to provide 
Geocoding features. The ASP.NET Core Server also includes a small Frontend to display a map and geocode results, so you get 
an idea *where to start*.

<div style="display:flex; align-items:center; justify-content:center;">
    <a href="https://bytefish.de/static/images/blog/mapboxtileserver_geocoding/MapboxTileServerGeocodingExample.png">
        <img src="https://bytefish.de/static/images/blog/mapboxtileserver_geocoding/MapboxTileServerGeocodingExample.png">
    </a>
</div>

The following articles describe the implementation in detail:

* [https://bytefish.de/blog/mapboxtileserver_csharp/](https://bytefish.de/blog/mapboxtileserver_csharp/)
* [https://bytefish.de/blog/mapboxtileserver_geocoding/](https://bytefish.de/blog/mapboxtileserver_geocoding/)

[Photon]: https://photon.komoot.de/

## Getting the Data ##

I want to be able to host maps without using external services, but host everything locally. This means 
we'll need to get the necessary data for vector tiles, fonts and sprites first.

### Vector Tilesets ###

There is a great project called [OpenMapTiles], which has a huge range of prebuilt tilesets 
and is used by large enterprises like Siemens, Bosch or IBM. And on the upside, most of the 
available map styles expect data in the OpenMapTiles schema:

* [https://openmaptiles.com/](https://openmaptiles.com/)

I want to show a map of Germany, so I am going to download the data for Germany here:

* [https://openmaptiles.com/downloads/dataset/osm/europe/germany/#4.8/51.376/10.458](https://openmaptiles.com/downloads/dataset/osm/europe/germany/#4.8/51.376/10.458)

And that's it for the tileset.

### Fonts ###

We'll need to display all kinds of names for roads, lakes or points of interests. In the Mapbox styles all 
fonts are given in the ``text-font`` attribute. The Mapbox GL viewer expects the fonts to be served as PBF 
files. 

I have absolutely no idea how to this, but the required fonts have already been pre-built by the 
[OpenMapTiles] team at:

* [https://github.com/openmaptiles/fonts/releases/download/v1.0/v1.0.zip](https://github.com/openmaptiles/fonts/releases/download/v1.0/v1.0.zip)

I only need a small subset, so I have added the PBF fonts to the GitHub repository.

### Sprites ###

Sprites are needed for displaying icons and other kinds of images on the map. We are going to use the OSM 
Liberty style from the [maputnik] editor. You can download these Sprites from the [maputnik] repositories 
at:

* [https://github.com/maputnik/osm-liberty](https://github.com/maputnik/osm-liberty)

### Natural Earth Tiles ###

Most of the examples you find for Mapbox Vector Tiles are using a layer for the Natural Earth dataset:

> Natural Earth is a public domain map dataset available at 1:10m, 1:50m, and 1:110 million scales. Featuring tightly 
> integrated vector and raster data, with Natural Earth you can make a variety of visually pleasing, well-crafted maps 
> with cartography or GIS software.

So how can we add it to our project? There are prebuilt Tilesets kindly provided [Lukas Martinelli](https://lukasmartinelli.ch), 
and the project page states:

> Natural Earth is one of the best public domain data sets and now you can instantly use it for 
> your mapping projects by using the prerendered vector or raster tiles.

The Tiles can be downloaded from:

* [http://naturalearthtiles.lukasmartinelli.ch/](http://naturalearthtiles.lukasmartinelli.ch/)

In the example I am going to use "Natural Earth II with Shaded Relief" Raster tiles.


## Photon ##

According to the documentation Photon is ...

> [...] an open source geocoder built for [OpenStreetMap] data. It is based on [elasticsearch] - an efficient, powerful and highly 
> scalable search platform.
>
> Photon was started by [komoot] and provides search-as-you-type and multilingual support. It's used in production with thousands 
> of requests per minute at [www.komoot.de]. Find our public API and demo on [photon.komoot.de].

You can find the GitHub repositories at:

* [https://github.com/komoot/photon]([https://github.com/komoot/photon)

### Getting the Search Index Data for Photon ###

Photon requires an elasticsearch index. This index can be built by using the Photon CLI to import the data 
from [Nomatim], but there is a much simpler way.

The kind people of [GraphHopper] and user [@ionvia] provide a download of the Photon elasticsearch index. The index 
is built each week, so it contains recent additions to the [OpenStreetMap] data. 

The archive ``photon-db-latest.tar.bz2`` always contains the latest build and can be downloaded from:

* [http://download1.graphhopper.com/public/](http://download1.graphhopper.com/public/)

If you are using a Unix system you can also run the following command to download and extract the latest search index:

```bash
wget -O - http://download1.graphhopper.com/public/photon-db-latest.tar.bz2 | bzip2 -cd | tar x
```

### Getting Photon Up and Running ###

When you have downloaded an extracted the data, you start with getting the latest JAR file from the Photon 
releases. As of writing this is 0.3.3:

* [https://github.com/komoot/photon/releases/tag/0.3.3](https://github.com/komoot/photon/releases/tag/0.3.3)

What I do now is writing a simple Batch Script to set the Java executable, the JAR file and the Data Directory, which 
contains the Search index you have just downloaded and extracted:

```batch
@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set JAVA_EXE="G:\Applications\Oracle\jdk-14.0.1\bin\java.exe"
set PHOTON_JAR="G:\Applications\Photon\photon-0.3.3.jar"
set DATA_DIR="G:\Data\Photon\photon-db-de-200809"

%JAVA_EXE% -jar %PHOTON_JAR% -data-dir %DATA_DIR% -listen-ip 0.0.0.0 -listen-port 2322 -cors-any

pause
```

Now if you execute the Script, you should give elasticsearch some seconds to load the index and... that's it!

It was *unbelievably easy* to get started with Photon. The developers have done a great job!

[maputnik]: http://maputnik.github.io/
[OpenMapTiles]: https://openmaptiles.org/
[Nomatim]: https://nominatim.org/
[komoot]: http://www.komoot.de/
[photon.komoot.de]: http://photon.komoot.de/
[www.komoot.de]: http://www.komoot.de/
[OpenStreetMap]: http://www.osm.org/
[elasticsearch]: http://elasticsearch.org/
[GraphHopper]: https://www.graphhopper.com/
[@ionvia]: https://github.com/lonvia