namespace ProfileGenerator.Core.Models.Defination
{
    internal abstract class DiamondDefinition : ShapeDefinition
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Unit { get; set; }
        public double WidthFt { get; set; }
        public double HeightFt { get; set; }
        public DiamondDefinition(double width, double height, string unit)
        {
            Width = width;
            Height = height;
            Unit = unit;
            WidthFt = Utils.Units.ToFeet(Width, Unit);
            HeightFt = Utils.Units.ToFeet(Height, Unit);
        }
    }
}
