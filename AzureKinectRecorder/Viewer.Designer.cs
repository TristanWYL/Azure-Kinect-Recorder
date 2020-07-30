namespace AzureKinectRecorder
{
    partial class Viewer
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
            this.pictureBoxColor = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsProducedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsRenderedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSampleRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.probarVolume = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSensitivity = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNumberOfAudioQueue = new System.Windows.Forms.Label();
            this.lblNumberOfVideoQueue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxColor
            // 
            this.pictureBoxColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxColor.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxColor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBoxColor.Name = "pictureBoxColor";
            this.pictureBoxColor.Size = new System.Drawing.Size(861, 469);
            this.pictureBoxColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxColor.TabIndex = 0;
            this.pictureBoxColor.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.fpsProducedLabel,
            this.toolStripStatusLabel5,
            this.fpsRenderedLabel,
            this.toolStripStatusLabel7,
            this.lblSampleRate,
            this.toolStripStatusLabel1,
            this.probarVolume,
            this.toolStripStatusLabel4,
            this.lblSensitivity});
            this.statusStrip1.Location = new System.Drawing.Point(0, 406);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(861, 63);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(73, 58);
            this.toolStripStatusLabel2.Text = "Real FPS:";
            // 
            // fpsProducedLabel
            // 
            this.fpsProducedLabel.AutoSize = false;
            this.fpsProducedLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.fpsProducedLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.fpsProducedLabel.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.fpsProducedLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fpsProducedLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.fpsProducedLabel.Name = "fpsProducedLabel";
            this.fpsProducedLabel.Size = new System.Drawing.Size(60, 58);
            this.fpsProducedLabel.Text = "FPS";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStripStatusLabel5.Margin = new System.Windows.Forms.Padding(20, 4, 0, 3);
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(110, 56);
            this.toolStripStatusLabel5.Text = "Rendered FPS:";
            // 
            // fpsRenderedLabel
            // 
            this.fpsRenderedLabel.AutoSize = false;
            this.fpsRenderedLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.fpsRenderedLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.fpsRenderedLabel.Name = "fpsRenderedLabel";
            this.fpsRenderedLabel.Size = new System.Drawing.Size(60, 58);
            this.fpsRenderedLabel.Text = "FPS";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStripStatusLabel7.Margin = new System.Windows.Forms.Padding(20, 4, 0, 3);
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(100, 56);
            this.toolStripStatusLabel7.Text = "Sample Rate:";
            // 
            // lblSampleRate
            // 
            this.lblSampleRate.AutoSize = false;
            this.lblSampleRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblSampleRate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSampleRate.Name = "lblSampleRate";
            this.lblSampleRate.Size = new System.Drawing.Size(65, 58);
            this.lblSampleRate.Text = "Sample Rate";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(20, 4, 0, 3);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(66, 56);
            this.toolStripStatusLabel1.Text = "Volume:";
            // 
            // probarVolume
            // 
            this.probarVolume.AutoSize = false;
            this.probarVolume.Name = "probarVolume";
            this.probarVolume.Size = new System.Drawing.Size(100, 57);
            this.probarVolume.Step = 1;
            this.probarVolume.Value = 53;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel4.Margin = new System.Windows.Forms.Padding(20, 4, 0, 3);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(84, 56);
            this.toolStripStatusLabel4.Text = "Sensitivity:";
            // 
            // lblSensitivity
            // 
            this.lblSensitivity.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensitivity.Name = "lblSensitivity";
            this.lblSensitivity.Size = new System.Drawing.Size(31, 58);
            this.lblSensitivity.Text = "8.0";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.SeaGreen;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Audio Frames:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.SeaGreen;
            this.label2.Location = new System.Drawing.Point(0, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Video Frames:";
            // 
            // lblNumberOfAudioQueue
            // 
            this.lblNumberOfAudioQueue.AutoSize = true;
            this.lblNumberOfAudioQueue.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblNumberOfAudioQueue.Location = new System.Drawing.Point(73, 3);
            this.lblNumberOfAudioQueue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNumberOfAudioQueue.Name = "lblNumberOfAudioQueue";
            this.lblNumberOfAudioQueue.Size = new System.Drawing.Size(76, 13);
            this.lblNumberOfAudioQueue.TabIndex = 3;
            this.lblNumberOfAudioQueue.Text = "# AudioQueue";
            // 
            // lblNumberOfVideoQueue
            // 
            this.lblNumberOfVideoQueue.AutoSize = true;
            this.lblNumberOfVideoQueue.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblNumberOfVideoQueue.Location = new System.Drawing.Point(73, 16);
            this.lblNumberOfVideoQueue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNumberOfVideoQueue.Name = "lblNumberOfVideoQueue";
            this.lblNumberOfVideoQueue.Size = new System.Drawing.Size(76, 13);
            this.lblNumberOfVideoQueue.TabIndex = 4;
            this.lblNumberOfVideoQueue.Text = "# VideoQueue";
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 469);
            this.Controls.Add(this.lblNumberOfVideoQueue);
            this.Controls.Add(this.lblNumberOfAudioQueue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pictureBoxColor);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Viewer";
            this.Text = "Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Viewer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxColor;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel fpsProducedLabel;
        private System.Windows.Forms.ToolStripProgressBar probarVolume;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel fpsRenderedLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblSensitivity;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel lblSampleRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNumberOfAudioQueue;
        private System.Windows.Forms.Label lblNumberOfVideoQueue;
    }
}

