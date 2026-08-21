using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;

namespace ProfileGenerator.Core.Models.Pattern
{
    internal class DiamondPattern : DiamondDefinition
    {
        public double Rotation;
        public DiamondPattern(double width, double height, string unit, double rotation) : base(width, height, unit)
        {
            Rotation = rotation;
            ShapeName = "DiamondPattern";
        }
        public override CurveLoop Generate(XYZ center)
        {
            double halfwidthft = WidthFt / 2.0;
            double halfheightft = HeightFt / 2.0;
            XYZ p1 = new XYZ(center.X - halfwidthft, center.Y, 0);
            XYZ p2 = new XYZ(center.X, center.Y + halfheightft, 0);
            XYZ p3 = new XYZ(center.X + halfwidthft, center.Y, 0);
            XYZ p4 = new XYZ(center.X, center.Y - halfheightft, 0);
            Line p1p2 = Line.CreateBound(p1, p2);
            Line p2p3 = Line.CreateBound(p2, p3);
            Line p3p4 = Line.CreateBound(p3, p4);
            Line p4p1 = Line.CreateBound(p4, p1);
            CurveLoop diamondshape = new CurveLoop();
            diamondshape.Append(p1p2);
            diamondshape.Append(p2p3);
            diamondshape.Append(p3p4);
            diamondshape.Append(p4p1);

            if (Rotation != 0)
            {
                Transform rotatedtrans = Transform.CreateRotationAtPoint(XYZ.BasisZ, Rotation * (Math.PI / 180), center);
                diamondshape.Transform(rotatedtrans);
            }
            return diamondshape;
        }
    }
}
