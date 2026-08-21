using Autodesk.Revit.DB;
using NetTopologySuite.Geometries;
using NetTopologySuite.Triangulate;
using ProfileGenerator.Core.Models.Arrangement;
using ProfileGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = NetTopologySuite.Geometries.Point;

namespace ProfileGenerator.Core.Arrangement
{
    internal class VoronoiArrangementEngine
    {
        // 最小线段长度（英尺），Revit 安全阈值
        private const double MinSegmentLength = 0.005;

        /// <summary>
        /// 生成内部环并返回 CurveArrArray（仅包含内部环，方向顺时针）。
        /// </summary>
        /// <param name="scaledBoundary">缩放后的外部环（作为图案边界）</param>
        /// <param name="voronoiArrange.targetCount">期望的点数/内部环数量</param>
        /// <param name="voronoiArrange.gapFt">内部环之间的期望间距（英尺）</param>
        /// <param name="voronoiArrange.seed">随机种子，0 表示随机</param>
        /// <returns>包含所有内部环的 CurveArrArray，若失败返回 null</returns>
        public static CurveArrArray GenerateInnerLoops(CurveLoop scaledBoundary, VoronoiArrange voronoiArrange)
        {
            if (scaledBoundary == null || voronoiArrange.targetCount < 3 || voronoiArrange.gapFt <= 0)
                return null;

            // 1. 将缩放边界离散为多边形点列表，并转换为 NTS Polygon
            List<XYZ> boundaryPoints = DiscretizeCurveLoop(scaledBoundary);
            if (boundaryPoints.Count < 3)
                return null;

            Polygon boundaryPolygon = CreateNTSPolygon(boundaryPoints);
            if (boundaryPolygon == null || !boundaryPolygon.IsValid)
                return null;

            // 2. 在边界内随机生成点
            List<Coordinate> sites = GenerateRandomSites(boundaryPolygon, voronoiArrange.targetCount, voronoiArrange.seed);
            if (sites.Count < 3)
                return null;

            // 3. 使用 NTS Voronoi 生成器
            var voronoiBuilder = new VoronoiDiagramBuilder();
            Envelope envelope = boundaryPolygon.EnvelopeInternal;
            voronoiBuilder.ClipEnvelope = envelope;
            voronoiBuilder.SetSites(sites);
            var geometries = voronoiBuilder.GetDiagram(GeometryFactory.Floating);

            CurveArrArray result = new CurveArrArray();
            foreach (var geom in geometries)
            {
                if (!(geom is Polygon cellPolygon))
                    continue;

                // 4. 与边界求交，确保完全在内部
                Geometry intersection = cellPolygon.Intersection(boundaryPolygon);
                if (intersection == null || intersection.IsEmpty)
                    continue;

                // 可能是 Polygon 或 MultiPolygon，遍历每个 Polygon
                List<Polygon> polygons = new List<Polygon>();
                if (intersection is Polygon p)
                    polygons.Add(p);
                else if (intersection is MultiPolygon mp)
                    polygons.AddRange(mp.Geometries.Cast<Polygon>());

                foreach (var poly in polygons)
                {
                    // 5. 部分内缩：外框边不缩，内部边内缩 voronoiArrange.gapFt/2
                    List<Coordinate> shrunkVerts = PartialShrink(poly, boundaryPolygon, voronoiArrange.gapFt / 2.0);
                    if (shrunkVerts == null || shrunkVerts.Count < 3)
                        continue;

                    // 6. 清理短边
                    shrunkVerts = CleanShortEdges(shrunkVerts, MinSegmentLength);
                    if (shrunkVerts.Count < 3)
                        continue;

                    // 7. 构建 CurveLoop（确保顺时针）
                    CurveLoop loop = BuildCurveLoopFromVerts(shrunkVerts);
                    if (loop != null && loop.Any())
                    {
                        loop = EnsureClockwise(loop);
                        if (loop != null && loop.Any())
                            result.Append(LoopToArray.ConvertToCurveArray(loop));
                    }
                }
            }

            return result;
        }

        // ---------- 辅助方法 ----------

        /// <summary>
        /// 将 CurveLoop 离散为直线段多边形顶点列表（对曲线进行采样）。
        /// </summary>
        private static List<XYZ> DiscretizeCurveLoop(CurveLoop loop)
        {
            var pts = new List<XYZ>();
            foreach (Curve curve in loop)
            {
                if (curve is Line line)
                {
                    pts.Add(line.GetEndPoint(0));
                }
                else
                {
                    // 对曲线密集采样，例如 64 段
                    int segments = 64;
                    for (int i = 0; i < segments; i++)
                    {
                        double t = (double)i / segments;
                        XYZ p = curve.Evaluate(t, true);
                        pts.Add(p);
                    }
                }
            }
            // 去除首尾重复
            if (pts.Count > 1 && pts[0].DistanceTo(pts[pts.Count - 1]) < 1e-9)
                pts.RemoveAt(pts.Count - 1);
            return pts;
        }

        /// <summary>
        /// 由顶点列表创建 NTS Polygon。
        /// </summary>
        private static Polygon CreateNTSPolygon(List<XYZ> pts)
        {
            if (pts.Count < 3)
                return null;
            var coords = new Coordinate[pts.Count + 1];
            for (int i = 0; i < pts.Count; i++)
                coords[i] = new Coordinate(pts[i].X, pts[i].Y);
            coords[pts.Count] = new Coordinate(pts[0].X, pts[0].Y);
            var ring = new LinearRing(coords);
            return new Polygon(ring);
        }

        /// <summary>
        /// 在边界多边形内随机生成点（使用 NTS 的 Contains 判断）。
        /// </summary>
        private static List<Coordinate> GenerateRandomSites(Polygon boundary,int count, int seed)
        {
            var rng = seed == 0 ? new Random() : new Random(seed);
            var sites = new List<Coordinate>();
            
            Envelope env = boundary.EnvelopeInternal;
            int maxTries = count * 100;
            int tries = 0;
            while (sites.Count < count && tries < maxTries)
            {
                tries++;
                double x = env.MinX + rng.NextDouble() * env.Width;
                double y = env.MinY + rng.NextDouble() * env.Height;
                var point = new Point(x, y);
                if (boundary.Contains(point))
                    sites.Add(new Coordinate(x, y));
            }
            return sites;
        }

        /// <summary>
        /// 对多边形执行部分内缩：与 boundary 重合的边不偏移，其余边向内偏移 offset。
        /// 使用半平面裁剪实现，返回逆时针顶点列表（不含重复闭合点）。
        /// </summary>
        private static List<Coordinate> PartialShrink(Polygon cell, Polygon boundary, double offset)
        {
            // 提取单元顶点并确保逆时针
            List<Coordinate> verts = GetCCWVerts(cell);
            if (verts.Count < 3)
                return null;

            int n = verts.Count;
            var planes = new List<HalfPlane>();
            double tol = 1e-6;

            for (int i = 0; i < n; i++)
            {
                Coordinate a = verts[i];
                Coordinate b = verts[(i + 1) % n];
                double dx = b.X - a.X;
                double dy = b.Y - a.Y;
                double len = Math.Sqrt(dx * dx + dy * dy);
                if (len < 1e-12) continue;

                // 内法线（逆时针环的左法线）
                double nx = -dy / len;
                double ny = dx / len;

                // 判断该边是否与 boundary 重合（两端点都在 boundary 的某条边上）
                bool onBoundary = IsEdgeOnBoundary(a, b, boundary, tol);

                // 外框边不偏移，内部边偏移 offset
                double px = onBoundary ? a.X : a.X + nx * offset;
                double py = onBoundary ? a.Y : a.Y + ny * offset;

                planes.Add(new HalfPlane(nx, ny, px, py));
            }

            if (planes.Count < 3)
                return null;

            // 初始裁剪区域：boundary 的包围盒向外扩展
            Envelope env = boundary.EnvelopeInternal;
            double expand = Math.Max(env.Width, env.Height) + Math.Abs(offset) + 1.0;
            var clipRegion = new List<Coordinate>
            {
                new Coordinate(env.MinX - expand, env.MinY - expand),
                new Coordinate(env.MaxX + expand, env.MinY - expand),
                new Coordinate(env.MaxX + expand, env.MaxY + expand),
                new Coordinate(env.MinX - expand, env.MaxY + expand)
            };

            // 依次用每个半平面裁剪
            foreach (var hp in planes)
            {
                clipRegion = ClipByHalfPlane(clipRegion, hp);
                if (clipRegion.Count < 3)
                    break;
            }

            return clipRegion.Count >= 3 ? clipRegion : null;
        }

        /// <summary>
        /// 获取单元外环顶点（逆时针顺序，不含重复闭合点）。
        /// </summary>
        private static List<Coordinate> GetCCWVerts(Polygon cell)
        {
            Coordinate[] coords = cell.ExteriorRing.Coordinates;
            var verts = new List<Coordinate>(coords.Length - 1);
            for (int i = 0; i < coords.Length - 1; i++)
                verts.Add(coords[i]);

            // 计算有符号面积，若为负则反转
            double signedArea = 0;
            for (int i = 0; i < verts.Count; i++)
            {
                Coordinate a = verts[i];
                Coordinate b = verts[(i + 1) % verts.Count];
                signedArea += a.X * b.Y - b.X * a.Y;
            }
            if (signedArea < 0)
                verts.Reverse();

            return verts;
        }

        /// <summary>
        /// 判断边 (a, b) 是否与 boundary 某条边重合（两端点都在 boundary 的同一线段上，容差 tol）。
        /// </summary>
        private static bool IsEdgeOnBoundary(Coordinate a, Coordinate b, Polygon boundary, double tol)
        {
            Coordinate[] boundaryCoords = boundary.ExteriorRing.Coordinates;
            for (int i = 0; i < boundaryCoords.Length - 1; i++)
            {
                Coordinate b1 = boundaryCoords[i];
                Coordinate b2 = boundaryCoords[i + 1];
                if (IsPointOnSegment(a, b1, b2, tol) && IsPointOnSegment(b, b1, b2, tol))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断点 p 是否在线段 (s1, s2) 上（距离 < tol 且投影在线段范围内）。
        /// </summary>
        private static bool IsPointOnSegment(Coordinate p, Coordinate s1, Coordinate s2, double tol)
        {
            double cross = (p.X - s1.X) * (s2.Y - s1.Y) - (p.Y - s1.Y) * (s2.X - s1.X);
            if (Math.Abs(cross) > tol) return false;
            double dot = (p.X - s1.X) * (s2.X - s1.X) + (p.Y - s1.Y) * (s2.Y - s1.Y);
            if (dot < 0) return false;
            double len2 = (s2.X - s1.X) * (s2.X - s1.X) + (s2.Y - s1.Y) * (s2.Y - s1.Y);
            if (dot > len2) return false;
            return true;
        }

        /// <summary>
        /// Sutherland-Hodgman 多边形裁剪
        /// </summary>
        private static List<Coordinate> ClipByHalfPlane(List<Coordinate> poly, HalfPlane hp)
        {
            var result = new List<Coordinate>();
            for (int i = 0; i < poly.Count; i++)
            {
                Coordinate s = poly[i];
                Coordinate e = poly[(i + 1) % poly.Count];
                bool sIn = hp.Inside(s);
                bool eIn = hp.Inside(e);
                if (sIn != eIn)
                    result.Add(Intersect(s, e, hp));
                if (eIn)
                    result.Add(e);
            }
            return result;
        }

        /// <summary>
        /// 线段与半平面边界交点
        /// </summary>
        private static Coordinate Intersect(Coordinate s, Coordinate e, HalfPlane hp)
        {
            double denom = hp.Nx * (e.X - s.X) + hp.Ny * (e.Y - s.Y);
            if (Math.Abs(denom) < 1e-12) return e;
            double t = (hp.Nx * (hp.Px - s.X) + hp.Ny * (hp.Py - s.Y)) / denom;
            t = Math.Max(0, Math.Min(1, t));
            return new Coordinate(s.X + t * (e.X - s.X), s.Y + t * (e.Y - s.Y));
        }

        /// <summary>
        /// 清理短边：删除相邻顶点距离过近的点，保持多边形有效。
        /// </summary>
        private static List<Coordinate> CleanShortEdges(List<Coordinate> verts, double minLen)
        {
            if (verts.Count < 3) return verts;
            var cleaned = new List<Coordinate>();
            foreach (var p in verts)
            {
                if (cleaned.Count == 0 || Distance(cleaned[cleaned.Count - 1], p) > minLen)
                    cleaned.Add(p);
            }
            // 检查首尾距离
            if (cleaned.Count > 1 && Distance(cleaned[0], cleaned[cleaned.Count - 1]) <= minLen)
                cleaned.RemoveAt(cleaned.Count - 1);
            return cleaned;
        }

        private static double Distance(Coordinate a, Coordinate b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 根据顶点列表构建 CurveLoop（直线段，不检查闭合，直接连接首尾）。
        /// </summary>
        private static CurveLoop BuildCurveLoopFromVerts(List<Coordinate> verts)
        {
            if (verts.Count < 3) return null;
            CurveLoop loop = new CurveLoop();
            for (int i = 0; i < verts.Count; i++)
            {
                Coordinate a = verts[i];
                Coordinate b = verts[(i + 1) % verts.Count];
                XYZ p1 = new XYZ(a.X, a.Y, 0);
                XYZ p2 = new XYZ(b.X, b.Y, 0);
                if (p1.DistanceTo(p2) > MinSegmentLength)
                    loop.Append(Line.CreateBound(p1, p2));
            }
            return loop;
        }

        /// <summary>
        /// 确保 CurveLoop 为顺时针方向（面积符号为负）。
        /// </summary>
        private static CurveLoop EnsureClockwise(CurveLoop loop)
        {
            // 使用顶点列表计算有符号面积
            List<XYZ> pts = new List<XYZ>();
            foreach (Curve curve in loop)
            {
                pts.Add(curve.GetEndPoint(0));
            }
            // 去除最后重复点（若存在）
            if (pts.Count > 1 && pts[0].DistanceTo(pts[pts.Count - 1]) < 1e-9)
                pts.RemoveAt(pts.Count - 1);

            if (pts.Count < 3) return loop;
            double signedArea = 0;
            for (int i = 0; i < pts.Count; i++)
            {
                var a = pts[i];
                var b = pts[(i + 1) % pts.Count];
                signedArea += a.X * b.Y - b.X * a.Y;
            }
            // 若面积为正（逆时针），则反转
            if (signedArea > 0)
            {
                var curves = loop.ToList();
                curves.Reverse();
                var reversedLoop = new CurveLoop();
                foreach (var c in curves)
                    reversedLoop.Append(c.CreateReversed());
                return reversedLoop;
            }
            return loop;
        }

        /// <summary>
        /// 半平面结构
        /// </summary>
        private struct HalfPlane
        {
            public double Nx, Ny, Px, Py;
            public HalfPlane(double nx, double ny, double px, double py)
            {
                Nx = nx; Ny = ny; Px = px; Py = py;
            }
            public bool Inside(Coordinate p) => (p.X - Px) * Nx + (p.Y - Py) * Ny >= -1e-9;
        }
    }

}
