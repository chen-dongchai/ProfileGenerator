using Autodesk.Revit.DB;
using ProfileGenerator.Core.Models.Arrangement;
using ProfileGenerator.Utils;
using System.Collections.Generic;

namespace ProfileGenerator.Core.Arrangement
{
    internal static class NormalArrangementEngine
    {
        public static List<XYZ> GridArrangeSet(double xmaxft, double ymaxft, CurveLoop outline, GridArrange gridArrange)
        {
            List<XYZ> result = new List<XYZ>();
            double halfX = xmaxft / 2.0;
            double halfY = ymaxft / 2.0;
            double stepX = xmaxft + gridArrange.HorizontalGapft;
            double stepY = ymaxft + gridArrange.VerticalGapft;
            double xOffset = (gridArrange.Cols - 1) / 2.0;
            double yOffset = (gridArrange.Rows - 1) / 2.0;

            for (int row = 0; row < gridArrange.Rows; row++)
            {
                for (int col = 0; col < gridArrange.Cols; col++)
                {
                    double x = (col - xOffset) * stepX;
                    double y = (yOffset - row) * stepY; // 注意 y 正方向向上
                    result.Add(new XYZ(x, y, 0));
                }
            }

            // 滤除矩形超出 outline 的点
            List<XYZ> filtered = new List<XYZ>();
            const double minHalf = 1e-6;
            if (halfX <= minHalf || halfY <= minHalf)
                return filtered; // 尺寸过小直接返回空

            const double halfShrink = 1e-9; // 英尺，约 0.0003 mm，完全不可见
            double halfXSafe = halfX - halfShrink;
            double halfYSafe = halfY - halfShrink;
            foreach (XYZ point in result)
            {
                XYZ p1 = new XYZ(point.X - halfXSafe, point.Y + halfYSafe, 0);
                XYZ p2 = new XYZ(point.X + halfXSafe, point.Y + halfYSafe, 0);
                XYZ p3 = new XYZ(point.X + halfXSafe, point.Y - halfYSafe, 0);
                XYZ p4 = new XYZ(point.X - halfXSafe, point.Y - halfYSafe, 0);

                CurveLoop rectLoop = new CurveLoop();
                rectLoop.Append(Line.CreateBound(p1, p2));
                rectLoop.Append(Line.CreateBound(p2, p3));
                rectLoop.Append(Line.CreateBound(p3, p4));
                rectLoop.Append(Line.CreateBound(p4, p1));

                if (CurveLoopCheckIsIn.CheckIsIn(rectLoop, outline))
                    filtered.Add(point);
            }

            return filtered;
        }
        public static List<XYZ> StaggerArrangeSet(double xmaxft, double ymaxft, CurveLoop outline, StaggerArrange staggerArrange)
        {
            //首先借用GridArrangeSet生成一个网格点集
            //然后根据StaggerArrange的参数进行错列处理
            //最后滤除超出outline的点
            List<XYZ> result = new List<XYZ>();
            double halfX = xmaxft / 2.0;
            double halfY = ymaxft / 2.0;
            double stepX = xmaxft + staggerArrange.HorizontalGapft;
            double stepY = ymaxft + staggerArrange.VerticalGapft;
            double xOffset = (staggerArrange.Cols - 1) / 2.0;
            double yOffset = (staggerArrange.Rows - 1) / 2.0;

            for (int row = 0; row < staggerArrange.Rows; row++)
            {
                for (int col = 0; col < staggerArrange.Cols; col++)
                {
                    double x = (col - xOffset) * stepX;
                    double y = (yOffset - row) * stepY; // 注意 y 正方向向上
                    result.Add(new XYZ(x, y, 0));
                }
            }
            //根据StaggerArrange的参数进行错列处理
            //参数有  IsRowOrColStagger  IsOddOrEvenStagger  StaggerOffsetFt
            if (staggerArrange.IsRowOrColStagger == true) //行错列
            {
                if (staggerArrange.IsOddOrEvenStagger == true) //奇偶行错列
                {
                    for (int i = 0; i < staggerArrange.Rows; i++)
                    {
                        if (i % 2 == 1) //奇数行
                        {
                            for (int j = 0; j < staggerArrange.Cols; j++)
                            {
                                int index = i * staggerArrange.Cols + j;
                                result[index] = new XYZ(result[index].X + staggerArrange.StaggerOffsetFt, result[index].Y, result[index].Z);
                            }
                        }
                    }
                }
                else if (staggerArrange.IsOddOrEvenStagger == false) //偶数行错列
                {
                    for (int i = 0; i < staggerArrange.Rows; i++)
                    {
                        if (i % 2 == 0) //偶数行
                        {
                            for (int j = 0; j < staggerArrange.Cols; j++)
                            {
                                int index = i * staggerArrange.Cols + j;
                                result[index] = new XYZ(result[index].X + staggerArrange.StaggerOffsetFt, result[index].Y, result[index].Z);
                            }
                        }
                    }
                }
            }
            else if (staggerArrange.IsRowOrColStagger == false) //列错列
            {

                if (staggerArrange.IsOddOrEvenStagger == true) //奇偶列错列
                {
                    for (int j = 0; j < staggerArrange.Cols; j++)
                    {
                        if (j % 2 == 1) //奇数列
                        {
                            for (int i = 0; i < staggerArrange.Rows; i++)
                            {
                                int index = i * staggerArrange.Cols + j;
                                result[index] = new XYZ(result[index].X, result[index].Y + staggerArrange.StaggerOffsetFt, result[index].Z);
                            }
                        }
                    }
                }
                else if (staggerArrange.IsOddOrEvenStagger == false) //偶数列错列
                {
                    for (int j = 0; j < staggerArrange.Cols; j++)
                    {
                        if (j % 2 == 0) //偶数列
                        {
                            for (int i = 0; i < staggerArrange.Rows; i++)
                            {
                                int index = i * staggerArrange.Cols + j;
                                result[index] = new XYZ(result[index].X, result[index].Y + staggerArrange.StaggerOffsetFt, result[index].Z);
                            }
                        }
                    }
                }
            }
            // 滤除矩形超出 outline 的点
            List<XYZ> filtered = new List<XYZ>();
            const double minHalf = 1e-6;
            if (halfX <= minHalf || halfY <= minHalf)
                return filtered; // 尺寸过小直接返回空

            const double halfShrink = 1e-9; // 英尺，约 0.0003 mm，完全不可见
            double halfXSafe = halfX - halfShrink;
            double halfYSafe = halfY - halfShrink;
            foreach (XYZ point in result)
            {
                XYZ p1 = new XYZ(point.X - halfXSafe, point.Y + halfYSafe, 0);
                XYZ p2 = new XYZ(point.X + halfXSafe, point.Y + halfYSafe, 0);
                XYZ p3 = new XYZ(point.X + halfXSafe, point.Y - halfYSafe, 0);
                XYZ p4 = new XYZ(point.X - halfXSafe, point.Y - halfYSafe, 0);

                CurveLoop rectLoop = new CurveLoop();
                rectLoop.Append(Line.CreateBound(p1, p2));
                rectLoop.Append(Line.CreateBound(p2, p3));
                rectLoop.Append(Line.CreateBound(p3, p4));
                rectLoop.Append(Line.CreateBound(p4, p1));

                if (CurveLoopCheckIsIn.CheckIsIn(rectLoop, outline))
                    filtered.Add(point);
            }

            return filtered;
        }
    }
}
