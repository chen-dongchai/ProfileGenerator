using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;

namespace ProfileGenerator.Core.Models.Pattern
{
    internal class RectanglePattern : RectangleDefinition
    {
        public double RotatedAngle;
        public RectanglePattern(double width, double height, double cornerradius, string unit, double rotatedangle) : base(width, height, cornerradius, unit)
        {
            RotatedAngle = rotatedangle;
            ShapeName = "RectanglePattern";
        }

        public override CurveLoop Generate(XYZ center)
        {
            CurveLoop rectangleresult = new CurveLoop();
            if (CornerRadiusFt == 0)
            {
                double halfwidthft = WidthFt / 2;
                double halfheightft = HeightFt / 2;
                XYZ p1 = new XYZ(center.X - halfwidthft, center.Y + halfheightft, 0);
                XYZ p2 = new XYZ(center.X + halfwidthft, center.Y + halfheightft, 0);
                XYZ p3 = new XYZ(center.X + halfwidthft, center.Y - halfheightft, 0);
                XYZ p4 = new XYZ(center.X - halfwidthft, center.Y - halfheightft, 0);
                Line p1p2 = Line.CreateBound(p1, p2);
                Line p2p3 = Line.CreateBound(p2, p3);
                Line p3p4 = Line.CreateBound(p3, p4);
                Line p4p1 = Line.CreateBound(p4, p1);
                rectangleresult.Append(p1p2);
                rectangleresult.Append(p2p3);
                rectangleresult.Append(p3p4);
                rectangleresult.Append(p4p1);
            }
            else if (CornerRadiusFt > 0)
            {
                double halfWidthFt = WidthFt / 2;
                double halfHeightFt = HeightFt / 2;
                double radiusFt = CornerRadiusFt;

                XYZ a1 = new XYZ(center.X - halfWidthFt, center.Y + halfHeightFt - radiusFt, 0);
                XYZ a2 = new XYZ(center.X - halfWidthFt + radiusFt, center.Y + halfHeightFt, 0);
                XYZ b1 = new XYZ(center.X + halfWidthFt - radiusFt, center.Y + halfHeightFt, 0);
                XYZ b2 = new XYZ(center.X + halfWidthFt, center.Y + halfHeightFt - radiusFt, 0);
                XYZ c1 = new XYZ(center.X + halfWidthFt, center.Y - halfHeightFt + radiusFt, 0);
                XYZ c2 = new XYZ(center.X + halfWidthFt - radiusFt, center.Y - halfHeightFt, 0);
                XYZ d1 = new XYZ(center.X - halfWidthFt + radiusFt, center.Y - halfHeightFt, 0);
                XYZ d2 = new XYZ(center.X - halfWidthFt, center.Y - halfHeightFt + radiusFt, 0);

                XYZ acenter = new XYZ(center.X - halfWidthFt + radiusFt, center.Y + halfHeightFt - radiusFt, 0);
                Arc a1a2 = Arc.Create(acenter, radiusFt, 0.5 * Math.PI, Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line a2b1 = Line.CreateBound(a2, b1);
                XYZ bcenter = new XYZ(center.X + halfWidthFt - radiusFt, center.Y + halfHeightFt - radiusFt, 0);
                Arc b1b2 = Arc.Create(bcenter, radiusFt, 0, 0.5 * Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line b2c1 = Line.CreateBound(b2, c1);
                XYZ ccenter = new XYZ(center.X + halfWidthFt - radiusFt, center.Y - halfHeightFt + radiusFt, 0);
                Arc c1c2 = Arc.Create(ccenter, radiusFt, 1.5 * Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line c2d1 = Line.CreateBound(c2, d1);
                XYZ dcenter = new XYZ(center.X - halfWidthFt + radiusFt, center.Y - halfHeightFt + radiusFt, 0);
                Arc d1d2 = Arc.Create(dcenter, radiusFt, 1 * Math.PI, 1.5 * Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line d2a1 = Line.CreateBound(d2, a1);

                rectangleresult.Append(a1a2);
                rectangleresult.Append(a2b1);
                rectangleresult.Append(b1b2);
                rectangleresult.Append(b2c1);
                rectangleresult.Append(c1c2);
                rectangleresult.Append(c2d1);
                rectangleresult.Append(d1d2);
                rectangleresult.Append(d2a1);
            }
            if (RotatedAngle != 0)
            {

                Transform rotatedtrans = Transform.CreateRotationAtPoint(XYZ.BasisZ, RotatedAngle * (Math.PI / 180), center);
                rectangleresult.Transform(rotatedtrans);

            }
            return rectangleresult;
        }
    }
}
