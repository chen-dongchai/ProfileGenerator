using ProfileGenerator.Core.Models.Defination;

namespace ProfileGenerator.Core.Models.Arrangement
{
    internal class GridArrange : NormalArrangeDefinition
    {
        public GridArrange(double horizontalGap, double verticalGap, string unitname, int rows, int cols) : base(horizontalGap, verticalGap, unitname, rows, cols)
        {
            ArrangeTypeName = "网格";
        }
    }
}
