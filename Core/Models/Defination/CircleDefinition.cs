namespace ProfileGenerator.Core.Models.Defination
{
    internal abstract class CircleDefinition : ShapeDefinition
    {
        public double Radius { get; set; }
        public string Unit { get; set; }
        public double RadiusFt { get; set; }
        public CircleDefinition(double radius, string unit)
        {
            Radius = radius;
            Unit = unit;
            RadiusFt = Utils.Units.ToFeet(Radius, Unit);
        }

    }
}
