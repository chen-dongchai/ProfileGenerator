namespace ProfileGenerator.Core.Models.Defination
{
    internal abstract class RectangleDefinition : ShapeDefinition
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double CornerRadius { get; set; } = 0;
        public string Unit { get; set; }

        public double WidthFt { get; set; }
        public double HeightFt { get; set; }
        public double CornerRadiusFt { get; set; }

        public RectangleDefinition(double width, double height, double cornerradius, string unit)
        {
            Width = width;
            Height = height;

            CornerRadius = cornerradius;
            Unit = unit;

            WidthFt = Utils.Units.ToFeet(Width, Unit);
            HeightFt = Utils.Units.ToFeet(Height, Unit);
            CornerRadiusFt = Utils.Units.ToFeet(CornerRadius, Unit);

        }
    }
}
