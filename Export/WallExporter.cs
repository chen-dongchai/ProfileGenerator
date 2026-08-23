using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileGenerator.Export
{
    public class WallExporter : IExternalEventHandler
    {
        // 外部传入的数据
        public CurveArrArray ContourData { get; set; }
        public ElementId WallId { get; set; }

        public void GetNeed(CurveArrArray curveArrArray, ElementId elementId)
        {
            ContourData = curveArrArray;
            WallId = elementId;
        }

        public void Execute(UIApplication application)
        {
            Document doc = application.ActiveUIDocument.Document;
            Element element = doc.GetElement(WallId);
            Wall wall = element as Wall;
            if (wall == null) return;

            // 提取内部环
            IList<CurveArray> innerLoops = new List<CurveArray>();
            for (int i = 1; i < ContourData.Size; i++)
            {
                innerLoops.Add(ContourData.get_Item(i));
            }
            if (innerLoops.Count == 0) return;

            // 获取墙定位线信息（仅支持直线墙）
            LocationCurve locCurve = wall.Location as LocationCurve;
            if (locCurve == null || !(locCurve.Curve is Line))
            {
                TaskDialog.Show("错误", "仅支持直线墙");
                return;
            }
            Line wallLine = locCurve.Curve as Line;
            XYZ wallDir = (wallLine.GetEndPoint(1) - wallLine.GetEndPoint(0)).Normalize();

            // 获取墙的几何中心
            BoundingBoxXYZ bbox = wall.get_BoundingBox(null);
            XYZ wallCenter = (bbox.Min + bbox.Max) / 2;

            // 构建变换（原点 = 墙中心）
            Transform transform = GetWallPlaneTransform(wallCenter, wallDir);

            // 变换所有内部环
            IList<CurveArray> transformedLoops = new List<CurveArray>();
            foreach (CurveArray loop in innerLoops)
            {
                CurveArray newLoop = new CurveArray();
                foreach (Curve curve in loop)
                {
                    Curve newCurve = curve.CreateTransformed(transform);
                    newLoop.Append(newCurve);
                }
                transformedLoops.Add(newLoop);
            }
            innerLoops = transformedLoops;

            // 获取或创建墙草图
            Sketch sketch = null;
            if (wall.CanHaveProfileSketch())
            {
                if (wall.SketchId != ElementId.InvalidElementId)
                    sketch = doc.GetElement(wall.SketchId) as Sketch;
                else
                {
                    using (Transaction trans = new Transaction(doc, "Create Wall Sketch"))
                    {
                        trans.Start();
                        sketch = wall.CreateProfileSketch();
                        trans.Commit();
                    }
                }
            }
            else
            {
                TaskDialog.Show("错误", "墙不支持轮廓草图");
                return;
            }

            if (sketch == null)
            {
                TaskDialog.Show("错误", "无法获取草图");
                return;
            }

            // 启动 SketchEditScope
            var sketchEditScope = new SketchEditScope(doc, "Add openings to wall profile");
            sketchEditScope.Start(sketch.Id);

            using (Transaction trans = new Transaction(doc, "Add opening loops"))
            {
                trans.Start();
                foreach (CurveArray loop in innerLoops)
                {
                    foreach (Curve curve in loop)
                    {
                        doc.Create.NewModelCurve(curve, sketch.SketchPlane);
                    }
                }
                trans.Commit();
            }

            sketchEditScope.Commit(new FailuresPreprocessor());
        }

        private Transform GetWallPlaneTransform(XYZ origin, XYZ wallDir)
        {
            wallDir = wallDir.Normalize();
            XYZ xAxis = wallDir;
            XYZ yAxis = XYZ.BasisZ;
            XYZ zAxis = xAxis.CrossProduct(yAxis).Normalize();

            Transform transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = xAxis;
            transform.BasisY = yAxis;
            transform.BasisZ = zAxis;
            return transform;
        }

        public string GetName()
        {
            return "WallExporter";
        }
    }
    public class FailuresPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            IList<FailureMessageAccessor> failList = new List<FailureMessageAccessor>();
            // Inside event handler, get all warnings
            failList = failuresAccessor.GetFailureMessages();
            foreach (FailureMessageAccessor failure in failList)
            {
                // check FailureDefinitionIds against ones that you want to dismiss, 
                FailureDefinitionId failID = failure.GetFailureDefinitionId();
                // prevent Revit from showing Unenclosed room warnings
                if (failID == BuiltInFailures.WallFailures.WallSettingsChanged)
                {
                    failuresAccessor.DeleteWarning(failure);
                }
            }

            return FailureProcessingResult.Continue;
        }
    }
}
