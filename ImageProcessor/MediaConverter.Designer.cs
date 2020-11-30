using System;
using System.Windows.Forms;

namespace ImageProcessor
{
    partial class MediaConverter
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "HAHA"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaConverter));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckblistTypes = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvFiles = new ImageProcessor.CustomListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblNumOfFiles = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNumOfFilesSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuBtnStartConverting = new System.Windows.Forms.ToolStripMenuItem();
            this.amplifyCafBy12dBCxtMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.startConvertingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amplifyCafBy12dBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(708, 432);
            this.splitContainer1.SplitterDistance = 136;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckblistTypes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 432);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Types";
            // 
            // ckblistTypes
            // 
            this.ckblistTypes.CheckOnClick = true;
            this.ckblistTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckblistTypes.FormattingEnabled = true;
            this.ckblistTypes.HorizontalScrollbar = true;
            this.ckblistTypes.Location = new System.Drawing.Point(3, 19);
            this.ckblistTypes.Name = "ckblistTypes";
            this.ckblistTypes.Size = new System.Drawing.Size(130, 410);
            this.ckblistTypes.TabIndex = 0;
            this.ckblistTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ckblistTypes_ItemCheck);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvFiles);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(568, 432);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Files to Be Converted";
            // 
            // lvFiles
            // 
            this.lvFiles.CheckBoxes = true;
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvFiles.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            this.lvFiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvFiles.Location = new System.Drawing.Point(3, 19);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.OwnerDraw = true;
            this.lvFiles.ShowItemToolTips = true;
            this.lvFiles.Size = new System.Drawing.Size(562, 410);
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvFiles_DrawItem);
            this.lvFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvFiles_ItemCheck);
            this.lvFiles.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvFiles_ItemChecked);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblNumOfFiles,
            this.lblNumOfFilesSelected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 456);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(708, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblNumOfFiles
            // 
            this.lblNumOfFiles.Name = "lblNumOfFiles";
            this.lblNumOfFiles.Size = new System.Drawing.Size(39, 17);
            this.lblNumOfFiles.Text = "0 Files";
            // 
            // lblNumOfFilesSelected
            // 
            this.lblNumOfFilesSelected.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.lblNumOfFilesSelected.Name = "lblNumOfFilesSelected";
            this.lblNumOfFilesSelected.Size = new System.Drawing.Size(86, 17);
            this.lblNumOfFilesSelected.Text = "0 Files Selected";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.ctxMenuBtnStartConverting,
            this.amplifyCafBy12dBCxtMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(184, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // ctxMenuBtnStartConverting
            // 
            this.ctxMenuBtnStartConverting.Name = "ctxMenuBtnStartConverting";
            this.ctxMenuBtnStartConverting.Size = new System.Drawing.Size(183, 22);
            this.ctxMenuBtnStartConverting.Text = "Start Converting";
            this.ctxMenuBtnStartConverting.Click += new System.EventHandler(this.ctxMenuBtnStartConverting_Click);
            // 
            // amplifyCafBy12dBCxtMenuItem
            // 
            this.amplifyCafBy12dBCxtMenuItem.CheckOnClick = true;
            this.amplifyCafBy12dBCxtMenuItem.Name = "amplifyCafBy12dBCxtMenuItem";
            this.amplifyCafBy12dBCxtMenuItem.Size = new System.Drawing.Size(183, 22);
            this.amplifyCafBy12dBCxtMenuItem.Text = "Amplify .caf by 12dB";
            this.amplifyCafBy12dBCxtMenuItem.Click += new System.EventHandler(this.amplifyForcafBy12dBToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(708, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem2,
            this.startConvertingToolStripMenuItem,
            this.amplifyCafBy12dBToolStripMenuItem});
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(72, 20);
            this.openToolStripMenuItem1.Text = "Operation";
            this.openToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.openToolStripMenuItem1_DropDownOpening);
            // 
            // openToolStripMenuItem2
            // 
            this.openToolStripMenuItem2.Name = "openToolStripMenuItem2";
            this.openToolStripMenuItem2.Size = new System.Drawing.Size(201, 22);
            this.openToolStripMenuItem2.Text = "Open...";
            this.openToolStripMenuItem2.Click += new System.EventHandler(this.openToolStripMenuItem2_Click);
            // 
            // startConvertingToolStripMenuItem
            // 
            this.startConvertingToolStripMenuItem.Name = "startConvertingToolStripMenuItem";
            this.startConvertingToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.startConvertingToolStripMenuItem.Text = "Start Converting";
            this.startConvertingToolStripMenuItem.Click += new System.EventHandler(this.startConvertingToolStripMenuItem_Click);
            // 
            // amplifyCafBy12dBToolStripMenuItem
            // 
            this.amplifyCafBy12dBToolStripMenuItem.CheckOnClick = true;
            this.amplifyCafBy12dBToolStripMenuItem.Name = "amplifyCafBy12dBToolStripMenuItem";
            this.amplifyCafBy12dBToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.amplifyCafBy12dBToolStripMenuItem.Text = "Amplify for .caf by 12dB";
            this.amplifyCafBy12dBToolStripMenuItem.Click += new System.EventHandler(this.settingToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 432);
            this.panel1.TabIndex = 3;
            // 
            // MediaConverter
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 478);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MediaConverter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Convertor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MediaConverter_FormClosing);
            this.Load += new System.EventHandler(this.MediaConverter_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MediaConverter_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MediaConverter_DragEnter);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox ckblistTypes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblNumOfFiles;
        private System.Windows.Forms.ToolStripStatusLabel lblNumOfFilesSelected;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuBtnStartConverting;
        private CustomListView lvFiles;
        private ColumnHeader columnHeader1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem openToolStripMenuItem1;
        private ToolStripMenuItem openToolStripMenuItem2;
        private ToolStripMenuItem startConvertingToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem amplifyCafBy12dBToolStripMenuItem;
        private ToolStripMenuItem amplifyCafBy12dBCxtMenuItem;
        private Panel panel1;
    }
}

