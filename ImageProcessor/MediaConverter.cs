using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xabe.FFmpeg;

namespace ImageProcessor
{
    public partial class MediaConverter : Form
    {
        List<String> fileExtAccepted = new List<string>();
        List<String> files = new List<string>();

        public MediaConverter()
        {
            InitializeComponent();

            fileExtAccepted.Add("mkv");
            fileExtAccepted.Add("m4a");
            fileExtAccepted.Add("caf");
            foreach (var ext in fileExtAccepted) {
                ckblistTypes.Items.Add(ext);
            }
            
            lvFiles.Items.Clear();
            lvFiles.Columns[0].Width = lvFiles.Width;

            //Set directory where app should look for FFmpeg 
            //FFmpeg.SetExecutablesPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        private void MediaConverter_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MediaConverter_DragDrop(object sender, DragEventArgs e)
        {
            lvFiles.Reset();
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            files = GetMatchedFileList(s);
            for (int i = 0; i < ckblistTypes.Items.Count; i++)
            {
                ckblistTypes.SetItemChecked(i, true);
            }
            foreach ( var file in files) {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems[0].Text = file;
                lvFiles.Items.Add(lvi).Checked = true;
            }
            lblNumOfFiles.Text = $"{files.Count} Files";
        }

        private List<String> GetMatchedFileList(String[] paths) {
            var chosenFiles = new List<String>();
            int i;
            for (i = 0; i < paths.Length; i++)
            {
                bool isFolder = File.GetAttributes(paths[i]).HasFlag(FileAttributes.Directory);
                //FileAttributes attr = File.GetAttributes(s[i]);
                //bool isFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;
                if (isFolder)
                {
                    foreach (var ext in fileExtAccepted)
                    {
                        chosenFiles.AddRange(Directory.GetFiles(paths[i], $"*.{ext}", SearchOption.AllDirectories));
                    }
                }
                else {
                    FileInfo info = new FileInfo(paths[i]);
                    if (IsExtMatched(info.Extension)) {
                        chosenFiles.Add(paths[i]);
                    }
                }
            }
            return chosenFiles;
        }

        private bool IsExtMatched(String ext) {
            String extNormalized = ext.Remove(0, 1);
            if (fileExtAccepted.IndexOf(extNormalized) >= 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
        private void lvFiles_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            lblNumOfFilesSelected.Text = $"{GetNumberOfSelectedFile()} Files Selected";
        }

        private void ckblistTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckState newState = e.NewValue;
            String ext = (String)ckblistTypes.Items[e.Index];
            if (newState == CheckState.Checked)
            {
                for(int i = 0; i < lvFiles.Items.Count; i++)
                {
                    if (lvFiles.Items[i].Text.EndsWith(ext))
                    {
                        lvFiles.Items[i].Checked = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < lvFiles.Items.Count; i++)
                {
                    if (lvFiles.Items[i].Text.EndsWith(ext))
                    {
                        lvFiles.Items[i].Checked = false;
                    }
                }
            }
        }

        private int GetNumberOfSelectedFile() {
            int num = 0;
            for (int i = 0; i < lvFiles.Items.Count; i++)
            {                
                if (lvFiles.Items[i].Checked) {
                    num++;
                }
            }
            return num;
        }

        private Dictionary<int, String> GetCheckedFile() {
            Dictionary<int, String> checkedFile = new Dictionary<int, String>();
            for (int i = 0; i < lvFiles.Items.Count; i++)
            {
                if (lvFiles.Items[i].Checked)
                {
                    checkedFile[i] = lvFiles.Items[i].Text; 
                }
            }
            return checkedFile;
        }

        private void ckblistFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int curNumberOfSelectedFile = GetNumberOfSelectedFile();
            if (e.NewValue == CheckState.Checked) {
                curNumberOfSelectedFile++;
            }
            else {
                curNumberOfSelectedFile--;
            }
            lblNumOfFilesSelected.Text = $"{curNumberOfSelectedFile} Files Selected";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    lvFiles.Reset();
                    string[] paths = new string[] { fbd.SelectedPath };
                    files = GetMatchedFileList(paths);
                    for (int i = 0; i < ckblistTypes.Items.Count; i++)
                    {
                        ckblistTypes.SetItemChecked(i, true);
                    }
                    foreach (var file in files) {
                        ListViewItem lvi = new ListViewItem();
                        lvi.SubItems[0].Text = file;
                        lvFiles.Items.Add(lvi).Checked = true;
                    }
                    lblNumOfFiles.Text = $"{files.Count} Files";
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ctxMenuBtnStartConverting.Enabled = GetNumberOfSelectedFile() > 0;
        }

        private bool isConverting = false;
        private bool IsConverting {
            get {
                return isConverting;
            }
            set {
                isConverting = value;
                if (isConverting)
                {
                    this.DragDrop -= MediaConverter_DragDrop;
                    contextMenuStrip1.Enabled = false;
                    ckblistTypes.Enabled = false;
                }
                else {
                    this.DragDrop += MediaConverter_DragDrop;
                    contextMenuStrip1.Enabled = true;                    
                    ckblistTypes.Enabled = true;
                }
            }
        }

        private async void ctxMenuBtnStartConverting_Click(object sender, EventArgs e)
        {
            IsConverting = true;
            var dictCheckedFiles = GetCheckedFile();
            await Task.Run(() => {
                Parallel.ForEach(dictCheckedFiles, (file) => {
                    string argument = "";
                    string outputFileName = "";
                    if (file.Value.EndsWith("mkv"))
                    {
                        //Save file to the same location with changed extension
                        outputFileName = Path.ChangeExtension(file.Value, ".mp4");
                        // Refer to https://ffmpeg.xabe.net/docs.html
                        argument = $"-i {file.Value} -c:v libx264 -preset ultrafast -crf 18 {outputFileName}";
                    }
                    else if (file.Value.EndsWith("m4a"))
                    {
                        outputFileName = file.Value + ".wav";
                        // Refer to https://ffmpeg.xabe.net/docs.html
                        argument = $"-i {file.Value} {outputFileName}";
                    }
                    else if (file.Value.EndsWith("caf")) {
                        outputFileName = file.Value + ".wav";
                        // Refer to https://ffmpeg.xabe.net/docs.html
                        argument = $"-i {file.Value} {outputFileName}";
                    }
                    else { return; }
                    if (File.Exists(outputFileName)) {
                        Conversion_Error(file.Key, "The converted file has already been existing in the target folder!");
                        return;
                    };
                    var conversion = FFmpeg.Conversions.New();
                    conversion.SetOverwriteOutput(false);
                    conversion.OnProgress += new FFmpegConversionProgress(this, lvFiles, file.Key).Conversion_OnProgress;
                    try { conversion.Start(argument).GetAwaiter().GetResult(); }
                    catch (Exception exception)
                    {
                        string msg = exception.Message.Substring(exception.Message.LastIndexOf('[') + 1);
                        Conversion_Error(file.Key, msg);
                    }
                });
            });
            MessageBox.Show("The conversion has finished!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            IsConverting = false;
        }

        

        private void Conversion_Error(int ind, string errorMsg)
        {
            this.Invoke(new MethodInvoker(delegate () {
                lvFiles.UpdateError(ind, errorMsg);
            }));
            // var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
            // Debug.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            lvFiles.Columns[0].Width = lvFiles.Width;
        }

        private void lvFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                var where = lvFiles.HitTest(e.Location);
                if (where.Location == ListViewHitTestLocations.Label)
                {
                    where.Item.Checked = !where.Item.Checked;
                }
            }
        }

        private void lvFiles_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // e.DrawDefault = true;
        }

        private void lvFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (IsConverting) {
                e.NewValue = e.CurrentValue;
            }
        }

        private void MediaConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConverting) { 
                e.Cancel = true;
                MessageBox.Show("The conversion is going on, so please close the window after it is done!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    public class FFmpegConversionProgress {
        CustomListView lv;
        int indOfItem;
        Control host;
        public FFmpegConversionProgress(Control parent, CustomListView clv, int indexOfItem) {
            lv = clv;
            indOfItem = indexOfItem;
            host = parent;
        }
        public void Conversion_OnProgress(object sender, Xabe.FFmpeg.Events.ConversionProgressEventArgs args)
        {
            host.Invoke(new MethodInvoker(delegate () {
                lv.UpdateProgress(indOfItem, args.Percent);
            }));

            // var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
            // Debug.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
        }
    }

    public class CustomCheckedListBox : CheckedListBox
    {
        public CustomCheckedListBox()
        {
            DoubleBuffered = true;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Size checkSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
            int dx = (e.Bounds.Height - checkSize.Width) / 2;
            e.DrawBackground();
            bool isChecked = GetItemChecked(e.Index);//For some reason e.State doesn't work so we have to do this instead.
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(dx, e.Bounds.Top + dx), isChecked ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal : System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                using (Brush brush = new SolidBrush(isChecked ? BackgroundItemColor : ForeColor))
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, new Rectangle(e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height), sf);
                }
            }
        }
        Color bgItemColor = Color.Green;
        public Color BackgroundItemColor
        {
            get { return bgItemColor; }
            set
            {
                bgItemColor = value;
                Invalidate();
            }
        }
    }

    enum ConvertState {
        BeforeStart,
        Progress,
        Error,
        Success
    }

    public class CustomListView : ListView {
        Color ColorSuccess = Color.DarkGreen;
        Color ColorError = Color.Red;
        Color ColorProgress = Color.LightGreen;
        int progressValue = 0; // 0~100
        Dictionary<int, int> DictItemProgress = new Dictionary<int, int>();
        Dictionary<int, ConvertState> DictItemConvertState = new Dictionary<int, ConvertState>();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void UpdateProgress(int indItem, int value) {
            if (value > 100) value = 100;
            DictItemProgress[indItem] = value;
            DictItemConvertState[indItem] = ConvertState.Progress;
            this.RedrawItems(indItem, indItem, true);
        }

        public void UpdateError(int indItem, String errorMsg) {
            DictItemConvertState[indItem] = ConvertState.Error;
            this.RedrawItems(indItem, indItem, true);
            this.Items[indItem].ToolTipText = errorMsg;
        }

        public void UpdateSuccess(int indItem) {
            DictItemConvertState[indItem] = ConvertState.Success;
            this.RedrawItems(indItem, indItem, true);
        }

        public void Reset() {
            Enabled = true;
            Items.Clear();
            DictItemProgress.Clear();
            DictItemConvertState.Clear();
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            base.OnDrawSubItem(e);
            if (e.ColumnIndex == 0)
            {
                e.DrawDefault = false;

                //e.Graphics.DrawRectangle(new Pen(Color.Black,1), e.Bounds);

                int widthOfCheckBox = e.Bounds.Height;
                CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.X, e.Bounds.Y), e.Item.Checked ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal : System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                if (DictItemConvertState.ContainsKey(e.ItemIndex)) {
                    switch (DictItemConvertState[e.ItemIndex]) {
                        case ConvertState.Progress:
                            e.Graphics.FillRectangle(new SolidBrush(ColorProgress), widthOfCheckBox + e.Bounds.X + 1, e.Bounds.Y + 1, DictItemProgress[e.ItemIndex]* (e.Bounds.Width - widthOfCheckBox) / 100, e.Bounds.Height - 1);
                            break;
                        case ConvertState.Error:
                            e.Graphics.FillRectangle(new SolidBrush(ColorError), widthOfCheckBox + e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 1, e.Bounds.Height - 1);
                            break;
                        case ConvertState.Success:
                            e.Graphics.FillRectangle(new SolidBrush(ColorSuccess), widthOfCheckBox + e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 1, e.Bounds.Height - 1);
                            break;
                        default:
                            break;
                    }
                }

                Font font = new Font(e.Item.SubItems[0].Font, e.Item.Checked ? FontStyle.Bold : FontStyle.Regular);
                e.Graphics.DrawString(e.Item.SubItems[0].Text, font, new SolidBrush(Color.Black), e.Bounds.X + widthOfCheckBox + 1, e.Bounds.Y);
            }
            else {
                e.DrawDefault = true;
            }
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e) {
            base.OnDrawItem(e);
        }
    }
}
