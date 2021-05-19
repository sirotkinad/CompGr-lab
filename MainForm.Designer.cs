namespace RayTracing
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbPosX = new System.Windows.Forms.TrackBar();
            this.tbPosY = new System.Windows.Forms.TrackBar();
            this.comboSize = new System.Windows.Forms.ComboBox();
            this.comboColor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textCubePosZ = new System.Windows.Forms.TextBox();
            this.textCubePosY = new System.Windows.Forms.TextBox();
            this.textCubePosX = new System.Windows.Forms.TextBox();
            this.buttonAddCube = new System.Windows.Forms.Button();
            this.trackRayTrDepth = new System.Windows.Forms.TrackBar();
            this.radioLight = new System.Windows.Forms.RadioButton();
            this.radioCamera = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRayTrDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // tbPosX
            // 
            this.tbPosX.Location = new System.Drawing.Point(-4, 610);
            this.tbPosX.Margin = new System.Windows.Forms.Padding(4);
            this.tbPosX.Maximum = 50;
            this.tbPosX.Minimum = -50;
            this.tbPosX.Name = "tbPosX";
            this.tbPosX.Size = new System.Drawing.Size(667, 56);
            this.tbPosX.TabIndex = 2;
            this.tbPosX.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbPosX.Value = 2;
            this.tbPosX.Scroll += new System.EventHandler(this.tbPosX_Scroll);
            // 
            // tbPosY
            // 
            this.tbPosY.Location = new System.Drawing.Point(671, 15);
            this.tbPosY.Margin = new System.Windows.Forms.Padding(4);
            this.tbPosY.Maximum = 50;
            this.tbPosY.Minimum = -50;
            this.tbPosY.Name = "tbPosY";
            this.tbPosY.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPosY.Size = new System.Drawing.Size(56, 615);
            this.tbPosY.TabIndex = 3;
            this.tbPosY.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbPosY.Value = 2;
            this.tbPosY.Scroll += new System.EventHandler(this.tbPosY_Scroll);
            // 
            // comboSize
            // 
            this.comboSize.FormattingEnabled = true;
            this.comboSize.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.comboSize.Location = new System.Drawing.Point(748, 337);
            this.comboSize.Margin = new System.Windows.Forms.Padding(4);
            this.comboSize.Name = "comboSize";
            this.comboSize.Size = new System.Drawing.Size(239, 24);
            this.comboSize.TabIndex = 8;
            this.comboSize.Text = "Size";
            this.comboSize.SelectedIndexChanged += new System.EventHandler(this.ComboSize_SelectedIndexChanged);
            // 
            // comboColor
            // 
            this.comboColor.FormattingEnabled = true;
            this.comboColor.Items.AddRange(new object[] {
            "red",
            "green",
            "blue",
            "yellow",
            "white"});
            this.comboColor.Location = new System.Drawing.Point(748, 408);
            this.comboColor.Margin = new System.Windows.Forms.Padding(4);
            this.comboColor.Name = "comboColor";
            this.comboColor.Size = new System.Drawing.Size(239, 24);
            this.comboColor.TabIndex = 7;
            this.comboColor.Text = "Color";
            this.comboColor.SelectedIndexChanged += new System.EventHandler(this.ComboColor_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1109, 224);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Coordinate z:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(920, 224);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Coordinate y:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(749, 224);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Coordinate x:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textCubePosZ
            // 
            this.textCubePosZ.Location = new System.Drawing.Point(1110, 258);
            this.textCubePosZ.Margin = new System.Windows.Forms.Padding(4);
            this.textCubePosZ.Name = "textCubePosZ";
            this.textCubePosZ.Size = new System.Drawing.Size(91, 22);
            this.textCubePosZ.TabIndex = 3;
            this.textCubePosZ.Text = "0";
            // 
            // textCubePosY
            // 
            this.textCubePosY.Location = new System.Drawing.Point(923, 258);
            this.textCubePosY.Margin = new System.Windows.Forms.Padding(4);
            this.textCubePosY.Name = "textCubePosY";
            this.textCubePosY.Size = new System.Drawing.Size(92, 22);
            this.textCubePosY.TabIndex = 2;
            this.textCubePosY.Text = "0";
            // 
            // textCubePosX
            // 
            this.textCubePosX.Location = new System.Drawing.Point(748, 258);
            this.textCubePosX.Margin = new System.Windows.Forms.Padding(4);
            this.textCubePosX.Name = "textCubePosX";
            this.textCubePosX.Size = new System.Drawing.Size(92, 22);
            this.textCubePosX.TabIndex = 1;
            this.textCubePosX.Text = "0";
            // 
            // buttonAddCube
            // 
            this.buttonAddCube.Location = new System.Drawing.Point(1162, 362);
            this.buttonAddCube.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAddCube.Name = "buttonAddCube";
            this.buttonAddCube.Size = new System.Drawing.Size(151, 33);
            this.buttonAddCube.TabIndex = 0;
            this.buttonAddCube.Text = "Add cube";
            this.buttonAddCube.UseVisualStyleBackColor = true;
            this.buttonAddCube.Click += new System.EventHandler(this.buttonAddCube_Click);
            // 
            // trackRayTrDepth
            // 
            this.trackRayTrDepth.Location = new System.Drawing.Point(735, 52);
            this.trackRayTrDepth.Margin = new System.Windows.Forms.Padding(4);
            this.trackRayTrDepth.Minimum = 1;
            this.trackRayTrDepth.Name = "trackRayTrDepth";
            this.trackRayTrDepth.Size = new System.Drawing.Size(539, 56);
            this.trackRayTrDepth.TabIndex = 7;
            this.trackRayTrDepth.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackRayTrDepth.Value = 10;
            this.trackRayTrDepth.Scroll += new System.EventHandler(this.SetRayTracingDepth);
            // 
            // radioLight
            // 
            this.radioLight.AutoSize = true;
            this.radioLight.Checked = true;
            this.radioLight.Location = new System.Drawing.Point(131, 748);
            this.radioLight.Margin = new System.Windows.Forms.Padding(4);
            this.radioLight.Name = "radioLight";
            this.radioLight.Size = new System.Drawing.Size(111, 21);
            this.radioLight.TabIndex = 0;
            this.radioLight.TabStop = true;
            this.radioLight.Text = "Light change";
            this.radioLight.UseVisualStyleBackColor = true;
            this.radioLight.CheckedChanged += new System.EventHandler(this.radioPositionChanged);
            // 
            // radioCamera
            // 
            this.radioCamera.AutoSize = true;
            this.radioCamera.Location = new System.Drawing.Point(131, 699);
            this.radioCamera.Margin = new System.Windows.Forms.Padding(4);
            this.radioCamera.Name = "radioCamera";
            this.radioCamera.Size = new System.Drawing.Size(129, 21);
            this.radioCamera.TabIndex = 1;
            this.radioCamera.Text = "Camera change";
            this.radioCamera.UseVisualStyleBackColor = true;
            this.radioCamera.CheckedChanged += new System.EventHandler(this.radioPositionChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(743, 169);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(271, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "Adding geometric figure cube:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(13, 718);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "Mode:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(743, 15);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "Raytracing depth:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1469, 822);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackRayTrDepth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radioCamera);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioLight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textCubePosX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboColor);
            this.Controls.Add(this.textCubePosY);
            this.Controls.Add(this.comboSize);
            this.Controls.Add(this.textCubePosZ);
            this.Controls.Add(this.tbPosY);
            this.Controls.Add(this.buttonAddCube);
            this.Controls.Add(this.tbPosX);
            this.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "RayTracing, Гондурова, Михайлова, Сироткина, 381803-1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRayTrDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl GLView;
        private System.Windows.Forms.TrackBar tbPosX;
        private System.Windows.Forms.TrackBar tbPosY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textCubePosZ;
        private System.Windows.Forms.TextBox textCubePosY;
        private System.Windows.Forms.TextBox textCubePosX;
        private System.Windows.Forms.Button buttonAddCube;
        private System.Windows.Forms.ComboBox comboSize;
        private System.Windows.Forms.ComboBox comboColor;
        private System.Windows.Forms.TrackBar trackRayTrDepth;
        private System.Windows.Forms.RadioButton radioLight;
        private System.Windows.Forms.RadioButton radioCamera;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

