using ProfileGenerator.Core.Models.Defination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileGenerator.Core.Models.Arrangement
{
    internal class VoronoiArrange : ArrangeDefinition
    {
        public int targetCount;
        public double gap;
        public double gapFt;
        public int seed;
        public string userunit;
        public VoronoiArrange(int targetCount, double gap, int seed,string userunit)
        {
            ArrangeTypeName = "Voronoi";
            this.targetCount = targetCount;
            this.gap = gap;
            this.gapFt = Utils.Units.ToFeet(gap, userunit);
            this.seed = seed;
        }
    }
}
