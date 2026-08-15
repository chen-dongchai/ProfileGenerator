using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProfileGenerator.Export
{
    public class DWG2DExporter : IExternalEventHandler
    {
        //需整理的有Application类型的属性方法   NewFamilyDocument(),Plane,Extrusion，SketchPlane, View3D,DWGExportOptions还有针对轮廓的过滤器 
        public CurveArrArray curveArrArray { get; set; }
        public string exportPath { get; set; }
        public string exportName { get; set; } = "ProfileExport.dwg"; // 默认导出文件名
        public void Execute(UIApplication application)
        {
            try
            {
                // 重用通用导出方法，保持 Execute 简洁
                string outPath;
                string error;
                bool ok = TryExportCurveArrToDwg(exportName, application.Application, curveArrArray, exportPath, out outPath, out error);   //问题来自该方法

                if (ok)
                {
                    TaskDialog.Show("完成", $"DWG 文件已生成：\n{outPath}");
                }
                else
                {
                    TaskDialog.Show("错误", $"导出失败：\n{error}");
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("错误", $"导出失败：{ex.Message}");   //有问题
            }
        }

        /// <summary>
        /// 通用方法：将二维 CurveArrArray 导出为 DWG（在临时族文档中创建一个很薄的拉伸体并导出某个 3D 视图）。
        /// - 不修改项目中其它类。
        /// - 输入 curveArrArray 必须是合法的平面闭合轮廓。
        /// </summary>
        /// <param name="app">Revit Application（可从 UIApplication.Application 获得）</param>
        /// <param name="curveArr">要导出的二维轮廓（CurveArrArray）</param>
        /// <param name="exportPath">可为文件夹或完整文件路径（可含文件名）</param>
        /// <param name="outFullPath">成功时返回完整生成文件路径</param>
        /// <param name="error">失败时返回错误描述</param>
        /// <returns>导出是否成功</returns>
        public static bool TryExportCurveArrToDwg(string fileName, Application app, CurveArrArray curveArr, string exportPath, out string outFullPath, out string error)
        {
            outFullPath = null;
            error = null;

            if (curveArr == null)
            {
                error = "要导出的几何为空。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(exportPath))
            {
                error = "未设置导出路径。";
                return false;
            }

            string folder;

            try
            {
                if (Directory.Exists(exportPath))
                {
                    folder = exportPath;
                    fileName = "ProfileExport.dwg";
                }
                else
                {
                    folder = Path.GetDirectoryName(exportPath);
                    fileName = Path.GetFileName(exportPath);

                    if (string.IsNullOrEmpty(folder))
                    {
                        // 若只提供了文件名，则使用桌面作为默认目录
                        folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                }

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // 选择族模板：优先中文/英文，再做全目录查找
                string chineseTemplate = Path.Combine(app.FamilyTemplatePath, "公制常规模型.rft");
                string englishTemplate = Path.Combine(app.FamilyTemplatePath, "Metric Generic Model.rft");

                string templatepath = null;
                if (File.Exists(chineseTemplate))
                {
                    templatepath = chineseTemplate;
                }
                else if (File.Exists(englishTemplate))
                {
                    templatepath = englishTemplate;
                }
                else
                {
                    try
                    {
                        var rfts = Directory.GetFiles(app.FamilyTemplatePath, "*.rft", SearchOption.AllDirectories);
                        if (rfts.Length > 0)
                        {
                            templatepath = rfts.First();
                        }
                    }
                    catch
                    {
                        // 忽略异常，下面会报告找不到模板
                    }
                }

                if (string.IsNullOrEmpty(templatepath) || !File.Exists(templatepath))
                {
                    error = $"未找到族模板文件 (.rft)。尝试的路径：\n{chineseTemplate}\n{englishTemplate}\n请检查 Revit 安装或 FamilyTemplatePath 设置。";
                    return false;
                }

                Document familyDocument = null;
                try
                {
                    familyDocument = app.NewFamilyDocument(templatepath);
                }
                catch (Exception ex)
                {
                    error = $"创建临时族文档失败：{ex.Message}";
                    return false;
                }

                try
                {
                    using (Transaction trans = new Transaction(familyDocument, "临时轮廓"))
                    {
                        trans.Start();

                        // 在 XY 面上创建 sketch plane，z = 0
                        Plane plane = Plane.CreateByThreePoints(new XYZ(0, 0, 0), new XYZ(1, 0, 0), new XYZ(0, 1, 0));
                        SketchPlane sketchplane = SketchPlane.Create(familyDocument, plane);

                        // 需要一个非常薄的拉伸厚度（内部单位），保证形体可见但不会影响二维轮廓本意
                        double thinThickness = UnitUtils.ConvertToInternalUnits(1.0, UnitTypeId.Millimeters); // 1 mm

                        // 创建拉伸体。假定 curveArr 已经在同一平面上且为闭合轮廓。
                        familyDocument.FamilyCreate.NewExtrusion(true, curveArr, sketchplane, thinThickness);

                        trans.Commit();
                    }

                    // 获取一个非模板的 3D 视图导出
                    FilteredElementCollector viewCollector = new FilteredElementCollector(familyDocument);
                    View3D view3D = viewCollector.OfClass(typeof(View3D))
                                                  .Cast<View3D>()
                                                  .FirstOrDefault(v => !v.IsTemplate);

                    if (view3D == null)
                    {
                        familyDocument.Close(false);
                        error = "无法获取 3D 视图，导出失败。";
                        return false;
                    }

                    DWGExportOptions options = new DWGExportOptions();
                    options.FileVersion = ACADVersion.R2018;

                    List<ElementId> viewIds = new List<ElementId> { view3D.Id };

                    bool success = familyDocument.Export(folder, fileName, viewIds, options);
                    familyDocument.Close(false);

                    outFullPath = Path.Combine(folder, fileName);
                    if (!success)
                    {
                        error = "导出完成但 API 返回未成功，请检查输出文件或日志。";
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    try { familyDocument.Close(false); } catch { }
                    error = $"导出过程中发生异常：{ex.Message}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = $"导出失败：{ex.Message}";
                return false;
            }
        }

        public string GetName()
        {
            return "DWGExporter";
        }
        private View3D GetDefault3DView(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            return collector.OfClass(typeof(View3D))
                            .Cast<View3D>()
                            .FirstOrDefault(v => !v.IsTemplate);
        }
        public void GetCurveArr(CurveArrArray curvearrArray, string exportpath, string exportName)
        {
            // 修正赋值，确保将参数保存到实例属性
            this.curveArrArray = curvearrArray;
            this.exportPath = exportpath;
            this.exportName = exportName;
        }
    }


}
