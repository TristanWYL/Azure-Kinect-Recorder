namespace AzureKinectRecorder
{
    partial class Control
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Control));
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxMircophone = new System.Windows.Forms.ComboBox();
            this.lblRecordingTime = new System.Windows.Forms.Label();
            this.cboxCamera = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnFar = new System.Windows.Forms.RadioButton();
            this.rbtnClose = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cboxSite = new System.Windows.Forms.ComboBox();
            this.btnTune = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.Location = new System.Drawing.Point(51, 258);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(157, 31);
            this.btnPreview.TabIndex = 0;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.Enabled = false;
            this.btnRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRecord.Location = new System.Drawing.Point(51, 336);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(157, 31);
            this.btnRecord.TabIndex = 0;
            this.btnRecord.Text = "Record";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Camera:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Microphone:";
            // 
            // cboxMircophone
            // 
            this.cboxMircophone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMircophone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxMircophone.FormattingEnabled = true;
            this.cboxMircophone.Location = new System.Drawing.Point(8, 152);
            this.cboxMircophone.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.cboxMircophone.Name = "cboxMircophone";
            this.cboxMircophone.Size = new System.Drawing.Size(229, 24);
            this.cboxMircophone.TabIndex = 1;
            this.cboxMircophone.SelectedIndexChanged += new System.EventHandler(this.cboxMircophone_SelectedIndexChanged);
            // 
            // lblRecordingTime
            // 
            this.lblRecordingTime.AutoSize = true;
            this.lblRecordingTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordingTime.ForeColor = System.Drawing.Color.Gray;
            this.lblRecordingTime.Location = new System.Drawing.Point(38, 375);
            this.lblRecordingTime.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblRecordingTime.Name = "lblRecordingTime";
            this.lblRecordingTime.Size = new System.Drawing.Size(182, 46);
            this.lblRecordingTime.TabIndex = 3;
            this.lblRecordingTime.Text = "00:00:00";
            // 
            // cboxCamera
            // 
            this.cboxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxCamera.FormattingEnabled = true;
            this.cboxCamera.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cboxCamera.Location = new System.Drawing.Point(8, 92);
            this.cboxCamera.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.cboxCamera.Name = "cboxCamera";
            this.cboxCamera.Size = new System.Drawing.Size(229, 24);
            this.cboxCamera.TabIndex = 1;
            this.cboxCamera.SelectedIndexChanged += new System.EventHandler(this.cboxCamera_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnFar);
            this.groupBox1.Controls.Add(this.rbtnClose);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(8, 189);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(229, 47);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Field";
            // 
            // rbtnFar
            // 
            this.rbtnFar.AutoSize = true;
            this.rbtnFar.Location = new System.Drawing.Point(139, 16);
            this.rbtnFar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbtnFar.Name = "rbtnFar";
            this.rbtnFar.Size = new System.Drawing.Size(51, 24);
            this.rbtnFar.TabIndex = 1;
            this.rbtnFar.TabStop = true;
            this.rbtnFar.Text = "Far";
            this.rbtnFar.UseVisualStyleBackColor = true;
            // 
            // rbtnClose
            // 
            this.rbtnClose.AutoSize = true;
            this.rbtnClose.Location = new System.Drawing.Point(28, 16);
            this.rbtnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbtnClose.Name = "rbtnClose";
            this.rbtnClose.Size = new System.Drawing.Size(67, 24);
            this.rbtnClose.TabIndex = 0;
            this.rbtnClose.TabStop = true;
            this.rbtnClose.Text = "Close";
            this.rbtnClose.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Site:";
            // 
            // cboxSite
            // 
            this.cboxSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxSite.FormattingEnabled = true;
            this.cboxSite.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cboxSite.Location = new System.Drawing.Point(8, 32);
            this.cboxSite.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.cboxSite.Name = "cboxSite";
            this.cboxSite.Size = new System.Drawing.Size(229, 24);
            this.cboxSite.TabIndex = 1;
            this.cboxSite.SelectedIndexChanged += new System.EventHandler(this.cboxSite_SelectedIndexChanged);
            // 
            // btnTune
            // 
            this.btnTune.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTune.Location = new System.Drawing.Point(51, 297);
            this.btnTune.Margin = new System.Windows.Forms.Padding(1);
            this.btnTune.Name = "btnTune";
            this.btnTune.Size = new System.Drawing.Size(157, 31);
            this.btnTune.TabIndex = 0;
            this.btnTune.Text = "Auto-Tune";
            this.btnTune.UseVisualStyleBackColor = true;
            this.btnTune.Click += new System.EventHandler(this.btnTune_Click);
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 433);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblRecordingTime);
            this.Controls.Add(this.cboxMircophone);
            this.Controls.Add(this.cboxSite);
            this.Controls.Add(this.cboxCamera);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.btnTune);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Control";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control Center";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Control_FormClosing);
            this.Load += new System.EventHandler(this.Control_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboxMircophone;
        private System.Windows.Forms.Label lblRecordingTime;
        private System.Windows.Forms.ComboBox cboxCamera;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnFar;
        private System.Windows.Forms.RadioButton rbtnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboxSite;
        private System.Windows.Forms.Button btnTune;
    }
}