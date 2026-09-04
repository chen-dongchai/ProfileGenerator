using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Operation.Distance;
using ProfileGenerator.Core.Arrangement;
using ProfileGenerator.Core.Assembler;
using ProfileGenerator.Core.Models.Arrangement;
using ProfileGenerator.Core.Models.Defination;
using ProfileGenerator.Core.Models.Outline;
using ProfileGenerator.Core.Models.Pattern;
using ProfileGenerator.Export;
using ProfileGenerator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace ProfileGenerator
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private DWG2DExporter _DWG2DExporter;
        private ExternalEvent _DWG2DEvent;

        private RFA3DExporter _RFA3DExporter;
        private ExternalEvent _RFA3DEvent;

        private WallExporter _WallExporter;
        private ExternalEvent _WallEvent;

        private UIDocument _uIDocument;
        public MainForm()
        {
            InitializeComponent();
            outlineUnitBox.SelectedIndex = 0;
            patternUnitBox.SelectedIndex = 0;
            arrangeUnitBox.SelectedIndex = 0;
            height3DUnitBox.SelectedIndex = 0;
            outlineTypeBox.SelectedIndex = 0;
            patternTypeBox.SelectedIndex = 0;

        }

        public void GetActiveUIDocument(UIDocument uidocument)
        {
            this._uIDocument = uidocument;
        }
        private void arrangeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = arrangeBox.SelectedItem.ToString();
            if (selectedOption == "网格排列")
            {
                arrangePanel.Controls.Clear();
                Label rowslabel = new Label();
                rowslabel.Text = "行数:";
                rowslabel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox rowsBox = new System.Windows.Forms.TextBox();
                rowsBox.Name = "rowsBox";
                rowsBox.Location = new System.Drawing.Point(150, 50);
                Label colslabel = new Label();
                colslabel.Text = "列数:";
                colslabel.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox colsBox = new System.Windows.Forms.TextBox();
                colsBox.Name = "colsBox";
                colsBox.Location = new System.Drawing.Point(150, 100);

                Label horizontalGaplebel = new Label();
                horizontalGaplebel.Text = "水平间距:";
                horizontalGaplebel.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox horizonGapBox = new System.Windows.Forms.TextBox();
                horizonGapBox.Name = "widBox";
                horizonGapBox.Location = new System.Drawing.Point(150, 150);
                Label verticalGapLabel = new Label();
                verticalGapLabel.Text = "垂直间距:";
                verticalGapLabel.Location = new System.Drawing.Point(50, 200);
                System.Windows.Forms.TextBox verticalGapBox = new System.Windows.Forms.TextBox();
                verticalGapBox.Name = "highBox";
                verticalGapBox.Location = new System.Drawing.Point(150, 200);

                arrangePanel.Controls.Add(rowslabel);
                arrangePanel.Controls.Add(rowsBox);
                arrangePanel.Controls.Add(colslabel);
                arrangePanel.Controls.Add(colsBox);
                arrangePanel.Controls.Add(horizontalGaplebel);
                arrangePanel.Controls.Add(horizonGapBox);
                arrangePanel.Controls.Add(verticalGapLabel);
                arrangePanel.Controls.Add(verticalGapBox);
                arrangePanel.BringToFront();
                arrangePanel.Show();
            }
            else if (selectedOption == "交错排列")
            {
                arrangePanel.Controls.Clear();
                Label rowslabel = new Label();
                rowslabel.Text = "行数:";
                rowslabel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox rowsBox = new System.Windows.Forms.TextBox();
                rowsBox.Name = "rowsBox";
                rowsBox.Location = new System.Drawing.Point(150, 50);
                Label colslabel = new Label();
                colslabel.Text = "列数:";
                colslabel.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox colsBox = new System.Windows.Forms.TextBox();
                colsBox.Name = "colsBox";
                colsBox.Location = new System.Drawing.Point(150, 100);

                Label horizontalGaplebel = new Label();
                horizontalGaplebel.Text = "水平间距:";
                horizontalGaplebel.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox horizonGapBox = new System.Windows.Forms.TextBox();
                horizonGapBox.Name = "widBox";
                horizonGapBox.Location = new System.Drawing.Point(150, 150);
                Label verticalGapLabel = new Label();
                verticalGapLabel.Text = "垂直间距:";
                verticalGapLabel.Location = new System.Drawing.Point(50, 200);
                System.Windows.Forms.TextBox verticalGapBox = new System.Windows.Forms.TextBox();
                verticalGapBox.Name = "highBox";
                verticalGapBox.Location = new System.Drawing.Point(150, 200);


                CheckBox IsRowcheckbox = new CheckBox();
                IsRowcheckbox.Text = "按行偏移";
                IsRowcheckbox.Location = new System.Drawing.Point(300, 50);
                IsRowcheckbox.Width = 150;
                CheckBox IsColcheckbox = new CheckBox();
                IsColcheckbox.Text = "按列偏移";
                IsColcheckbox.Location = new System.Drawing.Point(500, 50);
                IsColcheckbox.Width = 150;
                CheckBox IsOddcheckbox = new CheckBox();
                IsOddcheckbox.Text = "奇数列或行开始";
                IsOddcheckbox.Location = new System.Drawing.Point(300, 100);
                IsOddcheckbox.Width = 200;
                CheckBox IsEvencheckbox = new CheckBox();
                IsEvencheckbox.Text = "偶数列或行开始";
                IsEvencheckbox.Location = new System.Drawing.Point(500, 100);
                IsEvencheckbox.Width = 200;
                Label Offsetlabel = new Label();
                Offsetlabel.Text = "偏移距离:";
                Offsetlabel.Location = new System.Drawing.Point(300, 150);
                System.Windows.Forms.TextBox Offsetbox = new System.Windows.Forms.TextBox();
                Offsetbox.Name = "offset";
                Offsetbox.Location = new System.Drawing.Point(500, 150);

                arrangePanel.Controls.Add(rowslabel);
                arrangePanel.Controls.Add(rowsBox);
                arrangePanel.Controls.Add(colslabel);
                arrangePanel.Controls.Add(colsBox);
                arrangePanel.Controls.Add(horizontalGaplebel);
                arrangePanel.Controls.Add(horizonGapBox);
                arrangePanel.Controls.Add(verticalGapLabel);
                arrangePanel.Controls.Add(verticalGapBox);
                arrangePanel.Controls.Add(IsRowcheckbox);
                arrangePanel.Controls.Add(IsColcheckbox);
                arrangePanel.Controls.Add(IsOddcheckbox);
                arrangePanel.Controls.Add(IsEvencheckbox);
                arrangePanel.Controls.Add(Offsetlabel);
                arrangePanel.Controls.Add(Offsetbox);
                arrangePanel.BringToFront();
                arrangePanel.Show();
            }
            else if (selectedOption == "Voronoi")
            {
                arrangePanel.Controls.Clear();
                Label targetCountLabel = new Label();
                targetCountLabel.Text = "目标数量";
                targetCountLabel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox targetCountBox = new System.Windows.Forms.TextBox();
                targetCountBox.Name = "targetCount";
                targetCountBox.Location = new System.Drawing.Point(150, 50);
                Label gapLabel = new Label();
                gapLabel.Text = "间距";
                gapLabel.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox gapBox = new System.Windows.Forms.TextBox();
                gapBox.Name = "gap";
                gapBox.Location = new System.Drawing.Point(150, 100);
                Label seedLabel = new Label();
                seedLabel.Text = "seed";
                seedLabel.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox seedBox = new System.Windows.Forms.TextBox();
                seedBox.Name = "seed";
                seedBox.Location = new System.Drawing.Point(50, 200);
                seedBox.Width = 180;
                arrangePanel.Controls.Add( targetCountLabel );
                arrangePanel.Controls.Add(targetCountBox);
                arrangePanel.Controls.Add(gapLabel);
                arrangePanel.Controls.Add(gapBox);
                arrangePanel.Controls.Add(seedLabel);
                arrangePanel.Controls.Add(seedBox);
                arrangePanel.BringToFront();
                arrangePanel.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // 设置对话框标题
                folderDialog.Description = "请选择保存位置";

                // 设置默认打开的路径（当前文本框中的路径，如果存在的话）
                if (!string.IsNullOrEmpty(filePathBox.Text) && Directory.Exists(filePathBox.Text))
                {
                    folderDialog.SelectedPath = filePathBox.Text;
                }
                else
                {
                    // 如果文本框为空或路径无效，则从桌面开始
                    folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                // 显示对话框
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    filePathBox.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void outlineTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = outlineTypeBox.SelectedItem.ToString();

            if (selectedOption == "矩形")
            {

                outlinePanel.Controls.Clear();

                Label horizontalGaplebel = new Label();
                horizontalGaplebel.Text = "宽度:";
                horizontalGaplebel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox horizonGapBox = new System.Windows.Forms.TextBox();
                horizonGapBox.Name = "widBox";
                horizonGapBox.Location = new System.Drawing.Point(150, 50);
                Label verticalGapLabel = new Label();
                verticalGapLabel.Text = "高度:";
                verticalGapLabel.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox verticalGapBox = new System.Windows.Forms.TextBox();
                verticalGapBox.Name = "highBox";
                verticalGapBox.Location = new System.Drawing.Point(150, 100);
                Label radiuslabel = new Label();
                radiuslabel.Text = "圆角半径:";
                radiuslabel.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox radiusbox = new System.Windows.Forms.TextBox();
                radiusbox.Name = "radiusbox";
                radiusbox.Text = "0";
                radiusbox.Location = new System.Drawing.Point(150, 150);
                outlinePanel.Controls.Add(horizontalGaplebel);
                outlinePanel.Controls.Add(horizonGapBox);
                outlinePanel.Controls.Add(verticalGapLabel);
                outlinePanel.Controls.Add(verticalGapBox);
                outlinePanel.Controls.Add(radiuslabel);
                outlinePanel.Controls.Add(radiusbox);
                outlinePanel.BringToFront();
                outlinePanel.Show();

            }
            else if (selectedOption == "圆形")
            {
                outlinePanel.Controls.Clear();
                Label radiusLabel = new Label();
                radiusLabel.Text = "半径:";
                radiusLabel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox radiusBox = new System.Windows.Forms.TextBox();
                radiusBox.Name = "radiusBox";
                radiusBox.Location = new System.Drawing.Point(150, 50);
                outlinePanel.Controls.Add(radiusLabel);
                outlinePanel.Controls.Add(radiusBox);
                outlinePanel.BringToFront();
                outlinePanel.Show();
            }
            else if(selectedOption == "点选墙")
            {
                //生成一个按钮，指引用户点选墙，然后根据点选的墙，获取数据，（使按钮隐藏），生成TextBox存储数据
                //数据为矩形外部环数据，圆角半径默认为零，需要宽度，高度，还需要选取的ID，用于定位到它并生成空洞
                //需要一个新的类型用于储存，或者在原本的矩形类中增加重载
                outlinePanel.Controls.Clear();
                Button wallbutton = new Button();
                wallbutton.Text = "选择";
                wallbutton.Size = new System.Drawing.Size(50, 100);
                wallbutton.Location = new System.Drawing.Point(50, 50);
                wallbutton.Click += Wallbutton_Click;
                outlinePanel.Controls.Add(wallbutton);
                outlinePanel.BringToFront();
                outlinePanel.Show();
            }
        }

        private void Wallbutton_Click(object sender, EventArgs e)
        {
            // 1. 确保 UIDocument 已初始化
            if (_uIDocument == null)
            {
                MessageBox.Show("UIDocument 未初始化，请重新启动插件。");
                return;
            }

            Document doc = _uIDocument.Document;
            Reference reference = null;

            // 2. 安全选择墙（处理用户取消）
            try
            {
                reference = _uIDocument.Selection.PickObject(ObjectType.Element, "请选择一面墙");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return; // 用户按 ESC 取消
            }

            if (reference == null) return;

            // 3. 获取并验证为墙
            Element element = doc.GetElement(reference);
            Wall wall = element as Wall;
            if (wall == null)
            {
                MessageBox.Show("选择的不是墙，请重新选择。");
                return;
            }

            // =====================================================
            // 4. 获取实际几何长度（使用 LocationCurve）
            // =====================================================
            double lengthFt = 0;
            LocationCurve locationCurve = wall.Location as LocationCurve;
            if (locationCurve != null)
            {
                lengthFt = locationCurve.Curve.Length; // 内部单位：英尺
            }

            // =====================================================
            // 5. 获取实际几何高度（使用 BoundingBox）
            // =====================================================
            double heightFt = 0;
            BoundingBoxXYZ bbox = wall.get_BoundingBox(null);
            if (bbox != null)
            {
                heightFt = bbox.Max.Z - bbox.Min.Z; // 内部单位：英尺
            }
            else
            {
                // 如果无法获取包围盒，可尝试通过 Solid 计算（备选）
                // 但通常情况下 get_BoundingBox 都能成功
                MessageBox.Show("无法获取墙的包围盒，高度可能不准确。");
            }

            // =====================================================
            // 6. 显示信息（完全保持您原来的 Label 样式）
            // =====================================================
            string heightlabeltop = "高度(ft):";
            string lengthlabeltop = "长度(ft):";
            string idlabeltop = "ID:";

            Label heightlabel = new Label();
            heightlabel.Text = heightlabeltop + heightFt.ToString("F3"); // 保留三位小数，清晰
            heightlabel.Location = new System.Drawing.Point(150, 50);
            heightlabel.AutoSize = true;
            Label lengthlabel = new Label();
            lengthlabel.Text = lengthlabeltop + lengthFt.ToString("F3");
            lengthlabel.Location = new System.Drawing.Point(150, 100);
            lengthlabel.AutoSize = true;
            Label idlabel = new Label();
            idlabel.Text = idlabeltop + wall.Id.IntegerValue.ToString();
            idlabel.Location = new System.Drawing.Point(150, 150);
            idlabel.AutoSize = true;
            // 清空旧控件，避免重复添加
            outlinePanel.Controls.Remove(heightlabel);
            outlinePage.Controls.Remove(lengthlabel);
            outlinePage.Controls.Remove(idlabel);

            outlinePanel.Controls.Add(heightlabel);
            outlinePanel.Controls.Add(lengthlabel);
            outlinePanel.Controls.Add(idlabel);
        }

        private void patternTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = patternTypeBox.SelectedItem.ToString();
            if (selectedOption == "矩形")
            {
                patternPanel.Controls.Clear();

                Label horizontalGaplebel = new Label();
                horizontalGaplebel.Text = "宽度:";
                horizontalGaplebel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox horizonGapBox = new System.Windows.Forms.TextBox();
                horizonGapBox.Name = "widBox";
                horizonGapBox.Location = new System.Drawing.Point(150, 50);
                Label verticalGapLabel = new Label();
                verticalGapLabel.Text = "高度:";
                verticalGapLabel.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox verticalGapBox = new System.Windows.Forms.TextBox();
                verticalGapBox.Name = "highBox";
                verticalGapBox.Location = new System.Drawing.Point(150, 100);
                Label radiuslabel = new Label();
                radiuslabel.Text = "圆角半径:";
                radiuslabel.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox radiusbox = new System.Windows.Forms.TextBox();
                radiusbox.Name = "radiusbox";
                radiusbox.Text = "0";
                radiusbox.Location = new System.Drawing.Point(150, 150);
                Label rotatelabel = new Label();
                rotatelabel.Text = "旋转角度(⁰):";
                rotatelabel.Location = new System.Drawing.Point(50, 200);
                System.Windows.Forms.TextBox rotatebox = new System.Windows.Forms.TextBox();
                rotatebox.Name = "rotatebox";
                rotatebox.Text = "0";
                rotatebox.Location = new System.Drawing.Point(150, 200);
                patternPanel.Controls.Add(horizontalGaplebel);
                patternPanel.Controls.Add(horizonGapBox);
                patternPanel.Controls.Add(verticalGapLabel);
                patternPanel.Controls.Add(verticalGapBox);
                patternPanel.Controls.Add(radiuslabel);
                patternPanel.Controls.Add(radiusbox);
                patternPanel.Controls.Add(rotatelabel);
                patternPanel.Controls.Add(rotatebox);
                patternPanel.BringToFront();
                patternPanel.Show();
            }
            else if (selectedOption == "圆形")
            {
                patternPanel.Controls.Clear();
                Label radiusLabel = new Label();
                radiusLabel.Text = "半径:";
                radiusLabel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox radiusBox = new System.Windows.Forms.TextBox();
                radiusBox.Name = "radiusBox";
                radiusBox.Location = new System.Drawing.Point(150, 50);
                patternPanel.Controls.Add(radiusLabel);
                patternPanel.Controls.Add(radiusBox);
                patternPanel.BringToFront();
                patternPanel.Show();
            }
            else if (selectedOption == "菱形")
            {
                patternPanel.Controls.Clear();
                Label horizontalGaplebel = new Label();
                horizontalGaplebel.Text = "宽度:";
                horizontalGaplebel.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox horizonGapBox = new System.Windows.Forms.TextBox();
                horizonGapBox.Name = "widBox";
                horizonGapBox.Location = new System.Drawing.Point(155, 50);
                Label verticalGapLabel = new Label();
                verticalGapLabel.Text = "高度:";
                verticalGapLabel.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox verticalGapBox = new System.Windows.Forms.TextBox();
                verticalGapBox.Name = "highBox";
                verticalGapBox.Location = new System.Drawing.Point(155, 100);
                Label rotatelabel = new Label();
                rotatelabel.Text = "旋转角度(⁰):";
                rotatelabel.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox rotatebox = new System.Windows.Forms.TextBox();
                rotatebox.Name = "rotatebox";
                rotatebox.Text = "0";
                rotatebox.Location = new System.Drawing.Point(155, 150);
                patternPanel.Controls.Add(horizontalGaplebel);
                patternPanel.Controls.Add(horizonGapBox);
                patternPanel.Controls.Add(verticalGapLabel);
                patternPanel.Controls.Add(verticalGapBox);
                patternPanel.Controls.Add(rotatelabel);
                patternPanel.Controls.Add(rotatebox);
                patternPanel.BringToFront();
                patternPanel.Show();
            }
            else if (selectedOption == "星形")
            {
                patternPanel.Controls.Clear();
                Label inCircleRadius = new Label();
                inCircleRadius.Text = "内接圆半径:";
                inCircleRadius.Location = new System.Drawing.Point(50, 50);
                System.Windows.Forms.TextBox inCircleRadiusBox = new System.Windows.Forms.TextBox();
                inCircleRadiusBox.Name = "inCircleRadius";
                inCircleRadiusBox.Location = new System.Drawing.Point(155, 50);
                Label outCircleRadius = new Label();
                outCircleRadius.Text = "外接圆半径:";
                outCircleRadius.Location = new System.Drawing.Point(50, 100);
                System.Windows.Forms.TextBox outCircleRadiusBox = new System.Windows.Forms.TextBox();
                outCircleRadiusBox.Name = "outCircleRadius";
                outCircleRadiusBox.Location = new System.Drawing.Point(155, 100);
                Label starsCount = new Label();
                starsCount.Text = "角数(最小2):";
                starsCount.Location = new System.Drawing.Point(50, 150);
                System.Windows.Forms.TextBox starsCountBox = new System.Windows.Forms.TextBox();
                starsCountBox.Name = "starsCount";
                starsCountBox.Text = "5";
                starsCountBox.Location = new System.Drawing.Point(155, 150);
                Label rotatelabel = new Label();
                rotatelabel.Text = "旋转角度(⁰):";
                rotatelabel.Location = new System.Drawing.Point(50, 200);
                System.Windows.Forms.TextBox rotatebox = new System.Windows.Forms.TextBox();
                rotatebox.Name = "rotatebox";
                rotatebox.Text = "0";
                rotatebox.Location = new System.Drawing.Point(155, 200);

                patternPanel.Controls.Add(inCircleRadius);
                patternPanel.Controls.Add(inCircleRadiusBox);
                patternPanel.Controls.Add(outCircleRadius);
                patternPanel.Controls.Add(outCircleRadiusBox);
                patternPanel.Controls.Add(starsCount);
                patternPanel.Controls.Add(starsCountBox);
                patternPanel.Controls.Add(rotatelabel);
                patternPanel.Controls.Add(rotatebox);
                patternPanel.BringToFront();
                patternPanel.Show();

            }
        }
        public void GetHandler(DWG2DExporter dWGExporter, ExternalEvent externalEvent, RFA3DExporter rFA3DExporter, ExternalEvent rFa3DEvent,WallExporter wallExporter,ExternalEvent wallEvent)
        {
            _DWG2DExporter = dWGExporter;
            _DWG2DEvent = externalEvent;
            _RFA3DExporter = rFA3DExporter;
            _RFA3DEvent = rFa3DEvent;
            _WallEvent = wallEvent;
            _WallExporter = wallExporter;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                CurveArrArray curveArrArray = new CurveArrArray();
                string exportPath = "";
                (curveArrArray, exportPath) = GetFinalArrayAndPath();

                // 成功得到 curveArrArray 后再传给 DWGExporter 并 Raise
                try
                {
                    _DWG2DExporter.GetCurveArr(curveArrArray, exportPath, fileNameBox.Text);
                    _DWG2DEvent.Raise();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"触发导出事件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"未知错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private (CurveArrArray, string) GetFinalArrayAndPath()
        {
            try
            {

                ShapeDefinition outlinestorage = null;
                ShapeDefinition patternstorage = null;
                ArrangeDefinition arrangeDef = null;
                outlinestorage = GetUserChoosedOutline();
                patternstorage = GetUserChoosedPattern();
                arrangeDef = GetUserChoosedArrange();   //接下来实现根据这三个定义生成 CurveArrArray 的逻辑，还有获取导出路径的逻辑
                CurveArrArray curveArrArray = null;
                //先生成点位，然后组装，
                
                //判断ArrangeDefinition的类型，调用不同的方法，若为GridArrange，则调用ArrangementEngine.GridArrangeSet获得点位
                if (arrangeDef.ArrangeTypeName == "网格")
                {
                    //生成点位方法需要先获得图案包围框
                    CurveLoop tmpPatternLoop = null;
                    tmpPatternLoop = patternstorage.Generate(new XYZ(0, 0, 0));
                    double xMaxLengthft = 0, yMaxLengthft = 0;
                    (xMaxLengthft, yMaxLengthft) = BoxPara.GetBox(tmpPatternLoop);
                    //之后需要根据外界环的ShapeDefinition获得Curveloop
                    CurveLoop outline = outlinestorage.Generate(new XYZ(0, 0, 0));


                    GridArrange gridArrange = arrangeDef as GridArrange;
                    List<XYZ> points = NormalArrangementEngine.GridArrangeSet(xMaxLengthft, yMaxLengthft, outline, gridArrange);
                    //最后调用Assembler.Assemble获得CurveArrArray
                    curveArrArray = Assembler.Assemble(points, outlinestorage, patternstorage);
                }
                else if (arrangeDef.ArrangeTypeName == "交错")
                {

                    //生成点位方法需要先获得图案包围框
                    CurveLoop tmpPatternLoop = null;
                    tmpPatternLoop = patternstorage.Generate(new XYZ(0, 0, 0));
                    double xMaxLengthft = 0, yMaxLengthft = 0;
                    (xMaxLengthft, yMaxLengthft) = BoxPara.GetBox(tmpPatternLoop);
                    //之后需要根据外界环的ShapeDefinition获得Curveloop
                    CurveLoop outline = outlinestorage.Generate(new XYZ(0, 0, 0));



                    StaggerArrange staggerArrange = arrangeDef as StaggerArrange;
                    List<XYZ> points = NormalArrangementEngine.StaggerArrangeSet(xMaxLengthft, yMaxLengthft, outline, staggerArrange);
                    curveArrArray = Assembler.Assemble(points, outlinestorage, patternstorage);
                }
                else if (arrangeDef.ArrangeTypeName == "Voronoi")
                {
                    VoronoiArrange voronoiArrange = arrangeDef as VoronoiArrange;
                    CurveLoop curveLoop = new CurveLoop();
                    curveLoop = Utils.OffsetWay.OutlineOffset(outlinestorage, voronoiArrange.gapFt);//这里的gapft直接采用的内部间距！！！注意是英尺！
                    //生成的外部环的外边距只有gapft的一半，内部环间距正常


                    curveArrArray = VoronoiArrangementEngine.GenerateInnerLoops(curveLoop, voronoiArrange);//这里的curveLoop是外部环缩小后的结果，可能会导致无法生成内部环，返回null
                    CurveLoop originalOutline = new CurveLoop();
                    originalOutline = outlinestorage.Generate(new XYZ(0, 0, 0));

                    curveArrArray.Insert(Utils.LoopToArray.ConvertToCurveArray(originalOutline), 0);
                    //curveArrArray本身为null（触发条件-点选墙，试图输出到墙实例，，，疑问，当点选墙但导出时，偶尔可以正常导出没有内部环的文件
                    //所以这里的null是因为VoronoiArrangementEngine.GenerateInnerLoops返回了null，原因是外部环被缩小后，无法生成内部环？？？？？
                    //需要查看外部环的缩小逻辑，是否存在bug，疑似是间距问题
                    //主要问题，点选墙导出 和 指定外部换导出，VoronoiArrangementEngine.GenerateInnerLoops返回的curveArrArray不一样，前者为null，后者正常------疑似单位问题，点选墙的单位是英尺，指定外部环的单位是毫米，导致缩小后的外部环不同，导致无法生成内部环
                    //大部分时候点选墙无法导出也无法应用到墙实例上，原因是VoronoiArrangementEngine.GenerateInnerLoops返回了null，导致curveArrArray为null，无法应用到墙实例上
                }
                return (curveArrArray, filePathBox.Text);   //别忘记此处确实异常处理
            }
            catch (Exception ex)
            {
                MessageBox.Show($"未知错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (null, null);

            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {



        }

        private void button3_Click(object sender, EventArgs e)
        {
            CurveArrArray curveArrArray = new CurveArrArray();
            string exportPath = "";
            (curveArrArray, exportPath) = GetFinalArrayAndPath();

            double height = double.Parse(height3DBox.Text);            // 缺少异常处理
            _RFA3DExporter.GetCurveArr(curveArrArray, exportPath, fileNameBox.Text, height, height3DUnitBox.Text);
            _RFA3DEvent.Raise();
        }
        public ShapeDefinition GetUserChoosedOutline()    //没有异常处理，调用者需要自行处理异常，！！！注意浮点误差问题，避免“精确比较”导致的错误
        {
            ShapeDefinition shapeDef = null;
            if (outlineTypeBox.SelectedItem?.ToString() == "矩形")
            {
                double outlinewidth = 0, outlineheight = 0, outlineradius = 0;
                string outlineUnit = outlineUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in outlinePanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "widBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out outlinewidth);
                                break;
                            case "highBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out outlineheight);
                                break;
                            case "radiusbox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out outlineradius);
                                break;
                        }
                    }
                }
                shapeDef = new RectangleOutline(outlinewidth, outlineheight, outlineradius, outlineUnit);
            }
            else if (outlineTypeBox.SelectedItem?.ToString() == "圆形")
            {
                double radius = 0;
                string outlineUnit = outlineUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in outlinePanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "radiusBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out radius);
                                break;
                        }
                    }
                }
                shapeDef = new Core.Models.Outline.CircleOutline(radius, outlineUnit);
            }
            else if(outlineTypeBox.SelectedItem?.ToString() == "点选墙")
            {
                //墙是矩形的，所以本质还是一个矩形的外部环数据
                double length = 0;
                double height = 0;
                ElementId elementId = null;
                foreach(System.Windows.Forms.Control control in outlinePanel.Controls)
                {
                    if(control is System.Windows.Forms.Label lb)
                    {
                        string[] strings = lb.Text.Split(':');
                        if (strings[0] == "高度(ft)")
                        {
                            height = double.Parse(strings[1]);
                        }
                        else if (strings[0] == "长度(ft)")
                        {
                            length = double.Parse(strings[1]);
                        }
                        else if ((strings[0] == "ID"))
                        {
                            elementId = new ElementId(int.Parse(strings[1])); 
                        }
                    }
                }
                shapeDef = new WallOutline(length, height, elementId);
            }
            else if (outlineTypeBox.SelectedItem?.ToString() == "不指定")
            {
                shapeDef = null;
            }
            else
            {
                MessageBox.Show("未实现的 Outline 类型。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return shapeDef;

        }
        public ShapeDefinition GetUserChoosedPattern() //没有异常处理，调用者需要自行处理异常，！！！注意浮点误差问题，避免“精确比较”导致的错
        {
            ShapeDefinition shapeDef = null;
            if (patternTypeBox.SelectedItem?.ToString() == "矩形")
            {
                double patternwidth = 0, patternheight = 0, patternradius = 0, patternrotate = 0;
                string patternUnit = outlineUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in patternPanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "widBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out patternwidth);
                                break;
                            case "highBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out patternheight);
                                break;
                            case "radiusbox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out patternradius);
                                break;
                            case "rotatebox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out patternrotate);
                                break;
                        }
                    }
                }
                shapeDef = new RectanglePattern(patternwidth, patternheight, patternradius, patternUnit, patternrotate);
            }
            else if (patternTypeBox.SelectedItem?.ToString() == "圆形")
            {
                double radius = 0;
                string outlineUnit = outlineUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in patternPanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "radiusBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out radius);
                                break;
                        }
                    }
                }
                shapeDef = new Core.Models.Pattern.CirclePattern(radius, outlineUnit);
            }
            else if (patternTypeBox.SelectedItem?.ToString() == "菱形")
            {
                double width = 0, height = 0;
                double rotation = 0;
                string outlineUnit = outlineUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in patternPanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "widBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out width);
                                break;
                            case "highBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out height);
                                break;
                            case "rotatebox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out rotation);
                                break;
                        }
                    }
                }
                shapeDef = new DiamondPattern(width, height, outlineUnit, rotation);

            }
            else if (patternTypeBox.SelectedItem?.ToString() == "星形")
            {
                double inCircleRadius = 0, outCircleRadius = 0;
                int starsCount = 0;
                double rotation = 0;
                string starUnit = outlineUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in patternPanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "inCircleRadius":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out inCircleRadius);
                                break;
                            case "outCircleRadius":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out outCircleRadius);
                                break;
                            case "rotatebox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out rotation);
                                break;
                            case "starsCount":
                                int.TryParse(tb.Text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out starsCount);
                                break;

                        }

                    }

                }
                shapeDef = new StarPattern(inCircleRadius, outCircleRadius, starsCount, starUnit, rotation);

            }
            else if(patternTypeBox.SelectedItem?.ToString() == "不指定")
            {
                shapeDef = null;
            }
            else
            {
                MessageBox.Show("未实现的 Pattern 类型。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return shapeDef;
        }
        public ArrangeDefinition GetUserChoosedArrange()    //没有异常处理，调用者需要自行处理异常，！！！注意浮点误差问题，避免“精确比较”导致的错
        {
            ArrangeDefinition arrangeDef = null;
            if (arrangeBox.SelectedItem?.ToString() == "网格排列")
            {
                int rows = 0, cols = 0;
                double horizontalGap = 0, verticalGap = 0;
                string arrangeUnit = arrangeUnitBox.Text ?? "";
                foreach (System.Windows.Forms.Control control in arrangePanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "rowsBox":
                                int.TryParse(tb.Text, out rows);
                                break;
                            case "colsBox":
                                int.TryParse(tb.Text, out cols);
                                break;
                            case "widBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out horizontalGap);
                                break;
                            case "highBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out verticalGap);
                                break;
                        }
                    }
                }
                arrangeDef = new GridArrange(horizontalGap, verticalGap, arrangeUnit, rows, cols);
            }
            else if (arrangeBox.SelectedItem?.ToString() == "交错排列")
            {
                int rows = 0, cols = 0;
                double horizontalGap = 0, verticalGap = 0;
                double offset = 0;
                string arrangeUnit = arrangeUnitBox.Text ?? "";
                // 用于记录 CheckBox 状态和控件引用
                bool? isRowOrColoffset = null;
                bool? isOddOrEven = null;
                bool rowOffsetChecked = false, colOffsetChecked = false;
                bool oddStartChecked = false, evenStartChecked = false;
                CheckBox rowOffsetCheckBox = null, colOffsetCheckBox = null;
                CheckBox oddStartCheckBox = null, evenStartCheckBox = null;

                foreach (System.Windows.Forms.Control control in arrangePanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "rowsBox":
                                int.TryParse(tb.Text, out rows);
                                break;
                            case "colsBox":
                                int.TryParse(tb.Text, out cols);
                                break;
                            case "widBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out horizontalGap);
                                break;
                            case "highBox":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out verticalGap);
                                break;
                            case "offset":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out offset);
                                break;
                        }
                    }
                    else if (control is System.Windows.Forms.CheckBox cb)
                    {

                        // 仅记录状态和保存控件引用，不做互斥处理
                        switch (cb.Text)
                        {
                            case "按行偏移":
                                rowOffsetChecked = cb.Checked;
                                rowOffsetCheckBox = cb;
                                break;
                            case "按列偏移":
                                colOffsetChecked = cb.Checked;
                                colOffsetCheckBox = cb;
                                break;
                            case "奇数列或行开始":
                                oddStartChecked = cb.Checked;
                                oddStartCheckBox = cb;
                                break;
                            case "偶数列或行开始":
                                evenStartChecked = cb.Checked;
                                evenStartCheckBox = cb;
                                break;
                        }
                    }
                }
                if (rowOffsetChecked && colOffsetChecked)
                {
                    TaskDialog.Show("Notification", "仅可选择一种偏移方式，行/列");
                    // 重置两个复选框
                    if (rowOffsetCheckBox != null) rowOffsetCheckBox.Checked = false;
                    if (colOffsetCheckBox != null) colOffsetCheckBox.Checked = false;
                    return null; // 获取失败，调用者需处理
                }
                else if (rowOffsetChecked)
                {
                    isRowOrColoffset = true;
                }
                else if (colOffsetChecked)
                {
                    isRowOrColoffset = false;
                }

                // ---- 处理“奇偶开始”互斥 ----
                if (oddStartChecked && evenStartChecked)
                {
                    TaskDialog.Show("Notification", "仅可选择奇数或偶数偏移的一种");
                    if (oddStartCheckBox != null) oddStartCheckBox.Checked = false;
                    if (evenStartCheckBox != null) evenStartCheckBox.Checked = false;
                    return null;
                }
                else if (oddStartChecked)
                {
                    isOddOrEven = true;
                }
                else if (evenStartChecked)
                {
                    isOddOrEven = false;
                }
                arrangeDef = new StaggerArrange(horizontalGap, verticalGap, arrangeUnit, rows, cols, offset, isRowOrColoffset, isOddOrEven);

            }
            else if (arrangeBox.SelectedItem?.ToString() == "Voronoi")
            {
                string arrangeUnit = arrangeUnitBox.Text ?? "";
                double gap = 0;
                int seed = 0;
                int targetcount = 0;
                foreach (System.Windows.Forms.Control control in arrangePanel.Controls)
                {
                    if (control is System.Windows.Forms.TextBox tb)
                    {
                        switch (tb.Name)
                        {
                            case "targetCount":
                                int.TryParse(tb.Text, out targetcount);
                                break;
                            case "seed":
                                int.TryParse(tb.Text, out seed);
                                break;
                            case "gap":
                                double.TryParse(tb.Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out gap);
                                break;
                            
                        }
                    }
                }
                arrangeDef = new VoronoiArrange(targetcount, gap, seed, arrangeUnit);
            }
                return arrangeDef;
        }

        private void WallExportButton_Click(object sender, EventArgs e)
        {
            //收集所需参数，传递给IExternalEventHandler接口，调用接口方法生成
            CurveArrArray curveArrArray = new CurveArrArray();
            string Unuse = "";
            (curveArrArray, Unuse) = GetFinalArrayAndPath();
            ElementId elementid = null;
            foreach (System.Windows.Forms.Control control in outlinePanel.Controls)
            {
                if (control is System.Windows.Forms.Label lb)
                {
                    string[] strings = lb.Text.Split(':');
                    
                    if ((strings[0] == "ID"))
                    {
                        elementid = new ElementId(int.Parse(strings[1]));
                    }
                }
            }

            _WallExporter.GetNeed(curveArrArray,elementid);
            _WallEvent.Raise();
        }
    }

}
