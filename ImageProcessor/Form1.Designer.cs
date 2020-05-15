namespace ImageProcessor
{
    partial class Form1
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
            this.gboxResize = new System.Windows.Forms.GroupBox();
            this.btnResize = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gboxResize.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboxResize
            // 
            this.gboxResize.Controls.Add(this.btnResize);
            this.gboxResize.Controls.Add(this.label1);
            this.gboxResize.Location = new System.Drawing.Point(8, 8);
            this.gboxResize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gboxResize.Name = "gboxResize";
            this.gboxResize.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gboxResize.Size = new System.Drawing.Size(200, 99);
            this.gboxResize.TabIndex = 0;
            this.gboxResize.TabStop = false;
            this.gboxResize.Text = "Resize";
            // 
            // btnResize
            // 
            this.btnResize.Location = new System.Drawing.Point(81, 68);
            this.btnResize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(103, 27);
            this.btnResize.TabIndex = 1;
            this.btnResize.Text = "Start Resizing";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 153);
            this.Controls.Add(this.gboxResize);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gboxResize.ResumeLayout(false);
            this.gboxResize.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboxResize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnResize;
    }
}

