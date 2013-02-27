namespace JeeAWattVis
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.serial = new System.IO.Ports.SerialPort(this.components);
            this.chtVis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblAmpBase = new System.Windows.Forms.Label();
            this.lblVoltBase = new System.Windows.Forms.Label();
            this.btnBase = new System.Windows.Forms.Button();
            this.lblAmpTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnBaseSingle = new System.Windows.Forms.Button();
            this.cbxMode = new System.Windows.Forms.ComboBox();
            this.lblWatts = new System.Windows.Forms.Label();
            this.lblVolts = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPowerFactor = new System.Windows.Forms.Label();
            this.txtVoltFactor = new System.Windows.Forms.TextBox();
            this.chkShowLabels = new System.Windows.Forms.CheckBox();
            this.txtAmpFactor = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chtVis)).BeginInit();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // serial
            // 
            this.serial.BaudRate = 115200;
            this.serial.PortName = "COM3";
            // 
            // chtVis
            // 
            this.chtVis.BorderlineColor = System.Drawing.Color.MidnightBlue;
            this.chtVis.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chtVis.BorderlineWidth = 2;
            this.chtVis.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.Name = "ChartArea1";
            this.chtVis.ChartAreas.Add(chartArea1);
            this.chtVis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chtVis.Location = new System.Drawing.Point(113, 0);
            this.chtVis.Margin = new System.Windows.Forms.Padding(0);
            this.chtVis.Name = "chtVis";
            this.chtVis.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.IsValueShownAsLabel = true;
            series1.MarkerSize = 1;
            series1.Name = "srsVolt";
            series1.ShadowOffset = 1;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.IsValueShownAsLabel = true;
            series2.Name = "srsAmp";
            series2.ShadowOffset = 1;
            this.chtVis.Series.Add(series1);
            this.chtVis.Series.Add(series2);
            this.chtVis.Size = new System.Drawing.Size(542, 434);
            this.chtVis.TabIndex = 1;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lblPowerFactor);
            this.pnlLeft.Controls.Add(this.txtVoltFactor);
            this.pnlLeft.Controls.Add(this.label2);
            this.pnlLeft.Controls.Add(this.lblVolts);
            this.pnlLeft.Controls.Add(this.lblWatts);
            this.pnlLeft.Controls.Add(this.cbxMode);
            this.pnlLeft.Controls.Add(this.btnBaseSingle);
            this.pnlLeft.Controls.Add(this.lblCount);
            this.pnlLeft.Controls.Add(this.chkShowLabels);
            this.pnlLeft.Controls.Add(this.lblCurrent);
            this.pnlLeft.Controls.Add(this.label1);
            this.pnlLeft.Controls.Add(this.txtAmpFactor);
            this.pnlLeft.Controls.Add(this.lblAmpTotal);
            this.pnlLeft.Controls.Add(this.btnBase);
            this.pnlLeft.Controls.Add(this.lblVoltBase);
            this.pnlLeft.Controls.Add(this.lblAmpBase);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(113, 434);
            this.pnlLeft.TabIndex = 0;
            // 
            // lblAmpBase
            // 
            this.lblAmpBase.AutoSize = true;
            this.lblAmpBase.Location = new System.Drawing.Point(12, 9);
            this.lblAmpBase.Name = "lblAmpBase";
            this.lblAmpBase.Size = new System.Drawing.Size(62, 13);
            this.lblAmpBase.TabIndex = 0;
            this.lblAmpBase.Text = "lblAmpBase";
            // 
            // lblVoltBase
            // 
            this.lblVoltBase.AutoSize = true;
            this.lblVoltBase.Location = new System.Drawing.Point(12, 22);
            this.lblVoltBase.Name = "lblVoltBase";
            this.lblVoltBase.Size = new System.Drawing.Size(59, 13);
            this.lblVoltBase.TabIndex = 1;
            this.lblVoltBase.Text = "lblVoltBase";
            // 
            // btnBase
            // 
            this.btnBase.Location = new System.Drawing.Point(3, 379);
            this.btnBase.Name = "btnBase";
            this.btnBase.Size = new System.Drawing.Size(107, 23);
            this.btnBase.TabIndex = 2;
            this.btnBase.Text = "Full Baseline";
            this.btnBase.UseVisualStyleBackColor = true;
            this.btnBase.Click += new System.EventHandler(this.btnBase_Click);
            // 
            // lblAmpTotal
            // 
            this.lblAmpTotal.AutoSize = true;
            this.lblAmpTotal.Location = new System.Drawing.Point(12, 47);
            this.lblAmpTotal.Name = "lblAmpTotal";
            this.lblAmpTotal.Size = new System.Drawing.Size(62, 13);
            this.lblAmpTotal.TabIndex = 3;
            this.lblAmpTotal.Text = "lblAmpTotal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Amp Factor";
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrent.Location = new System.Drawing.Point(12, 229);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(52, 17);
            this.lblCurrent.TabIndex = 5;
            this.lblCurrent.Text = "0 amps";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(12, 34);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(45, 13);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "lblCount";
            // 
            // btnBaseSingle
            // 
            this.btnBaseSingle.Location = new System.Drawing.Point(3, 408);
            this.btnBaseSingle.Name = "btnBaseSingle";
            this.btnBaseSingle.Size = new System.Drawing.Size(107, 23);
            this.btnBaseSingle.TabIndex = 8;
            this.btnBaseSingle.Text = "1-Shot Baseline";
            this.btnBaseSingle.UseVisualStyleBackColor = true;
            this.btnBaseSingle.Click += new System.EventHandler(this.btnBaseSingle_Click);
            // 
            // cbxMode
            // 
            this.cbxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMode.FormattingEnabled = true;
            this.cbxMode.Items.AddRange(new object[] {
            "Mode: Measure",
            "Mode: Dump"});
            this.cbxMode.Location = new System.Drawing.Point(3, 352);
            this.cbxMode.Name = "cbxMode";
            this.cbxMode.Size = new System.Drawing.Size(107, 21);
            this.cbxMode.TabIndex = 9;
            this.cbxMode.SelectedIndexChanged += new System.EventHandler(this.cbxMode_SelectedIndexChanged);
            // 
            // lblWatts
            // 
            this.lblWatts.AutoSize = true;
            this.lblWatts.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWatts.Location = new System.Drawing.Point(12, 246);
            this.lblWatts.Name = "lblWatts";
            this.lblWatts.Size = new System.Drawing.Size(52, 17);
            this.lblWatts.TabIndex = 10;
            this.lblWatts.Text = "0 watts";
            // 
            // lblVolts
            // 
            this.lblVolts.AutoSize = true;
            this.lblVolts.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolts.Location = new System.Drawing.Point(12, 212);
            this.lblVolts.Name = "lblVolts";
            this.lblVolts.Size = new System.Drawing.Size(49, 17);
            this.lblVolts.TabIndex = 11;
            this.lblVolts.Text = "0 volts";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Volt Factor";
            // 
            // lblPowerFactor
            // 
            this.lblPowerFactor.AutoSize = true;
            this.lblPowerFactor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPowerFactor.Location = new System.Drawing.Point(12, 263);
            this.lblPowerFactor.Name = "lblPowerFactor";
            this.lblPowerFactor.Size = new System.Drawing.Size(52, 17);
            this.lblPowerFactor.TabIndex = 14;
            this.lblPowerFactor.Text = "0.00 PF";
            // 
            // txtVoltFactor
            // 
            this.txtVoltFactor.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::JeeAWattVis.Properties.Settings.Default, "VoltFactor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtVoltFactor.Location = new System.Drawing.Point(15, 150);
            this.txtVoltFactor.Name = "txtVoltFactor";
            this.txtVoltFactor.Size = new System.Drawing.Size(71, 20);
            this.txtVoltFactor.TabIndex = 13;
            this.txtVoltFactor.Tag = "voltFactor=";
            this.txtVoltFactor.Text = global::JeeAWattVis.Properties.Settings.Default.VoltFactor;
            // 
            // chkShowLabels
            // 
            this.chkShowLabels.AutoSize = true;
            this.chkShowLabels.Checked = global::JeeAWattVis.Properties.Settings.Default.PointLabels;
            this.chkShowLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLabels.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::JeeAWattVis.Properties.Settings.Default, "PointLabels", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkShowLabels.Location = new System.Drawing.Point(6, 329);
            this.chkShowLabels.Name = "chkShowLabels";
            this.chkShowLabels.Size = new System.Drawing.Size(80, 17);
            this.chkShowLabels.TabIndex = 6;
            this.chkShowLabels.Text = "Point labels";
            this.chkShowLabels.UseVisualStyleBackColor = true;
            this.chkShowLabels.CheckStateChanged += new System.EventHandler(this.chkShowLabels_CheckStateChanged);
            // 
            // txtAmpFactor
            // 
            this.txtAmpFactor.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::JeeAWattVis.Properties.Settings.Default, "AmpFactor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtAmpFactor.Location = new System.Drawing.Point(15, 111);
            this.txtAmpFactor.Name = "txtAmpFactor";
            this.txtAmpFactor.Size = new System.Drawing.Size(71, 20);
            this.txtAmpFactor.TabIndex = 2;
            this.txtAmpFactor.Tag = "ampFactor=";
            this.txtAmpFactor.Text = global::JeeAWattVis.Properties.Settings.Default.AmpFactor;
            this.txtAmpFactor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAmpFactor_KeyDown);
            this.txtAmpFactor.Leave += new System.EventHandler(this.txtAmpFactor_Leave);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 434);
            this.Controls.Add(this.chtVis);
            this.Controls.Add(this.pnlLeft);
            this.DoubleBuffered = true;
            this.Name = "frmMain";
            this.Text = "JeeAWatt Visualization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chtVis)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serial;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtVis;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblVoltBase;
        private System.Windows.Forms.Label lblAmpBase;
        private System.Windows.Forms.Button btnBase;
        private System.Windows.Forms.Label lblAmpTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAmpFactor;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.CheckBox chkShowLabels;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnBaseSingle;
        private System.Windows.Forms.ComboBox cbxMode;
        private System.Windows.Forms.Label lblWatts;
        private System.Windows.Forms.Label lblVolts;
        private System.Windows.Forms.TextBox txtVoltFactor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPowerFactor;
    }
}

