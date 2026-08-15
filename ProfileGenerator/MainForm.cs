using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
using System.Windows.Forms;

namespace ProfileGenerator
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private DWG2DExporter _DWG2DExporter;
        private ExternalEvent _DWG2DEvent;

        private RFA3DExporter _RFA3DExporter;
        private ExternalEvent _RFA3DEvent;
        public MainForm()
        {
            InitializeComponent();
            outlineUnitBox.SelectedIndex = 0;
            patternUnitBox.SelectedIndex = 0;
            arrangeUnitBox.SelectedIndex = 0;
            height3DUnitBox.SelectedIndex = 0;
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
        public void GetHandler(DWG2DExporter dWGExporter, ExternalEvent externalEvent, RFA3DExporter rFA3DExporter, ExternalEvent rFa3DEvent)
        {
            _DWG2DExporter = dWGExporter;
            _DWG2DEvent = externalEvent;
            _RFA3DExporter = rFA3DExporter;
            _RFA3DEvent = rFa3DEvent;
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
                //生成点位方法需要先获得图案包围框
                CurveLoop tmpPatternLoop = null;
                tmpPatternLoop = patternstorage.Generate(new XYZ(0, 0, 0));
                double xMaxLengthft = 0, yMaxLengthft = 0;
                (xMaxLengthft, yMaxLengthft) = BoxPara.GetBox(tmpPatternLoop);
                //之后需要根据外界环的ShapeDefinition获得Curveloop
                CurveLoop outline = outlinestorage.Generate(new XYZ(0, 0, 0));
                //判断ArrangeDefinition的类型，调用不同的方法，若为GridArrange，则调用ArrangementEngine.GridArrangeSet获得点位
                if (arrangeDef.ArrangeTypeName == "网格")
                {
                    GridArrange gridArrange = arrangeDef as GridArrange;
                    List<XYZ> points = NormalArrangementEngine.GridArrangeSet(xMaxLengthft, yMaxLengthft, outline, gridArrange);
                    //最后调用Assembler.Assemble获得CurveArrArray
                    curveArrArray = Assembler.Assemble(points, outlinestorage, patternstorage);
                }
                else if (arrangeDef.ArrangeTypeName == "交错")
                {
                    StaggerArrange staggerArrange = arrangeDef as StaggerArrange;
                    List<XYZ> points = NormalArrangementEngine.StaggerArrangeSet(xMaxLengthft, yMaxLengthft, outline, staggerArrange);
                    curveArrArray = Assembler.Assemble(points, outlinestorage, patternstorage);
                }
                else if (arrangeDef.ArrangeTypeName == "泊松盘")
                {

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
            return arrangeDef;
        }
    }

}
