namespace ProfileGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.outlinePage = new System.Windows.Forms.TabPage();
            this.outlineUnitBox = new System.Windows.Forms.ComboBox();
            this.outlinePanel = new System.Windows.Forms.Panel();
            this.outlineTypeBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.patternPage = new System.Windows.Forms.TabPage();
            this.patternUnitBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.patternPanel = new System.Windows.Forms.Panel();
            this.patternTypeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.arrangePage = new System.Windows.Forms.TabPage();
            this.arrangeUnitBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.arrangePanel = new System.Windows.Forms.Panel();
            this.arrangeBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.exportPage = new System.Windows.Forms.TabPage();
            this.height3DUnitBox = new System.Windows.Forms.ComboBox();
            this.height3DBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.fileNameBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.filePathBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.wallPage = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.outlinePage.SuspendLayout();
            this.patternPage.SuspendLayout();
            this.arrangePage.SuspendLayout();
            this.exportPage.SuspendLayout();
            this.wallPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.outlinePage);
            this.tabControl1.Controls.Add(this.patternPage);
            this.tabControl1.Controls.Add(this.arrangePage);
            this.tabControl1.Controls.Add(this.exportPage);
            this.tabControl1.Controls.Add(this.wallPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(874, 509);
            this.tabControl1.TabIndex = 0;
            // 
            // outlinePage
            // 
            this.outlinePage.Controls.Add(this.outlineUnitBox);
            this.outlinePage.Controls.Add(this.outlinePanel);
            this.outlinePage.Controls.Add(this.outlineTypeBox);
            this.outlinePage.Controls.Add(this.label2);
            this.outlinePage.Controls.Add(this.label1);
            this.outlinePage.Location = new System.Drawing.Point(4, 28);
            this.outlinePage.Name = "outlinePage";
            this.outlinePage.Padding = new System.Windows.Forms.Padding(3);
            this.outlinePage.Size = new System.Drawing.Size(866, 477);
            this.outlinePage.TabIndex = 0;
            this.outlinePage.Text = "外部环设置";
            this.outlinePage.UseVisualStyleBackColor = true;
            // 
            // outlineUnitBox
            // 
            this.outlineUnitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outlineUnitBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.outlineUnitBox.FormattingEnabled = true;
            this.outlineUnitBox.Items.AddRange(new object[] {
            "mm",
            "cm",
            "m",
            "ft",
            "in"});
            this.outlineUnitBox.Location = new System.Drawing.Point(214, 412);
            this.outlineUnitBox.Name = "outlineUnitBox";
            this.outlineUnitBox.Size = new System.Drawing.Size(121, 26);
            this.outlineUnitBox.TabIndex = 4;
            // 
            // outlinePanel
            // 
            this.outlinePanel.BackColor = System.Drawing.Color.Gainsboro;
            this.outlinePanel.Location = new System.Drawing.Point(26, 111);
            this.outlinePanel.Name = "outlinePanel";
            this.outlinePanel.Size = new System.Drawing.Size(820, 257);
            this.outlinePanel.TabIndex = 3;
            // 
            // outlineTypeBox
            // 
            this.outlineTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outlineTypeBox.FormattingEnabled = true;
            this.outlineTypeBox.Items.AddRange(new object[] {
            "矩形",
            "圆形"});
            this.outlineTypeBox.Location = new System.Drawing.Point(196, 63);
            this.outlineTypeBox.Name = "outlineTypeBox";
            this.outlineTypeBox.Size = new System.Drawing.Size(121, 26);
            this.outlineTypeBox.TabIndex = 2;
            this.outlineTypeBox.SelectedIndexChanged += new System.EventHandler(this.outlineTypeBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 415);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "单位:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "外部环类型:";
            // 
            // patternPage
            // 
            this.patternPage.Controls.Add(this.patternUnitBox);
            this.patternPage.Controls.Add(this.label4);
            this.patternPage.Controls.Add(this.patternPanel);
            this.patternPage.Controls.Add(this.patternTypeBox);
            this.patternPage.Controls.Add(this.label3);
            this.patternPage.Location = new System.Drawing.Point(4, 28);
            this.patternPage.Name = "patternPage";
            this.patternPage.Padding = new System.Windows.Forms.Padding(3);
            this.patternPage.Size = new System.Drawing.Size(866, 477);
            this.patternPage.TabIndex = 1;
            this.patternPage.Text = "内部环设置";
            this.patternPage.UseVisualStyleBackColor = true;
            // 
            // patternUnitBox
            // 
            this.patternUnitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.patternUnitBox.FormattingEnabled = true;
            this.patternUnitBox.Items.AddRange(new object[] {
            "mm",
            "cm",
            "m",
            "ft",
            "in"});
            this.patternUnitBox.Location = new System.Drawing.Point(260, 397);
            this.patternUnitBox.Name = "patternUnitBox";
            this.patternUnitBox.Size = new System.Drawing.Size(121, 26);
            this.patternUnitBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 400);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "单位:";
            // 
            // patternPanel
            // 
            this.patternPanel.BackColor = System.Drawing.Color.Gainsboro;
            this.patternPanel.Location = new System.Drawing.Point(24, 107);
            this.patternPanel.Name = "patternPanel";
            this.patternPanel.Size = new System.Drawing.Size(814, 261);
            this.patternPanel.TabIndex = 2;
            // 
            // patternTypeBox
            // 
            this.patternTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.patternTypeBox.FormattingEnabled = true;
            this.patternTypeBox.Items.AddRange(new object[] {
            "矩形",
            "圆形",
            "菱形",
            "星形"});
            this.patternTypeBox.Location = new System.Drawing.Point(240, 66);
            this.patternTypeBox.Name = "patternTypeBox";
            this.patternTypeBox.Size = new System.Drawing.Size(121, 26);
            this.patternTypeBox.TabIndex = 1;
            this.patternTypeBox.SelectedIndexChanged += new System.EventHandler(this.patternTypeBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(85, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "图案类型:";
            // 
            // arrangePage
            // 
            this.arrangePage.Controls.Add(this.arrangeUnitBox);
            this.arrangePage.Controls.Add(this.label8);
            this.arrangePage.Controls.Add(this.arrangePanel);
            this.arrangePage.Controls.Add(this.arrangeBox);
            this.arrangePage.Controls.Add(this.label5);
            this.arrangePage.Location = new System.Drawing.Point(4, 28);
            this.arrangePage.Name = "arrangePage";
            this.arrangePage.Size = new System.Drawing.Size(866, 477);
            this.arrangePage.TabIndex = 2;
            this.arrangePage.Text = "排序方式设置";
            this.arrangePage.UseVisualStyleBackColor = true;
            // 
            // arrangeUnitBox
            // 
            this.arrangeUnitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.arrangeUnitBox.FormattingEnabled = true;
            this.arrangeUnitBox.Items.AddRange(new object[] {
            "mm",
            "cm",
            "m",
            "ft",
            "in"});
            this.arrangeUnitBox.Location = new System.Drawing.Point(200, 415);
            this.arrangeUnitBox.Name = "arrangeUnitBox";
            this.arrangeUnitBox.Size = new System.Drawing.Size(121, 26);
            this.arrangeUnitBox.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(93, 418);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 18);
            this.label8.TabIndex = 3;
            this.label8.Text = "单位:";
            // 
            // arrangePanel
            // 
            this.arrangePanel.BackColor = System.Drawing.Color.Gainsboro;
            this.arrangePanel.Location = new System.Drawing.Point(26, 101);
            this.arrangePanel.Name = "arrangePanel";
            this.arrangePanel.Size = new System.Drawing.Size(811, 269);
            this.arrangePanel.TabIndex = 2;
            // 
            // arrangeBox
            // 
            this.arrangeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.arrangeBox.FormattingEnabled = true;
            this.arrangeBox.Items.AddRange(new object[] {
            "网格排列",
            "交错排列"});
            this.arrangeBox.Location = new System.Drawing.Point(248, 59);
            this.arrangeBox.Name = "arrangeBox";
            this.arrangeBox.Size = new System.Drawing.Size(121, 26);
            this.arrangeBox.TabIndex = 1;
            this.arrangeBox.SelectedIndexChanged += new System.EventHandler(this.arrangeBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "选择排列方式:";
            // 
            // exportPage
            // 
            this.exportPage.Controls.Add(this.height3DUnitBox);
            this.exportPage.Controls.Add(this.height3DBox);
            this.exportPage.Controls.Add(this.label10);
            this.exportPage.Controls.Add(this.label9);
            this.exportPage.Controls.Add(this.button3);
            this.exportPage.Controls.Add(this.button2);
            this.exportPage.Controls.Add(this.button1);
            this.exportPage.Controls.Add(this.fileNameBox);
            this.exportPage.Controls.Add(this.label7);
            this.exportPage.Controls.Add(this.filePathBox);
            this.exportPage.Controls.Add(this.label6);
            this.exportPage.Location = new System.Drawing.Point(4, 28);
            this.exportPage.Name = "exportPage";
            this.exportPage.Size = new System.Drawing.Size(866, 477);
            this.exportPage.TabIndex = 3;
            this.exportPage.Text = "选择导出";
            this.exportPage.UseVisualStyleBackColor = true;
            // 
            // height3DUnitBox
            // 
            this.height3DUnitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.height3DUnitBox.FormattingEnabled = true;
            this.height3DUnitBox.Items.AddRange(new object[] {
            "mm",
            "cm",
            "m",
            "ft",
            "in"});
            this.height3DUnitBox.Location = new System.Drawing.Point(678, 285);
            this.height3DUnitBox.Name = "height3DUnitBox";
            this.height3DUnitBox.Size = new System.Drawing.Size(121, 26);
            this.height3DUnitBox.TabIndex = 10;
            // 
            // height3DBox
            // 
            this.height3DBox.Location = new System.Drawing.Point(512, 284);
            this.height3DBox.Name = "height3DBox";
            this.height3DBox.Size = new System.Drawing.Size(100, 28);
            this.height3DBox.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(389, 287);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 18);
            this.label10.TabIndex = 8;
            this.label10.Text = "三维高度:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(588, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 18);
            this.label9.TabIndex = 7;
            this.label9.Text = "(无需输入后缀名）";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(479, 347);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(196, 68);
            this.button3.TabIndex = 6;
            this.button3.Text = "导出为.rfa文件\r\n拉伸";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(82, 303);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(163, 68);
            this.button2.TabIndex = 5;
            this.button2.Text = "导出为DWG文件\r\n（二维轮廓）\r\n";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(634, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 48);
            this.button1.TabIndex = 4;
            this.button1.Text = "浏览...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileNameBox
            // 
            this.fileNameBox.Location = new System.Drawing.Point(241, 152);
            this.fileNameBox.Name = "fileNameBox";
            this.fileNameBox.Size = new System.Drawing.Size(318, 28);
            this.fileNameBox.TabIndex = 3;
            this.fileNameBox.Text = "PatternGenerator";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(79, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 18);
            this.label7.TabIndex = 2;
            this.label7.Text = "文件名:";
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(241, 52);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.Size = new System.Drawing.Size(318, 28);
            this.filePathBox.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "保存地址:";
            // 
            // wallPage
            // 
            this.wallPage.Controls.Add(this.label11);
            this.wallPage.Location = new System.Drawing.Point(4, 28);
            this.wallPage.Name = "wallPage";
            this.wallPage.Size = new System.Drawing.Size(866, 477);
            this.wallPage.TabIndex = 4;
            this.wallPage.Text = "应用到墙体";
            this.wallPage.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(101, 113);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(611, 72);
            this.label11.TabIndex = 0;
            this.label11.Text = "墙体开洞需要轮廓，而且你现有的核心代码几乎可以直接复用。\r\n\r\n墙体开洞的API：使用 CurveArray（但包含 CurveLoop）\r\nRevit API " +
    "中创建墙体开洞的核心方法是 FamilyItemFactory.NewOpening：\r\n";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 531);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.outlinePage.ResumeLayout(false);
            this.outlinePage.PerformLayout();
            this.patternPage.ResumeLayout(false);
            this.patternPage.PerformLayout();
            this.arrangePage.ResumeLayout(false);
            this.arrangePage.PerformLayout();
            this.exportPage.ResumeLayout(false);
            this.exportPage.PerformLayout();
            this.wallPage.ResumeLayout(false);
            this.wallPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage outlinePage;
        private System.Windows.Forms.TabPage patternPage;
        private System.Windows.Forms.Panel outlinePanel;
        private System.Windows.Forms.ComboBox outlineTypeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage arrangePage;
        private System.Windows.Forms.TabPage exportPage;
        private System.Windows.Forms.ComboBox outlineUnitBox;
        private System.Windows.Forms.ComboBox patternUnitBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel patternPanel;
        private System.Windows.Forms.ComboBox patternTypeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel arrangePanel;
        private System.Windows.Forms.ComboBox arrangeBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox arrangeUnitBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox height3DUnitBox;
        private System.Windows.Forms.TextBox height3DBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage wallPage;
        private System.Windows.Forms.Label label11;
    }
}