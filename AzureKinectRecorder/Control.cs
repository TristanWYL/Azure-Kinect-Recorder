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
        public Control()
        {
            InitializeComponent();
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

            //foreach (var device in devices) {
            //    cboxMircophone.Items.Add(device.FriendlyName);
            //}

            cboxMircophone.DataSource = devices;
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
            if ((!rbtnClose.Checked) && (!rbtnFar.Checked)) {
                MessageBox.Show("Please specify the field type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Field field = Field.Close;

            if (rbtnClose.Checked) {
                if (Globals.getInstance().dictIsFieldOpen[Field.Close]) {
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
            bool isCameraOpen = Device.TryOpen(out var camera, cboxCamera.SelectedIndex);
            if (!isCameraOpen) {
                MessageBox.Show("The selected camera may be being used by other program or the power is unplugged, so failed to open it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MMDevice mic = (MMDevice)cboxMircophone.SelectedItem;

            Viewer viewer = new Viewer(camera, mic, field);
            viewer.Show();

            //TODO: if succeed:
            IntegratedRecorder recorder = new IntegratedRecorder(camera, viewer.cameraConfig, field, mic);

            // link the viewer with the recorder 
            viewer.OnNewCapture += new NewCaptureEventHandler(recorder.NewCaptureArrive);
            // The below line will block the thread that works on DataAvailable
            recorder.AudioDataAvailable += new EventHandler<WaveInEventArgs>(viewer.OnAudioDataAvailable);
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

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!Globals.getInstance().isRecording)
            {
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
                foreach (var viewerRecorderPair in Globals.getInstance().viewerRecorderPairs)
                {
                    viewerRecorderPair.Value.StopRecord();
                }
                lblRecordingTime.ForeColor = Color.Gray;
                btnRecord.Text = "Record";
                btnPreview.Enabled = true;
            }
        }

        private void Control_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                if (isTuned) btnRecord.Enabled = true;
                else btnRecord.Enabled = false;

                if (Globals.getInstance().isRecording)
                    btnTune.Enabled = false;
                else btnTune.Enabled = true;

                if (Globals.getInstance().viewerRecorderPairs.Count == 2) {
                    btnPreview.Enabled = false;
                }
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
    }
}
