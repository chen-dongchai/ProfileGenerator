using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;

namespace ProfileGenerator.Export
{
    public class RFA3DExporter : IExternalEventHandler
    {
        public CurveArrArray curveArrArray;
        public string exportPath;
        public string exportName;
        public double height;
        public string heightunit;
        public void Execute(UIApplication application)
        {
            Application app = application.Application;
            string chineseTemplate = Path.Combine(app.FamilyTemplatePath, "公制常规模型.rft");
            string englishTemplate = Path.Combine(app.FamilyTemplatePath, "Metric Generic Model.rft");
            string templatePath = null;
            if (File.Exists(chineseTemplate))
            {
                templatePath = chineseTemplate;
            }
            else if (File.Exists(englishTemplate))
            {
                templatePath = englishTemplate;
            }
            Document familyDocument = app.NewFamilyDocument(templatePath);
            using (Transaction trans = new Transaction(familyDocument, "3dDWG"))
            {
                trans.Start();
                Plane plane = Plane.CreateByThreePoints(new XYZ(0, 0, 0), new XYZ(1, 0, 0), new XYZ(0, 1, 0));
                SketchPlane sketchplane = SketchPlane.Create(familyDocument, plane);
                double Thickness = 0.0;
                switch (heightunit)
                {
                    case "mm":
                        Thickness = UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Millimeters);
                        break;
                    case "cm":
                        Thickness = UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Centimeters);
                        break;
                    case "m":
                        Thickness = UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Meters);
                        break;
                    case "ft":
                        Thickness = UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Feet);
                        break;
                    case "in":
                        Thickness = UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Inches);
                        break;
                    default:
                        Thickness = UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Feet);
                        break;
                }


                // 创建拉伸体。假定 curveArr 已经在同一平面上且为闭合轮廓。
                familyDocument.FamilyCreate.NewExtrusion(true, curveArrArray, sketchplane, Thickness);
                trans.Commit();

            }
            SaveAsOptions options = new SaveAsOptions();
            options.OverwriteExistingFile = true;
            familyDocument.SaveAs(Path.Combine(exportPath, exportName + ".rfa"), options);

        }
        public string GetName()
        {
            return "DWG3DExporter";
        }
        public void GetCurveArr(CurveArrArray curvearrArray, string exportpath, string exportName, double height, string heightunit)
        {
            // 修正赋值，确保将参数保存到实例属性
            this.curveArrArray = curvearrArray;
            this.exportPath = exportpath;
            this.exportName = exportName;
            this.height = height;
            this.heightunit = heightunit;
        }
    }
}
