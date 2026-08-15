using Autodesk.Revit.DB;
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

}
