/*

using System;
using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.Configuration.Parsing;

namespace RealSolarSystem.KopernicusMods
{
    public class VertexHeightCubeMap : ModLoader<PQSMod_VertexHeightCubeMap>
    {
        // The map textures for the planet
        [ParserTarget("mapXn")]
        public MapSOParserLarge<MapSO> vertexHeightMapXn
        {
            get { return Mod.vertexHeightMapXn; }
            set { Mod.vertexHeightMapXn = value; }
        }
        [ParserTarget("mapXp")]
        public MapSOParserLarge<MapSO> vertexHeightMapXp
        {
            get { return Mod.vertexHeightMapXp; }
            set { Mod.vertexHeightMapXp = value; }
        }
        [ParserTarget("mapYn")]
        public MapSOParserLarge<MapSO> vertexHeightMapYn
        {
            get { return Mod.vertexHeightMapYn; }
            set { Mod.vertexHeightMapYn = value; }
        }
        [ParserTarget("mapYp")]
        public MapSOParserLarge<MapSO> vertexHeightMapYp
        {
            get { return Mod.vertexHeightMapYp; }
            set { Mod.vertexHeightMapYp = value; }
        }
        [ParserTarget("mapZn")]
        public MapSOParserLarge<MapSO> vertexHeightMapZn
        {
            get { return Mod.vertexHeightMapZn; }
            set { Mod.vertexHeightMapZn = value; }
        }
        [ParserTarget("mapZp")]
        public MapSOParserLarge<MapSO> vertexHeightMapZp
        {
            get { return Mod.vertexHeightMapZp; }
            set { Mod.vertexHeightMapZp = value; }
        }
        // Height map offset
        [ParserTarget("offset")]
        public NumericParser<Double> heightMapOffset
        {
            get { return Mod.heightMapOffset; }
            set { Mod.heightMapOffset = value; }
        }

        // Height map offset
        [ParserTarget("deformity")]
        public NumericParser<Double> heightMapDeformity
        {
            get { return Mod.heightMapDeformity; }
            set { Mod.heightMapDeformity = value; }
        }

        // Height map offset
        [ParserTarget("scaleDeformityByRadius")]
        public NumericParser<Boolean> scaleDeformityByRadius
        {
            get { return Mod.scaleDeformityByRadius; }
            set { Mod.scaleDeformityByRadius = value; }
        }
    }
}
*/