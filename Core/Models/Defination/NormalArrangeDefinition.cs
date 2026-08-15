using ProfileGenerator.Utils;

namespace ProfileGenerator.Core.Models.Defination
{
    public class NormalArrangeDefinition : ArrangeDefinition
    {
        public double HorizontalGap;
        public double VerticalGap;
        public double HorizontalGapft;
        public double VerticalGapft;
        public int Rows;
        public int Cols;
        public string Unitname;
        public NormalArrangeDefinition(double horizontalGap, double verticalGap, string unitname, int rows, int cols)
        {
            HorizontalGap = horizontalGap;
            VerticalGap = verticalGap;
            Unitname = unitname;
            Rows = rows;
            Cols = cols;
            HorizontalGapft = Units.ToFeet(HorizontalGap, unitname);
            VerticalGapft = Units.ToFeet(VerticalGap, unitname);
        }
    }
}
