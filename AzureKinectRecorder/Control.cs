using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K4AdotNet.Sensor;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureKinectRecorder
{
    public partial class Control : Form
    {
        bool isCboxCameraSelected = false;
        bool isCboxMicrophoneSelected = false;
        DateTime dtStartRecordingTime;
        JArray jsonTestSite;
        String siteID = "";
        bool isTuned = false;
        private int HzDriveSpaceCheck = 1;
        private int TickNumInSpaceCheckPeriod;
        public Control()
        {
            InitializeComponent();
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS| EXECUTION_STATE.ES_SYSTEM_REQUIRED|EXECUTION_STATE.ES_DISPLAY_REQUIRED);
        }

        // Prevent the minotor from shutting down
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        private void Control_Load(object sender, EventArgs e)
        {
            LoadJson();
            foreach (var site in jsonTestSite) {
                cboxSite.Items.Add(site);
            }
            cboxSite.ValueMember = "id";
            cboxSite.DisplayMember = "desc";


            int number = Device.InstalledCount;
            // initialize cboxCamera
            for(int ind = 0; ind<number; ind++) { cboxCamera.Items.Add($"Azure Kinect {ind+1}"); }

            if (Environment.OSVersion.Version.Major >= 6)
            {
                LoadWasapiDevicesCombo();
            }
            else {
                throw new Exception("Too old operating system, please update!");
            }

            timer1.Enabled = true;
            TickNumInSpaceCheckPeriod = HzDriveSpaceCheck * 1000 / timer1.Interval;
        }

        private void LoadJson() {
            //using (StreamReader file = File.OpenText(@"site.json")) {
            //    using (JsonTextReader reader = new JsonTextReader(file))
            //    {
            //        jsonTestSite = (JObject)JToken.ReadFrom(reader);
            //    }
            //}
            using (StreamReader sr = File.OpenText(@"site.json"))
            {
                jsonTestSite = (JArray)JsonConvert.DeserializeObject(sr.ReadToEnd());
            }
        }

        private void LoadWasapiDevicesCombo()
        {
            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            List<MMDevice> microphones = new List<MMDevice>();
            foreach (var device in devices)
            {
                if (device.FriendlyName.ToLower().Contains("kinect")) {
                    microphones.Add(device);
                }
            }

            cboxMircophone.DataSource = microphones;
            cboxMircophone.DisplayMember = "FriendlyName";
            cboxMircophone.SelectedIndex = -1;
            isCboxMicrophoneSelected = false;
        }

        private void cboxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            isCboxCameraSelected = true;
            if (isCboxCameraSelected && isCboxMicrophoneSelected)
            {
                btnPreview.Enabled = true;
            }
            else {
                btnPreview.Enabled = false;
            }
        }

        private void cboxSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            siteID = ((JObject)cboxSite.SelectedItem).ToObject<Dictionary<dynamic, dynamic>>()["id"];
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (Device.InstalledCount > 2 && Globals.getInstance().viewerRecorderPairs.Count == 1) {
                MessageBox.Show("Cannot open next camera automatically as more than two cameras are being connected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Globals.getInstance().viewerRecorderPairs.Count == 2)
            {
                MessageBox.Show("Can only open 2 cameras at most.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((!rbtnClose.Checked) && (!rbtnFar.Checked)) {
                MessageBox.Show("Please specify the field type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Field field = Field.Close;
            Device camera;
            MMDevice microphone;

            if (Globals.getInstance().viewerRecorderPairs.Count == 1)
            {
                if (Device.InstalledCount == 1) {
                    MessageBox.Show("The only one installed camera has been opened.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Globals.getInstance().dictIsFieldOpen[Field.Close])
                {
                    field = Field.Far;
                }
                else
                {
                    field = Field.Close;
                }

                int cameraIndex = 0;
                if (Globals.getInstance().viewerRecorderPairs.First().Value.camera.DeviceIndex == 0) {
                    cameraIndex = 1;
                }
                bool isCameraOpen = Device.TryOpen(out var cam, cameraIndex);
                if (!isCameraOpen)
                {
                    MessageBox.Show("The second camera may be being used by other program or the power is unplugged, so failed to open it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                camera = cam;

                if (Globals.getInstance().viewerRecorderPairs.First().Key.mic == ((List<MMDevice>)(cboxMircophone.DataSource))[0])
                {
                    microphone = ((List<MMDevice>)(cboxMircophone.DataSource))[1];
                }
                else {
                    microphone = ((List<MMDevice>)(cboxMircophone.DataSource))[0];
                }
            }
            else {
                if (rbtnClose.Checked)
                {
                    if (Globals.getInstance().dictIsFieldOpen[Field.Close])
                    {
                        MessageBox.Show("The \"Close\" field has been opened. Please change the \"Field\" setting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    field = Field.Close;
                }
                if (rbtnFar.Checked)
                {
                    if (Globals.getInstance().dictIsFieldOpen[Field.Far])
                    {
                        MessageBox.Show("The \"Far\" field has been opened. Please change the \"Field\" setting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    field = Field.Far;
                }

                // Device device = Device.Open(cboxCamera.SelectedIndex);
                bool isCameraOpen = Device.TryOpen(out var cam, cboxCamera.SelectedIndex);
                if (!isCameraOpen)
                {
                    MessageBox.Show("The selected camera may be being used by other program or the power is unplugged, so failed to open it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                microphone = (MMDevice)cboxMircophone.SelectedItem;
                camera = cam;
            }

            Viewer viewer = new Viewer(microphone, field);
            viewer.Show();

            //TODO: if succeed:
            IntegratedRecorder recorder = new IntegratedRecorder(camera, field, microphone);

            // link the viewer with the recorder 
            // The below line will block the thread that works on DataAvailable
            recorder.AudioDataAvailable += new EventHandler<WaveInEventArgs>(viewer.OnAudioDataAvailable);
            recorder.VideoDataAvailable += new EventHandler<System.Drawing.Image>(viewer.OnFrameDataAvailable);
            Globals.getInstance().viewerRecorderPairs[viewer] = recorder;
            Globals.getInstance().dictIsFieldOpen[field] = true;
            viewer.UpdateSensitivityLabel();
            isTuned = false;
        }

        private void cboxMircophone_SelectedIndexChanged(object sender, EventArgs e)
        {
            isCboxMicrophoneSelected = true;
            if (isCboxCameraSelected && isCboxMicrophoneSelected)
            {
                btnPreview.Enabled = true;
            }
            else
            {
                btnPreview.Enabled = false;
            }
        }

        private async void btnRecord_Click(object sender, EventArgs e)
        {
            if (!Globals.getInstance().isRecording)
            {
                DriveSpaceManager.GetInstance().NumOfkinect = Globals.getInstance().viewerRecorderPairs.Count;
                if (DriveSpaceManager.GetInstance().ShouldStopRecording) {
                    MessageBox.Show("Free disk space is too small. Please clean it first and then start recording again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DriveSpaceManager.GetInstance().ShouldWarnWhenRecording) {
                    var result = MessageBox.Show("Free disk space is nearly used up. Are you sure you want to start recording now?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No) { return; }
                }else if (DriveSpaceManager.GetInstance().ShouldWarnBeforeRecording)
                {
                    var result = MessageBox.Show($"You can only record for around {DriveSpaceManager.GetInstance().GetMinutesByAvailabeSpace()} minutes due to limited free space in the disk drive. Are you sure you want to start recording now?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No) { return; }
                }

               if (siteID.Length < 1) {
                    MessageBox.Show("Please specify the testing site before recording.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // prepare the file-saving directory
                String folderName = DateTime.Now.ToString("yyyyMMddHHmmss");
                String fullDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Kinect",folderName);
                // create the folder
                try {
                    Directory.CreateDirectory(fullDirectory);
                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // initialize the recorder
                foreach (var viewerRecorderPair in Globals.getInstance().viewerRecorderPairs) {
                    viewerRecorderPair.Value.InitializeRecorder(fullDirectory, siteID);
                    viewerRecorderPair.Value.StartRecord();
                }

                // UI management
                lblRecordingTime.ForeColor = Color.Red;
                btnRecord.Text = "Stop Recording";
                btnPreview.Enabled = false;
                Globals.getInstance().isRecording = true;
                dtStartRecordingTime = DateTime.Now;

                TonePlayer.GetInstance().PlayBeep(2000, 500, WaveFormType.SquareWave, 32000);
            }
            else {
                Globals.getInstance().isRecording = false;
                List<Task> tasks = new List<Task>();
                foreach (var viewerRecorderPair in Globals.getInstance().viewerRecorderPairs)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        viewerRecorderPair.Value.StopRecord();
                    }));
                }
                Globals.getInstance().hasStoppedRecordingButFlushing = true;
                await Task.WhenAll(tasks);
                Globals.getInstance().hasStoppedRecordingButFlushing = false;
                lblRecordingTime.ForeColor = Color.Gray;
                btnRecord.Text = "Record";
                btnPreview.Enabled = true;
            }
        }

        private void Control_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Globals.getInstance().hasStoppedRecordingButFlushing) {
                MessageBox.Show("It is flushing data into the SSD, please try again after it is done. You can check out the number of remaining frames waiting to be written into the disk at the upper-left corner of the video-displaying windows.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (Globals.getInstance().isRecording)
            {
                MessageBox.Show("Please stop recording first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                List<Viewer> viewers = Globals.getInstance().viewerRecorderPairs.Keys.ToList<Viewer>();
                foreach (var viewer in viewers) {
                    viewer.Close();
                }
            }
        }


        private int TickCountForSpaceCheck = 0;
        private bool HasPromptedLimitedFreeDisk = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Globals.getInstance().isRecording)
            {
                // update the recording time label
                var span = DateTime.Now - dtStartRecordingTime;
                lblRecordingTime.Text = span.ToString(@"hh\:mm\:ss");
            }
            else {
                lblRecordingTime.Text = "00:00:00";
            }

            if (Globals.getInstance().viewerRecorderPairs.Count == 0)
            {
                btnRecord.Enabled = false;
                btnTune.Enabled = false;
            }
            else {
                if (Globals.getInstance().hasStoppedRecordingButFlushing)
                {
                    btnRecord.Enabled = false;
                    btnTune.Enabled = false;
                }
                else {
                    if (isTuned) btnRecord.Enabled = true;
                    else btnRecord.Enabled = false;
                    if (Globals.getInstance().isRecording)
                    {
                        btnTune.Enabled = false;
                    }
                    else {
                        btnTune.Enabled = true;
                    }
                }
            }

            if (Globals.getInstance().isRecording)
            {
                lblRemainingDiskTime.Text = $"{ DriveSpaceManager.GetInstance().GetMinutesByAvailabeSpace() }";
                TickCountForSpaceCheck++;
                if (TickCountForSpaceCheck > TickNumInSpaceCheckPeriod)
                {
                    TickCountForSpaceCheck = 0;
                    if (DriveSpaceManager.GetInstance().ShouldStopRecording)
                    {
                        btnRecord.PerformClick();
                        MessageBox.Show("Free disk space is too small. The system has to stop recording automatically now. Please clean the disk first and then start recording again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (DriveSpaceManager.GetInstance().ShouldWarnWhenRecording && !HasPromptedLimitedFreeDisk)
                    {
                        HasPromptedLimitedFreeDisk = true; 
                        MessageBox.Show("Free disk space is nearly used up. You may need to stop recording shortly?", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else {
                HasPromptedLimitedFreeDisk = false;
            }
        }

        private void btnTune_Click(object sender, EventArgs e)
        {
            Globals.getInstance().tunerProcesses = new List<TunerProcess>();
            List<Viewer> viewers = Globals.getInstance().viewerRecorderPairs.Keys.ToList<Viewer>();
            foreach (var viewer in viewers)
            {
                var waveFormat = Globals.getInstance().viewerRecorderPairs[viewer].audioCaptureDevice.WaveFormat;
                TunerProcess tp = new TunerProcess(viewer.mic, waveFormat.BitsPerSample / 8);
                Globals.getInstance().tunerProcesses.Add(tp);
                Globals.getInstance().viewerRecorderPairs[viewer].AudioDataAvailable += tp.OnAudioDataAvailable;
            }
            Tuner tuner = new Tuner();
            var dr = tuner.ShowDialog();
            if (dr == DialogResult.OK)
            {
                isTuned = true;
            }
            else {
                isTuned = false;
                foreach (var tp in Globals.getInstance().tunerProcesses) {
                    tp.RestoreVolumeLevel();
                }
            }

            //Debug.WriteLine("Tuning Ended");
            for(int i= 0; i < Globals.getInstance().viewerRecorderPairs.Count; i++){
                Globals.getInstance().viewerRecorderPairs.ElementAt(i).Value.AudioDataAvailable -= Globals.getInstance().tunerProcesses[i].OnAudioDataAvailable;
                Globals.getInstance().viewerRecorderPairs.ElementAt(i).Key.UpdateSensitivityLabel();
            }
            Globals.getInstance().tunerProcesses = null;
        }

        private void Control_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}
