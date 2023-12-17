/* 
 * This code is adapted from KopernicusExpansion-Continued
 * Available from https://github.com/StollD/KopernicusExpansion-Continued
 */

using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.Configuration.ModLoader;

namespace RealSolarSystem
{
    public class VertexDefineCoastSmooth : ModLoader<PQSMod_VertexDefineCoastSmooth>
    {
        // Height map offset
        [ParserTarget("minOffset")]
        public NumericParser<double> minHeightOffset
        {
            get { return Mod.minHeightOffset; }
            set { Mod.minHeightOffset = value; }
        }

        // Height map offset
        [ParserTarget("maxOffset")]
        public NumericParser<double> maxHeightOffset
        {
            get { return Mod.maxHeightOffset; }
            set { Mod.maxHeightOffset = value; }
        }

        // Height map offset
        [ParserTarget("slopeScale")]
        public NumericParser<double> slopeScale
        {
            get { return Mod.slopeScale; }
            set { Mod.slopeScale = value; }
        }
    }
}