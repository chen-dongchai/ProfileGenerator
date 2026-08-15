namespace ProfileGenerator.Core.Models.Defination
{
    internal abstract class StarDefinition : ShapeDefinition
    {
        public double InCircleRadius;
        public double OutCircleRadius;
        public int StarsCount;

        public double InCircleRadiusFt;
        public double OutCircleRadiusFt;
        public string StarUnit;
        public StarDefinition(double inCircleRadius, double outCircleRadius, int starsCount, string starUnit)
        {
            InCircleRadius = inCircleRadius;
            OutCircleRadius = outCircleRadius;
            StarsCount = starsCount;
            StarUnit = starUnit;
            InCircleRadiusFt = Utils.Units.ToFeet(inCircleRadius, StarUnit);
            OutCircleRadiusFt = Utils.Units.ToFeet(outCircleRadius, StarUnit);
        }
    }
}
