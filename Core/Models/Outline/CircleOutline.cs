using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;

namespace ProfileGenerator.Core.Models.Outline
{
    internal class CircleOutline : CircleDefinition
    {
        public CircleOutline(double radius, string unit) : base(radius, unit)
        {
            ShapeName = "CircleOutline";
        }
        public override Autodesk.Revit.DB.CurveLoop Generate(Autodesk.Revit.DB.XYZ center)
        {
            Arc arc1 = Arc.Create(center, RadiusFt, 0, 1 * Math.PI, XYZ.BasisX, XYZ.BasisY);
            Arc arc2 = Arc.Create(center, RadiusFt, 1 * Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY);
            CurveLoop curveLoop = new CurveLoop();
            curveLoop.Append(arc1);
            curveLoop.Append(arc2);
            return curveLoop;
        }
    }
}
