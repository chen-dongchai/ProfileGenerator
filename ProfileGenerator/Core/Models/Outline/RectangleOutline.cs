using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;
using System.Collections.Generic;

namespace ProfileGenerator.Core.Models.Outline
{
    internal class RectangleOutline : RectangleDefinition
    {

        public RectangleOutline(double width, double height, double cornerradius, string unit) : base(width, height, cornerradius, unit)
        {

        }
        public override CurveLoop Generate(XYZ center)
        {
            CurveLoop rectangleresult = new CurveLoop();
            List<Curve> curves = new List<Curve>();
            if (CornerRadiusFt == 0)
            {
                double halfWidthFt = WidthFt / 2;
                double halfHeightFt = HeightFt / 2;
                XYZ p1 = new XYZ(-halfWidthFt, halfHeightFt, 0);
                XYZ p2 = new XYZ(halfWidthFt, halfHeightFt, 0);
                XYZ p3 = new XYZ(halfWidthFt, -halfHeightFt, 0);
                XYZ p4 = new XYZ(-halfWidthFt, -halfHeightFt, 0);
                Line p1p2 = Line.CreateBound(p1, p2);
                Line p2p3 = Line.CreateBound(p2, p3);
                Line p3p4 = Line.CreateBound(p3, p4);
                Line p4p1 = Line.CreateBound(p4, p1);

                curves.Add(p1p2);
                curves.Add(p2p3);
                curves.Add(p3p4);
                curves.Add(p4p1);
                rectangleresult = CurveLoop.Create(curves);
            }

            else if (CornerRadiusFt > 0)
            {
                double halfWidthFt = WidthFt / 2;
                double halfHeightFt = HeightFt / 2;
                double radiusFt = CornerRadiusFt;

                XYZ a1 = new XYZ(-halfWidthFt, halfHeightFt - radiusFt, 0);
                XYZ a2 = new XYZ(-halfWidthFt + radiusFt, halfHeightFt, 0);
                XYZ b1 = new XYZ(halfWidthFt - radiusFt, halfHeightFt, 0);
                XYZ b2 = new XYZ(halfWidthFt, halfHeightFt - radiusFt, 0);
                XYZ c1 = new XYZ(halfWidthFt, -halfHeightFt + radiusFt, 0);
                XYZ c2 = new XYZ(halfWidthFt - radiusFt, -halfHeightFt, 0);
                XYZ d1 = new XYZ(-halfWidthFt + radiusFt, -halfHeightFt, 0);
                XYZ d2 = new XYZ(-halfWidthFt, -halfHeightFt + radiusFt, 0);

                XYZ acenter = new XYZ(-halfWidthFt + radiusFt, halfHeightFt - radiusFt, 0);
                Arc a1a2 = Arc.Create(acenter, radiusFt, 0.5 * Math.PI, Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line a2b1 = Line.CreateBound(a2, b1);
                XYZ bcenter = new XYZ(halfWidthFt - radiusFt, halfHeightFt - radiusFt, 0);
                Arc b1b2 = Arc.Create(bcenter, radiusFt, 0, 0.5 * Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line b2c1 = Line.CreateBound(b2, c1);
                XYZ ccenter = new XYZ(halfWidthFt - radiusFt, -halfHeightFt + radiusFt, 0);
                Arc c1c2 = Arc.Create(ccenter, radiusFt, 1.5 * Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line c2d1 = Line.CreateBound(c2, d1);
                XYZ dcenter = new XYZ(-halfWidthFt + radiusFt, -halfHeightFt + radiusFt, 0);
                Arc d1d2 = Arc.Create(dcenter, radiusFt, 1 * Math.PI, 1.5 * Math.PI, XYZ.BasisX, XYZ.BasisY).CreateReversed() as Arc;
                Line d2a1 = Line.CreateBound(d2, a1);

                curves.Add(a1a2);
                curves.Add(a2b1);
                curves.Add(b1b2);
                curves.Add(b2c1);
                curves.Add(c1c2);
                curves.Add(c2d1);
                curves.Add(d1d2);
                curves.Add(d2a1);
                rectangleresult = CurveLoop.Create(curves);
            }

            return rectangleresult;
        }
    }
}
