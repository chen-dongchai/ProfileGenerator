using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;

namespace ProfileGenerator.Core.Models.Pattern
{
    internal class CirclePattern : CircleDefinition
    {
        public CirclePattern(double radius, string unit) : base(radius, unit)
        {
        }
        public override Autodesk.Revit.DB.CurveLoop Generate(Autodesk.Revit.DB.XYZ center)
        {
            Autodesk.Revit.DB.Arc arc1 = Autodesk.Revit.DB.Arc.Create(center, RadiusFt, 0, 1 * Math.PI, Autodesk.Revit.DB.XYZ.BasisX, Autodesk.Revit.DB.XYZ.BasisY);
            Arc arc2 = Autodesk.Revit.DB.Arc.Create(center, RadiusFt, 1 * Math.PI, 2 * Math.PI, Autodesk.Revit.DB.XYZ.BasisX, Autodesk.Revit.DB.XYZ.BasisY);
            Autodesk.Revit.DB.CurveLoop curveLoop = new Autodesk.Revit.DB.CurveLoop();
            curveLoop.Append(arc1);
            curveLoop.Append(arc2);
            return curveLoop;
        }
    }
}
