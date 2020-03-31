namespace GrblCNC.Controls
{
    partial class ManualControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboJogStep = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.jogButtSpindleCW = new GrblCNC.Controls.JogButton();
            this.label2 = new System.Windows.Forms.Label();
            this.multiSelAxis = new GrblCNC.Controls.MultiSelect();
            this.jogButtAllHome = new GrblCNC.Controls.JogButton();
            this.valueSlideJogSpeedXYZ = new GrblCNC.Controls.ValueSlider();
            this.valueSlideSpinSpeed = new GrblCNC.Controls.ValueSlider();
            this.valueSlideMaxVelocity = new GrblCNC.Controls.ValueSlider();
            this.valueSlideJogSpeedAB = new GrblCNC.Controls.ValueSlider();
            this.valueSlideRapidOver = new GrblCNC.Controls.ValueSlider();
            this.valueSlideSpinOver = new GrblCNC.Controls.ValueSlider();
            this.valueSlideFeedOver = new GrblCNC.Controls.ValueSlider();
            this.jogButtSpindleStop = new GrblCNC.Controls.JogButton();
            this.jogButtSpindleCCW = new GrblCNC.Controls.JogButton();
            this.jogButtProbe = new GrblCNC.Controls.JogButton();
            this.jogButtToolTouchOff = new GrblCNC.Controls.JogButton();
            this.jogButtTouchOff = new GrblCNC.Controls.JogButton();
            this.jogButtYhome = new GrblCNC.Controls.JogButton();
            this.jogButtXhome = new GrblCNC.Controls.JogButton();
            this.jogButtBpos = new GrblCNC.Controls.JogButton();
            this.jogButtBhome = new GrblCNC.Controls.JogButton();
            this.jogButtBneg = new GrblCNC.Controls.JogButton();
            this.jogButtApos = new GrblCNC.Controls.JogButton();
            this.jogButtAhome = new GrblCNC.Controls.JogButton();
            this.jogButtAneg = new GrblCNC.Controls.JogButton();
            this.jogButtZneg = new GrblCNC.Controls.JogButton();
            this.jogButtZhome = new GrblCNC.Controls.JogButton();
            this.jogButtZpos = new GrblCNC.Controls.JogButton();
            this.jogButtYneg = new GrblCNC.Controls.JogButton();
            this.jogButtYpos = new GrblCNC.Controls.JogButton();
            this.jogButtXpos = new GrblCNC.Controls.JogButton();
            this.jogButtXneg = new GrblCNC.Controls.JogButton();
            this.SuspendLayout();
            // 
            // comboJogStep
            // 
            this.comboJogStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboJogStep.FormattingEnabled = true;
            this.comboJogStep.Items.AddRange(new object[] {
            "Continues",
            "5 mm",
            "1 mm",
            "0.5 mm",
            "0.1 mm",
            "0.05 mm",
            "0.01 mm"});
            this.comboJogStep.Location = new System.Drawing.Point(9, 21);
            this.comboJogStep.Name = "comboJogStep";
            this.comboJogStep.Size = new System.Drawing.Size(73, 21);
            this.comboJogStep.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Jog:";
            // 
            // jogButtSpindleCW
            // 
            this.jogButtSpindleCW.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtSpindleCW.Caption = "";
            this.jogButtSpindleCW.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtSpindleCW.FontScale = 0.6F;
            this.jogButtSpindleCW.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtSpindleCW.Id = 0;
            this.jogButtSpindleCW.Image = global::GrblCNC.Properties.Resources.SpinRightIcon;
            this.jogButtSpindleCW.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtSpindleCW.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtSpindleCW.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtSpindleCW.Location = new System.Drawing.Point(117, 170);
            this.jogButtSpindleCW.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.jogButtSpindleCW.Name = "jogButtSpindleCW";
            this.jogButtSpindleCW.Size = new System.Drawing.Size(32, 32);
            this.jogButtSpindleCW.TabIndex = 21;
            this.jogButtSpindleCW.TextOffset = new System.Drawing.Point(0, 0);
            this.toolTip1.SetToolTip(this.jogButtSpindleCW, "Increase spindle RPM / Clockwise");
            this.jogButtSpindleCW.Click += new System.EventHandler(this.jogButtSpindleCW_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Spindle:";
            // 
            // multiSelAxis
            // 
            this.multiSelAxis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(241)))), ((int)(((byte)(245)))));
            this.multiSelAxis.Location = new System.Drawing.Point(9, 129);
            this.multiSelAxis.Name = "multiSelAxis";
            this.multiSelAxis.SelectionTexts = "X|Y|Z|A|B";
            this.multiSelAxis.Size = new System.Drawing.Size(113, 27);
            this.multiSelAxis.TabIndex = 34;
            // 
            // jogButtAllHome
            // 
            this.jogButtAllHome.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtAllHome.Caption = "";
            this.jogButtAllHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtAllHome.FontScale = 0.5F;
            this.jogButtAllHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(199)))), ((int)(((byte)(211)))));
            this.jogButtAllHome.Id = 10;
            this.jogButtAllHome.Image = global::GrblCNC.Properties.Resources.HomeIconFull;
            this.jogButtAllHome.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtAllHome.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtAllHome.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtAllHome.Location = new System.Drawing.Point(130, 126);
            this.jogButtAllHome.Margin = new System.Windows.Forms.Padding(18, 11, 18, 11);
            this.jogButtAllHome.Name = "jogButtAllHome";
            this.jogButtAllHome.Size = new System.Drawing.Size(32, 32);
            this.jogButtAllHome.TabIndex = 33;
            this.jogButtAllHome.TextOffset = new System.Drawing.Point(0, 2);
            this.toolTip1.SetToolTip(this.jogButtAllHome, "Home Axis");
            this.jogButtAllHome.Click += new System.EventHandler(this.AxisHome_click);
            // 
            // valueSlideJogSpeedXYZ
            // 
            this.valueSlideJogSpeedXYZ.DataFormat = "{0:0} mm/min";
            this.valueSlideJogSpeedXYZ.Location = new System.Drawing.Point(6, 334);
            this.valueSlideJogSpeedXYZ.MaxValue = 100F;
            this.valueSlideJogSpeedXYZ.MinValue = 0F;
            this.valueSlideJogSpeedXYZ.Name = "valueSlideJogSpeedXYZ";
            this.valueSlideJogSpeedXYZ.Size = new System.Drawing.Size(271, 14);
            this.valueSlideJogSpeedXYZ.TabIndex = 31;
            this.valueSlideJogSpeedXYZ.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideJogSpeedXYZ.TitleText = "Jog Speed (XYZ):";
            this.valueSlideJogSpeedXYZ.Value = 0F;
            // 
            // valueSlideSpinSpeed
            // 
            this.valueSlideSpinSpeed.DataFormat = "{0:0} RPM";
            this.valueSlideSpinSpeed.Location = new System.Drawing.Point(6, 314);
            this.valueSlideSpinSpeed.MaxValue = 100F;
            this.valueSlideSpinSpeed.MinValue = 0F;
            this.valueSlideSpinSpeed.Name = "valueSlideSpinSpeed";
            this.valueSlideSpinSpeed.Size = new System.Drawing.Size(271, 14);
            this.valueSlideSpinSpeed.TabIndex = 30;
            this.valueSlideSpinSpeed.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideSpinSpeed.TitleText = "Spindle Speed:";
            this.valueSlideSpinSpeed.Value = 0F;
            // 
            // valueSlideMaxVelocity
            // 
            this.valueSlideMaxVelocity.DataFormat = "{0:0} mm/min";
            this.valueSlideMaxVelocity.Location = new System.Drawing.Point(6, 374);
            this.valueSlideMaxVelocity.MaxValue = 100F;
            this.valueSlideMaxVelocity.MinValue = 0F;
            this.valueSlideMaxVelocity.Name = "valueSlideMaxVelocity";
            this.valueSlideMaxVelocity.Size = new System.Drawing.Size(271, 14);
            this.valueSlideMaxVelocity.TabIndex = 29;
            this.valueSlideMaxVelocity.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideMaxVelocity.TitleText = "Max velocity:";
            this.valueSlideMaxVelocity.Value = 0F;
            // 
            // valueSlideJogSpeedAB
            // 
            this.valueSlideJogSpeedAB.DataFormat = "{0:0} deg/min";
            this.valueSlideJogSpeedAB.Location = new System.Drawing.Point(6, 354);
            this.valueSlideJogSpeedAB.MaxValue = 100F;
            this.valueSlideJogSpeedAB.MinValue = 0F;
            this.valueSlideJogSpeedAB.Name = "valueSlideJogSpeedAB";
            this.valueSlideJogSpeedAB.Size = new System.Drawing.Size(271, 14);
            this.valueSlideJogSpeedAB.TabIndex = 28;
            this.valueSlideJogSpeedAB.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideJogSpeedAB.TitleText = "Jog Speed (AB):";
            this.valueSlideJogSpeedAB.Value = 0F;
            // 
            // valueSlideRapidOver
            // 
            this.valueSlideRapidOver.DataFormat = "{0:0} %";
            this.valueSlideRapidOver.Location = new System.Drawing.Point(6, 274);
            this.valueSlideRapidOver.MaxValue = 100F;
            this.valueSlideRapidOver.MinValue = 0F;
            this.valueSlideRapidOver.Name = "valueSlideRapidOver";
            this.valueSlideRapidOver.Size = new System.Drawing.Size(271, 14);
            this.valueSlideRapidOver.TabIndex = 27;
            this.valueSlideRapidOver.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideRapidOver.TitleText = "Rapid override:";
            this.valueSlideRapidOver.Value = 100F;
            // 
            // valueSlideSpinOver
            // 
            this.valueSlideSpinOver.DataFormat = "{0:0} %";
            this.valueSlideSpinOver.Location = new System.Drawing.Point(6, 294);
            this.valueSlideSpinOver.MaxValue = 120F;
            this.valueSlideSpinOver.MinValue = 0F;
            this.valueSlideSpinOver.Name = "valueSlideSpinOver";
            this.valueSlideSpinOver.Size = new System.Drawing.Size(271, 14);
            this.valueSlideSpinOver.TabIndex = 26;
            this.valueSlideSpinOver.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideSpinOver.TitleText = "Spindle override:";
            this.valueSlideSpinOver.Value = 100F;
            // 
            // valueSlideFeedOver
            // 
            this.valueSlideFeedOver.DataFormat = "{0:0} %";
            this.valueSlideFeedOver.Location = new System.Drawing.Point(6, 254);
            this.valueSlideFeedOver.MaxValue = 120F;
            this.valueSlideFeedOver.MinValue = 0F;
            this.valueSlideFeedOver.Name = "valueSlideFeedOver";
            this.valueSlideFeedOver.Size = new System.Drawing.Size(271, 14);
            this.valueSlideFeedOver.TabIndex = 25;
            this.valueSlideFeedOver.ThumbImage = global::GrblCNC.Properties.Resources.SliderThumb;
            this.valueSlideFeedOver.TitleText = "Feed override:";
            this.valueSlideFeedOver.Value = 100F;
            // 
            // jogButtSpindleStop
            // 
            this.jogButtSpindleStop.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtSpindleStop.Caption = "S";
            this.jogButtSpindleStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtSpindleStop.FontScale = 0.6F;
            this.jogButtSpindleStop.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtSpindleStop.Id = 0;
            this.jogButtSpindleStop.Image = global::GrblCNC.Properties.Resources.StopIcon;
            this.jogButtSpindleStop.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtSpindleStop.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtSpindleStop.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtSpindleStop.Location = new System.Drawing.Point(86, 170);
            this.jogButtSpindleStop.Margin = new System.Windows.Forms.Padding(18, 11, 18, 11);
            this.jogButtSpindleStop.Name = "jogButtSpindleStop";
            this.jogButtSpindleStop.Size = new System.Drawing.Size(32, 32);
            this.jogButtSpindleStop.TabIndex = 22;
            this.jogButtSpindleStop.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtSpindleStop.Click += new System.EventHandler(this.jogButtSpindleStop_Click);
            // 
            // jogButtSpindleCCW
            // 
            this.jogButtSpindleCCW.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtSpindleCCW.Caption = "";
            this.jogButtSpindleCCW.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtSpindleCCW.FontScale = 0.6F;
            this.jogButtSpindleCCW.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtSpindleCCW.Id = 0;
            this.jogButtSpindleCCW.Image = global::GrblCNC.Properties.Resources.SpinLeftIcon;
            this.jogButtSpindleCCW.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtSpindleCCW.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtSpindleCCW.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtSpindleCCW.Location = new System.Drawing.Point(55, 170);
            this.jogButtSpindleCCW.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.jogButtSpindleCCW.Name = "jogButtSpindleCCW";
            this.jogButtSpindleCCW.Size = new System.Drawing.Size(32, 32);
            this.jogButtSpindleCCW.TabIndex = 20;
            this.jogButtSpindleCCW.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtSpindleCCW.Click += new System.EventHandler(this.jogButtSpindleCCW_Click);
            // 
            // jogButtProbe
            // 
            this.jogButtProbe.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtProbe.Caption = "";
            this.jogButtProbe.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtProbe.FontScale = 0.5F;
            this.jogButtProbe.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtProbe.Id = 0;
            this.jogButtProbe.Image = global::GrblCNC.Properties.Resources.ProbeIcon;
            this.jogButtProbe.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtProbe.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtProbe.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtProbe.Location = new System.Drawing.Point(246, 126);
            this.jogButtProbe.Margin = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.jogButtProbe.Name = "jogButtProbe";
            this.jogButtProbe.Size = new System.Drawing.Size(32, 32);
            this.jogButtProbe.TabIndex = 19;
            this.jogButtProbe.TextOffset = new System.Drawing.Point(0, 0);
            this.toolTip1.SetToolTip(this.jogButtProbe, "Tool Probe");
            this.jogButtProbe.Click += new System.EventHandler(this.jogButtProbe_Click);
            // 
            // jogButtToolTouchOff
            // 
            this.jogButtToolTouchOff.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtToolTouchOff.Caption = "";
            this.jogButtToolTouchOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtToolTouchOff.FontScale = 0.5F;
            this.jogButtToolTouchOff.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtToolTouchOff.Id = 0;
            this.jogButtToolTouchOff.Image = global::GrblCNC.Properties.Resources.TouchTIcon;
            this.jogButtToolTouchOff.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtToolTouchOff.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtToolTouchOff.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtToolTouchOff.Location = new System.Drawing.Point(206, 126);
            this.jogButtToolTouchOff.Margin = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.jogButtToolTouchOff.Name = "jogButtToolTouchOff";
            this.jogButtToolTouchOff.Size = new System.Drawing.Size(32, 32);
            this.jogButtToolTouchOff.TabIndex = 18;
            this.jogButtToolTouchOff.TextOffset = new System.Drawing.Point(0, 0);
            this.toolTip1.SetToolTip(this.jogButtToolTouchOff, "Tool touch off");
            this.jogButtToolTouchOff.Click += new System.EventHandler(this.jogButtToolTouchOff_Click);
            // 
            // jogButtTouchOff
            // 
            this.jogButtTouchOff.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtTouchOff.Caption = "";
            this.jogButtTouchOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtTouchOff.FontScale = 0.5F;
            this.jogButtTouchOff.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtTouchOff.Id = 0;
            this.jogButtTouchOff.Image = global::GrblCNC.Properties.Resources.TouchGIcon;
            this.jogButtTouchOff.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtTouchOff.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtTouchOff.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtTouchOff.Location = new System.Drawing.Point(167, 126);
            this.jogButtTouchOff.Margin = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.jogButtTouchOff.Name = "jogButtTouchOff";
            this.jogButtTouchOff.Size = new System.Drawing.Size(32, 32);
            this.jogButtTouchOff.TabIndex = 17;
            this.jogButtTouchOff.TextOffset = new System.Drawing.Point(0, 0);
            this.toolTip1.SetToolTip(this.jogButtTouchOff, "WCO Axis touch off");
            this.jogButtTouchOff.Click += new System.EventHandler(this.jogButtTouchOff_Click);
            // 
            // jogButtYhome
            // 
            this.jogButtYhome.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtYhome.Caption = "Y";
            this.jogButtYhome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtYhome.FontScale = 0.5F;
            this.jogButtYhome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(199)))), ((int)(((byte)(211)))));
            this.jogButtYhome.Id = 1;
            this.jogButtYhome.Image = global::GrblCNC.Properties.Resources.HomeIconFull;
            this.jogButtYhome.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtYhome.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtYhome.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtYhome.Location = new System.Drawing.Point(117, 15);
            this.jogButtYhome.Margin = new System.Windows.Forms.Padding(18, 10, 18, 10);
            this.jogButtYhome.Name = "jogButtYhome";
            this.jogButtYhome.Size = new System.Drawing.Size(32, 32);
            this.jogButtYhome.TabIndex = 16;
            this.jogButtYhome.TextOffset = new System.Drawing.Point(0, 2);
            this.jogButtYhome.Click += new System.EventHandler(this.AxisHome_click);
            // 
            // jogButtXhome
            // 
            this.jogButtXhome.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtXhome.Caption = "X";
            this.jogButtXhome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtXhome.FontScale = 0.5F;
            this.jogButtXhome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(199)))), ((int)(((byte)(211)))));
            this.jogButtXhome.Id = 0;
            this.jogButtXhome.Image = global::GrblCNC.Properties.Resources.HomeIconFull;
            this.jogButtXhome.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtXhome.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtXhome.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtXhome.Location = new System.Drawing.Point(55, 77);
            this.jogButtXhome.Margin = new System.Windows.Forms.Padding(18, 11, 18, 11);
            this.jogButtXhome.Name = "jogButtXhome";
            this.jogButtXhome.Size = new System.Drawing.Size(32, 32);
            this.jogButtXhome.TabIndex = 15;
            this.jogButtXhome.TextOffset = new System.Drawing.Point(0, 2);
            this.jogButtXhome.Click += new System.EventHandler(this.AxisHome_click);
            // 
            // jogButtBpos
            // 
            this.jogButtBpos.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Up;
            this.jogButtBpos.Caption = "+B";
            this.jogButtBpos.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtBpos.FontScale = 0.6F;
            this.jogButtBpos.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtBpos.Id = 4;
            this.jogButtBpos.Image = null;
            this.jogButtBpos.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtBpos.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtBpos.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtBpos.Location = new System.Drawing.Point(246, 5);
            this.jogButtBpos.Margin = new System.Windows.Forms.Padding(18, 11, 18, 11);
            this.jogButtBpos.Name = "jogButtBpos";
            this.jogButtBpos.Size = new System.Drawing.Size(32, 42);
            this.jogButtBpos.TabIndex = 14;
            this.jogButtBpos.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtBpos.Click += new System.EventHandler(this.AxisPos_click);
            this.jogButtBpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisPos_down);
            this.jogButtBpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtBhome
            // 
            this.jogButtBhome.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtBhome.Caption = "B";
            this.jogButtBhome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtBhome.FontScale = 0.5F;
            this.jogButtBhome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(199)))), ((int)(((byte)(211)))));
            this.jogButtBhome.Id = 4;
            this.jogButtBhome.Image = global::GrblCNC.Properties.Resources.HomeIconFull;
            this.jogButtBhome.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtBhome.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtBhome.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtBhome.Location = new System.Drawing.Point(246, 46);
            this.jogButtBhome.Margin = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.jogButtBhome.Name = "jogButtBhome";
            this.jogButtBhome.Size = new System.Drawing.Size(32, 32);
            this.jogButtBhome.TabIndex = 13;
            this.jogButtBhome.TextOffset = new System.Drawing.Point(0, 2);
            this.jogButtBhome.Click += new System.EventHandler(this.AxisHome_click);
            // 
            // jogButtBneg
            // 
            this.jogButtBneg.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Down;
            this.jogButtBneg.Caption = "-B";
            this.jogButtBneg.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtBneg.FontScale = 0.6F;
            this.jogButtBneg.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtBneg.Id = 4;
            this.jogButtBneg.Image = null;
            this.jogButtBneg.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtBneg.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtBneg.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtBneg.Location = new System.Drawing.Point(246, 78);
            this.jogButtBneg.Margin = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.jogButtBneg.Name = "jogButtBneg";
            this.jogButtBneg.Size = new System.Drawing.Size(32, 42);
            this.jogButtBneg.TabIndex = 12;
            this.jogButtBneg.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtBneg.Click += new System.EventHandler(this.AxisNeg_click);
            this.jogButtBneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisNeg_down);
            this.jogButtBneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtApos
            // 
            this.jogButtApos.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Up;
            this.jogButtApos.Caption = "+A";
            this.jogButtApos.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtApos.FontScale = 0.6F;
            this.jogButtApos.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtApos.Id = 3;
            this.jogButtApos.Image = null;
            this.jogButtApos.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtApos.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtApos.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtApos.Location = new System.Drawing.Point(206, 5);
            this.jogButtApos.Margin = new System.Windows.Forms.Padding(18, 13, 18, 13);
            this.jogButtApos.Name = "jogButtApos";
            this.jogButtApos.Size = new System.Drawing.Size(32, 42);
            this.jogButtApos.TabIndex = 11;
            this.jogButtApos.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtApos.Click += new System.EventHandler(this.AxisPos_click);
            this.jogButtApos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisPos_down);
            this.jogButtApos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtAhome
            // 
            this.jogButtAhome.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtAhome.Caption = "A";
            this.jogButtAhome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtAhome.FontScale = 0.5F;
            this.jogButtAhome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(199)))), ((int)(((byte)(211)))));
            this.jogButtAhome.Id = 3;
            this.jogButtAhome.Image = global::GrblCNC.Properties.Resources.HomeIconFull;
            this.jogButtAhome.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtAhome.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtAhome.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtAhome.Location = new System.Drawing.Point(206, 46);
            this.jogButtAhome.Margin = new System.Windows.Forms.Padding(18, 13, 18, 13);
            this.jogButtAhome.Name = "jogButtAhome";
            this.jogButtAhome.Size = new System.Drawing.Size(32, 32);
            this.jogButtAhome.TabIndex = 10;
            this.jogButtAhome.TextOffset = new System.Drawing.Point(0, 2);
            this.jogButtAhome.Click += new System.EventHandler(this.AxisHome_click);
            // 
            // jogButtAneg
            // 
            this.jogButtAneg.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Down;
            this.jogButtAneg.Caption = "-A";
            this.jogButtAneg.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtAneg.FontScale = 0.6F;
            this.jogButtAneg.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtAneg.Id = 3;
            this.jogButtAneg.Image = null;
            this.jogButtAneg.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtAneg.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtAneg.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtAneg.Location = new System.Drawing.Point(206, 77);
            this.jogButtAneg.Margin = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.jogButtAneg.Name = "jogButtAneg";
            this.jogButtAneg.Size = new System.Drawing.Size(32, 42);
            this.jogButtAneg.TabIndex = 9;
            this.jogButtAneg.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtAneg.Click += new System.EventHandler(this.AxisNeg_click);
            this.jogButtAneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisNeg_down);
            this.jogButtAneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtZneg
            // 
            this.jogButtZneg.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Down;
            this.jogButtZneg.Caption = "-Z";
            this.jogButtZneg.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtZneg.FontScale = 0.6F;
            this.jogButtZneg.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtZneg.Id = 2;
            this.jogButtZneg.Image = null;
            this.jogButtZneg.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtZneg.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtZneg.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtZneg.Location = new System.Drawing.Point(167, 77);
            this.jogButtZneg.Margin = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.jogButtZneg.Name = "jogButtZneg";
            this.jogButtZneg.Size = new System.Drawing.Size(32, 42);
            this.jogButtZneg.TabIndex = 8;
            this.jogButtZneg.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtZneg.Click += new System.EventHandler(this.AxisNeg_click);
            this.jogButtZneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisNeg_down);
            this.jogButtZneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtZhome
            // 
            this.jogButtZhome.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Center;
            this.jogButtZhome.Caption = "Z";
            this.jogButtZhome.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtZhome.FontScale = 0.5F;
            this.jogButtZhome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(199)))), ((int)(((byte)(211)))));
            this.jogButtZhome.Id = 2;
            this.jogButtZhome.Image = global::GrblCNC.Properties.Resources.HomeIconFull;
            this.jogButtZhome.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtZhome.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtZhome.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtZhome.Location = new System.Drawing.Point(167, 46);
            this.jogButtZhome.Margin = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.jogButtZhome.Name = "jogButtZhome";
            this.jogButtZhome.Size = new System.Drawing.Size(32, 32);
            this.jogButtZhome.TabIndex = 7;
            this.jogButtZhome.TextOffset = new System.Drawing.Point(0, 2);
            this.jogButtZhome.Click += new System.EventHandler(this.AxisHome_click);
            // 
            // jogButtZpos
            // 
            this.jogButtZpos.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Up;
            this.jogButtZpos.Caption = "+Z";
            this.jogButtZpos.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtZpos.FontScale = 0.6F;
            this.jogButtZpos.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtZpos.Id = 2;
            this.jogButtZpos.Image = null;
            this.jogButtZpos.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtZpos.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtZpos.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtZpos.Location = new System.Drawing.Point(167, 5);
            this.jogButtZpos.Margin = new System.Windows.Forms.Padding(18, 15, 18, 15);
            this.jogButtZpos.Name = "jogButtZpos";
            this.jogButtZpos.Size = new System.Drawing.Size(32, 42);
            this.jogButtZpos.TabIndex = 6;
            this.jogButtZpos.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtZpos.Click += new System.EventHandler(this.AxisPos_click);
            this.jogButtZpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisPos_down);
            this.jogButtZpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtYneg
            // 
            this.jogButtYneg.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Down;
            this.jogButtYneg.Caption = "-Y";
            this.jogButtYneg.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtYneg.FontScale = 0.6F;
            this.jogButtYneg.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtYneg.Id = 1;
            this.jogButtYneg.Image = null;
            this.jogButtYneg.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtYneg.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtYneg.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtYneg.Location = new System.Drawing.Point(86, 77);
            this.jogButtYneg.Margin = new System.Windows.Forms.Padding(18, 15, 18, 15);
            this.jogButtYneg.Name = "jogButtYneg";
            this.jogButtYneg.Size = new System.Drawing.Size(32, 42);
            this.jogButtYneg.TabIndex = 5;
            this.jogButtYneg.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtYneg.Click += new System.EventHandler(this.AxisNeg_click);
            this.jogButtYneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisNeg_down);
            this.jogButtYneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtYpos
            // 
            this.jogButtYpos.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Up;
            this.jogButtYpos.Caption = "+Y";
            this.jogButtYpos.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtYpos.FontScale = 0.6F;
            this.jogButtYpos.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtYpos.Id = 1;
            this.jogButtYpos.Image = null;
            this.jogButtYpos.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtYpos.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtYpos.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtYpos.Location = new System.Drawing.Point(86, 5);
            this.jogButtYpos.Margin = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.jogButtYpos.Name = "jogButtYpos";
            this.jogButtYpos.Size = new System.Drawing.Size(32, 42);
            this.jogButtYpos.TabIndex = 4;
            this.jogButtYpos.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtYpos.Click += new System.EventHandler(this.AxisPos_click);
            this.jogButtYpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisPos_down);
            this.jogButtYpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtXpos
            // 
            this.jogButtXpos.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Right;
            this.jogButtXpos.Caption = "+X";
            this.jogButtXpos.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtXpos.FontScale = 0.6F;
            this.jogButtXpos.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtXpos.Id = 0;
            this.jogButtXpos.Image = null;
            this.jogButtXpos.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtXpos.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtXpos.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtXpos.Location = new System.Drawing.Point(117, 46);
            this.jogButtXpos.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.jogButtXpos.Name = "jogButtXpos";
            this.jogButtXpos.Size = new System.Drawing.Size(42, 32);
            this.jogButtXpos.TabIndex = 1;
            this.jogButtXpos.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtXpos.Click += new System.EventHandler(this.AxisPos_click);
            this.jogButtXpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisPos_down);
            this.jogButtXpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // jogButtXneg
            // 
            this.jogButtXneg.ButtonType = GrblCNC.Controls.JogButton.ArrowDir.Left;
            this.jogButtXneg.Caption = "-X";
            this.jogButtXneg.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.jogButtXneg.FontScale = 0.6F;
            this.jogButtXneg.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.jogButtXneg.Id = 0;
            this.jogButtXneg.Image = null;
            this.jogButtXneg.ImageHover = global::GrblCNC.Properties.Resources.JogButtHover;
            this.jogButtXneg.ImageNormal = global::GrblCNC.Properties.Resources.JogButtNorm;
            this.jogButtXneg.ImagePress = global::GrblCNC.Properties.Resources.JogButtPress;
            this.jogButtXneg.Location = new System.Drawing.Point(45, 46);
            this.jogButtXneg.Margin = new System.Windows.Forms.Padding(18, 17, 18, 17);
            this.jogButtXneg.Name = "jogButtXneg";
            this.jogButtXneg.Size = new System.Drawing.Size(42, 32);
            this.jogButtXneg.TabIndex = 0;
            this.jogButtXneg.TextOffset = new System.Drawing.Point(0, 0);
            this.jogButtXneg.Click += new System.EventHandler(this.AxisNeg_click);
            this.jogButtXneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AxisNeg_down);
            this.jogButtXneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Axis_up);
            // 
            // ManualControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(220)))), ((int)(((byte)(232)))));
            this.Controls.Add(this.multiSelAxis);
            this.Controls.Add(this.jogButtAllHome);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.valueSlideJogSpeedXYZ);
            this.Controls.Add(this.valueSlideSpinSpeed);
            this.Controls.Add(this.valueSlideMaxVelocity);
            this.Controls.Add(this.valueSlideJogSpeedAB);
            this.Controls.Add(this.valueSlideRapidOver);
            this.Controls.Add(this.valueSlideSpinOver);
            this.Controls.Add(this.valueSlideFeedOver);
            this.Controls.Add(this.jogButtSpindleStop);
            this.Controls.Add(this.jogButtSpindleCW);
            this.Controls.Add(this.jogButtSpindleCCW);
            this.Controls.Add(this.jogButtProbe);
            this.Controls.Add(this.jogButtToolTouchOff);
            this.Controls.Add(this.jogButtTouchOff);
            this.Controls.Add(this.jogButtYhome);
            this.Controls.Add(this.jogButtXhome);
            this.Controls.Add(this.jogButtBpos);
            this.Controls.Add(this.jogButtBhome);
            this.Controls.Add(this.jogButtBneg);
            this.Controls.Add(this.jogButtApos);
            this.Controls.Add(this.jogButtAhome);
            this.Controls.Add(this.jogButtAneg);
            this.Controls.Add(this.jogButtZneg);
            this.Controls.Add(this.jogButtZhome);
            this.Controls.Add(this.jogButtZpos);
            this.Controls.Add(this.jogButtYneg);
            this.Controls.Add(this.jogButtYpos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboJogStep);
            this.Controls.Add(this.jogButtXpos);
            this.Controls.Add(this.jogButtXneg);
            this.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.Name = "ManualControl";
            this.Size = new System.Drawing.Size(300, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private JogButton jogButtXneg;
        private JogButton jogButtXpos;
        private System.Windows.Forms.ComboBox comboJogStep;
        private System.Windows.Forms.Label label1;
        private JogButton jogButtYpos;
        private JogButton jogButtYneg;
        private JogButton jogButtZpos;
        private JogButton jogButtZhome;
        private JogButton jogButtZneg;
        private JogButton jogButtAneg;
        private JogButton jogButtAhome;
        private JogButton jogButtApos;
        private JogButton jogButtBneg;
        private JogButton jogButtBhome;
        private JogButton jogButtBpos;
        private JogButton jogButtXhome;
        private JogButton jogButtYhome;
        private JogButton jogButtTouchOff;
        private JogButton jogButtToolTouchOff;
        private JogButton jogButtProbe;
        private JogButton jogButtSpindleCCW;
        private JogButton jogButtSpindleCW;
        private JogButton jogButtSpindleStop;
        private System.Windows.Forms.ToolTip toolTip1;
        private ValueSlider valueSlideFeedOver;
        private ValueSlider valueSlideSpinOver;
        private ValueSlider valueSlideRapidOver;
        private ValueSlider valueSlideJogSpeedAB;
        private ValueSlider valueSlideMaxVelocity;
        private ValueSlider valueSlideSpinSpeed;
        private ValueSlider valueSlideJogSpeedXYZ;
        private System.Windows.Forms.Label label2;
        private JogButton jogButtAllHome;
        private MultiSelect multiSelAxis;
    }
}
