using Autodesk.Revit.DB;

namespace ProfileGenerator.Core.Models.Defination
{
    public abstract class ShapeDefinition
    {
        public string ShapeName;
        public abstract CurveLoop Generate(XYZ center);
    }
}
