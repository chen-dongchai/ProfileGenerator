using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Defination;
using ProfileGenerator.Utils;
using System.Collections.Generic;

namespace ProfileGenerator.Core.Assembler
{
    internal static class Assembler
    {
        public static CurveArrArray Assemble(List<XYZ> points, ShapeDefinition outlineStorage, ShapeDefinition patternStorage)
        {
            //输入List点列表，要求的内部图案，外部环
            //通过内部图案的生成方法在每个点生成内部图案
            //将所有内部图案的曲线集合放入一个CurveArrArray中
            CurveArrArray curveArrArray = new CurveArrArray();
            curveArrArray.Append(LoopToArray.ConvertToCurveArray(outlineStorage.Generate(new XYZ(0, 0, 0))));
            foreach (XYZ point in points)
            {
                CurveLoop patternLoop = patternStorage.Generate(point);

                curveArrArray.Append(LoopToArray.ConvertToCurveArray(patternLoop));
            }
            return curveArrArray;

        }
    }
}
