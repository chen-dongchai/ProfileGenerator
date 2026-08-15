using Autodesk.Revit.DB;

namespace ProfileGenerator.Core.Models.Defination
{
    public abstract class ShapeDefinition
    {
        public abstract CurveLoop Generate(XYZ center);
    }
}
