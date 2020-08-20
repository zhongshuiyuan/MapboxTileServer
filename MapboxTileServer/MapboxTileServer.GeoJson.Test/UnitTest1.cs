using NUnit.Framework;
using System;
using System.IO;

namespace MapboxTileServer.GeoJson.Test
{
    public class Tests
    {



        [SetUp]
        public void Setup()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(basePath, "Resources", "Kml", "addresses.kml");

            var xml = File.ReadAllText(filePath);

            var json = GeoJsonConverter.FromKml(xml);

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}