using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;
using System.Collections.Generic;

namespace ProfileGenerator.Core.Models.Pattern
{
    internal class StarPattern : StarDefinition
    {
        public double Rotation;       //矩形的方法这里有边界框参数，圆形没有，矩形有旋转，旋转后边界框变化，   那么star也有旋转，但是旋转后大概不会变化  ---- 矩形的边界框参数并未使用，多余代码
        public StarPattern(double inCircleRadius, double outCircleRadius, int starsCount, string starunit, double rotation) : base(inCircleRadius, outCircleRadius, starsCount, starunit)
        {
            Rotation = rotation;
        }
        public override CurveLoop Generate(XYZ center)
        {
            var loop = new CurveLoop();
            int pointCount = StarsCount * 2;
            double angleStep = Math.PI / StarsCount; // 2π / (2N)
            double startAngle = Math.PI / 2;

            var pts = new List<XYZ>();
            for (int i = 0; i < pointCount; i++)
            {
                double radius = (i % 2 == 0) ? OutCircleRadiusFt : InCircleRadiusFt;
                double angle = startAngle + i * angleStep;
                pts.Add(new XYZ(
                    center.X + radius * Math.Cos(angle),
                    center.Y + radius * Math.Sin(angle),
                    0));
            }

            double minLength = 1.0 / 384.0;
            for (int i = 0; i < pointCount; i++)
            {
                XYZ a = pts[i];
                XYZ b = pts[(i + 1) % pointCount];
                if (a.DistanceTo(b) < minLength)
                    throw new InvalidOperationException("线段过短");
                loop.Append(Line.CreateBound(a, b));
            }
            if (Rotation != 0)
            {
                Transform rotatedtrans = Transform.CreateRotationAtPoint(XYZ.BasisZ, Rotation * (Math.PI / 180), center);
                loop.Transform(rotatedtrans);
            }
            return loop;
        }
    }
}
