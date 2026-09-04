using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileGenerator.Core.Models.Outline
{
    internal class WallOutline : ShapeDefinition
    {
        public double lengthFt;
        public double heightFt;
        public ElementId elementId;

        public WallOutline(double length, double height,ElementId elementId)
        {
            this.lengthFt = length;
            this.heightFt = height;
            this.elementId = elementId;
            ShapeName = "WallOutline";
        }
        public override CurveLoop Generate(XYZ xYZ)
        {
            CurveLoop loop = new CurveLoop();
            double halfLengthFt = lengthFt / 2;
            double halfHeightFt = heightFt / 2;
            XYZ p1 = new XYZ(-halfLengthFt, halfHeightFt, 0);
            XYZ p2 = new XYZ(halfLengthFt, halfHeightFt, 0);
            XYZ p3 = new XYZ(halfLengthFt, -halfHeightFt, 0);
            XYZ p4 = new XYZ(-halfLengthFt, -halfHeightFt, 0);
            Line p1p2 = Line.CreateBound(p1, p2);
            Line p2p3 = Line.CreateBound(p2, p3);
            Line p3p4 = Line.CreateBound(p3, p4);
            Line p4p1 = Line.CreateBound(p4, p1);
            loop.Append(p1p2);
            loop.Append(p2p3);
            loop.Append(p3p4);
            loop.Append(p4p1);
            loop.Flip();
            return loop;
        }
    }
}
