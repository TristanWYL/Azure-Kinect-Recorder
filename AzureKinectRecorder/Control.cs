using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K4AdotNet.Sensor;
using NAudio.CoreAudioApi;

namespace AzureKinectRecorder
{
    public partial class Control : Form
    {
        bool isCboxCameraSelected = false;
        bool isCboxMicrophoneSelected = false;
        DateTime dtStartRecordingTime;

        public Control()
        {
            InitializeComponent();
        }

        private void Control_Load(object sender, EventArgs e)
        {
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

        private void btnPreview_Click(object sender, EventArgs e)
        {
            // Device device = Device.Open(cboxCamera.SelectedIndex);
            bool isCameraOpen = Device.TryOpen(out var camera, cboxCamera.SelectedIndex);
            if (!isCameraOpen) {
                MessageBox.Show("The selected camera may be being used by other program or the power is unplugged, so failed to open it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MMDevice mic = (MMDevice)cboxMircophone.SelectedItem;

            Viewer viewer = new Viewer(camera, mic, (String)cboxCamera.SelectedItem);
            viewer.Show();

            //TODO: if succeed:
            IntegratedRecorder recorder = new IntegratedRecorder(camera, viewer.cameraConfig, $"Kinect{cboxCamera.SelectedIndex+1}", mic);

            // link the viewer with the recorder 
            viewer.OnNewCapture += new NewCaptureEventHandler(recorder.NewCaptureArrive);
            Globals.getInstance().viewerRecorderPairs[viewer] = recorder;
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
                    viewerRecorderPair.Value.InitializeRecorder(fullDirectory);
                    viewerRecorderPair.Value.StartRecord();
                }

                // UI management
                lblRecordingTime.ForeColor = Color.Red;
                btnRecord.Text = "Stop Recording";
                btnPreview.Enabled = false;
                Globals.getInstance().isRecording = true;
                dtStartRecordingTime = DateTime.Now;
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
            }
            else {
                btnRecord.Enabled = true;
            }
        }
    }
}
