using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using ProfileGenerator.Core.Models.Outline;
using ProfileGenerator.Core.Models.Pattern;
using System;
using System.Collections.Generic;

namespace ProfileGenerator.Utils
{
    internal static class Units
    {
        public static double ToFeet(double usernum, string userunit)
        {
            double feetnum = 0;
            switch (userunit)
            {
                case "mm":
                    {
                        feetnum = UnitUtils.ConvertToInternalUnits(usernum, UnitTypeId.Millimeters);
                        break;
                    }
                case "cm":
                    {
                        feetnum = UnitUtils.ConvertToInternalUnits(usernum, UnitTypeId.Centimeters);
                        break;
                    }
                case "m":
                    {
                        feetnum = UnitUtils.ConvertToInternalUnits(usernum, UnitTypeId.Meters);
                        break;
                    }
                case "ft":
                    {
                        feetnum = UnitUtils.ConvertToInternalUnits(usernum, UnitTypeId.Feet);
                        break;
                    }
                case "in":
                    {
                        feetnum = UnitUtils.ConvertToInternalUnits(usernum, UnitTypeId.Inches);
                        break;
                    }
            }
            return feetnum;
        }
    }
    internal static class BoxPara
    {
        public static (double xmaxft, double ymaxft) GetBox(CurveLoop loop)
        {
            List<XYZ> points = new List<XYZ>();
            foreach (Curve curve in loop)
            {
                points.AddRange(curve.Tessellate());
            }
            if (points.Count == 0) return (0, 0);

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;
            foreach (XYZ pt in points)
            {
                if (pt.X < minX) minX = pt.X;
                if (pt.X > maxX) maxX = pt.X;
                if (pt.Y < minY) minY = pt.Y;
                if (pt.Y > maxY) maxY = pt.Y;
            }
            return (maxX - minX, maxY - minY);
        }
    }
    internal static class CurveLoopCheckIsIn
    {
        public static bool CheckIsIn(CurveLoop incurves, CurveLoop outcurves)
        {
            if (HasAnyIntersection(incurves, outcurves)) return false;
            return IsTotallIn(incurves, outcurves);
        }
        private static bool HasAnyIntersection(CurveLoop incurves, CurveLoop outcurves)
        {
            foreach (Curve curveA in incurves)
            {
                foreach (Curve curveB in outcurves)
                {

                    SetComparisonResult result = curveA.Intersect(curveB);
                    if (result == SetComparisonResult.Equal || result == SetComparisonResult.Overlap
                        || result == SetComparisonResult.Subset || result == SetComparisonResult.Superset)
                    {
                        return true;
                    }

                }
            }
            return false;
        }
        private static bool IsTotallIn(CurveLoop incurves, CurveLoop outcurves)
        {
            // 1. 取内部图案的一个点（取第一条曲线的第一个采样点）
            XYZ anyonespot = null;
            foreach (Curve curve1 in incurves)
            {
                IList<XYZ> pts = curve1.Tessellate();
                if (pts.Count > 0)
                {
                    anyonespot = pts[0];
                    break;
                }
            }
            if (anyonespot == null) return false;

            // 2. 把外部轮廓离散成多边形点列表
            List<XYZ> polygon = new List<XYZ>();
            foreach (Curve curve in outcurves)
            {
                IList<XYZ> pts = curve.Tessellate();
                // Tessellate 返回的点包含首尾，去掉最后一个重复点，避免多边形线段重复
                for (int i = 0; i < pts.Count - 1; i++)
                {
                    polygon.Add(pts[i]);
                }
            }
            if (polygon.Count < 3) return false; // 不是有效多边形

            // 3. 射线法判断点是否在多边形内
            int intersections = 0;
            int n = polygon.Count;
            for (int i = 0; i < n; i++)
            {
                XYZ p1 = polygon[i];
                XYZ p2 = polygon[(i + 1) % n];

                // 忽略水平边
                if (Math.Abs(p1.Y - p2.Y) < 1e-9)
                    continue;

                bool isP1Above = p1.Y > anyonespot.Y;
                bool isP2Above = p2.Y > anyonespot.Y;

                if (isP1Above != isP2Above)
                {
                    double xIntersect = p1.X + (anyonespot.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);
                    if (xIntersect > anyonespot.X)
                        intersections++;
                }
            }

            return (intersections % 2) == 1;
        }
    }
    internal static class LoopToArray
    {
        public static CurveArray ConvertToCurveArray(CurveLoop loop)
        {

            CurveArray curveArray = new CurveArray();
            foreach (Curve curve in loop)
            {
                curveArray.Append(curve);
            }

            return curveArray;
        }
    }
    public static class OffsetWay
    {
        public static CurveLoop OutlineOffset(ShapeDefinition shapeDefinition,double gapFt)
        {
            CurveLoop finaloutline = new CurveLoop();
            XYZ center = new XYZ(0,0,0);
            switch (shapeDefinition.ShapeName)
            {
                case "CircleOutline":
                    CircleOutline circleOutline = shapeDefinition as CircleOutline;
                    Arc arc1 = Arc.Create(center,circleOutline.RadiusFt-gapFt, 0, 1 * Math.PI, XYZ.BasisX, XYZ.BasisY);
                    Arc arc2 = Arc.Create(center,circleOutline.RadiusFt-gapFt, 1 * Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY);
                    finaloutline.Append(arc1);
                    finaloutline.Append(arc2);
                    break;
                case "RectangleOutline":
                    RectangleOutline rectangleOutline = shapeDefinition as RectangleOutline;
                    List<Curve> curves = new List<Curve>();
                    if (rectangleOutline.CornerRadiusFt == 0)
                    {
                        double halfWidthFt = (rectangleOutline.WidthFt-gapFt) / 2;
                        double halfHeightFt = (rectangleOutline.HeightFt-gapFt) / 2;
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
                        
                    }

                    else if (rectangleOutline.CornerRadiusFt > 0)
                    {
                        double halfWidthFt = (rectangleOutline.WidthFt - gapFt) / 2;
                        double halfHeightFt = (rectangleOutline.HeightFt - gapFt) / 2;
                        double radiusFt = rectangleOutline.CornerRadiusFt;

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
                        
                    }
                    finaloutline = CurveLoop.Create(curves);
                    finaloutline.Flip();
                    break;
            }

            return finaloutline;
        }
    }

}
