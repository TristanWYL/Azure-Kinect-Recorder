using K4AdotNet;
using K4AdotNet.Record;
using K4AdotNet.Sensor;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureKinectRecorder
{
    public delegate void NewCaptureEventHandler(Capture capture);
    public partial class Viewer : Form
    {
        // To visualize images received from Capture
        // private readonly ImageVisualizer colorImageVisualizer;
        Device camera;
        MMDevice mic;
        DepthMode depthMode = DepthMode.Off;
        ColorResolution ColorResolution = ColorResolution.R2160p;
        FrameRate frameRate = FrameRate.Thirty;
        public DeviceConfiguration cameraConfig;
        public event NewCaptureEventHandler OnNewCapture;

        public Viewer(Device openedCamera, MMDevice mic, String cameraLabel)
        {
            InitializeComponent();
            this.Text = cameraLabel;
            camera = openedCamera;
            this.mic = mic;
            cameraConfig = new DeviceConfiguration
            {
                CameraFps = frameRate,
                ColorFormat = ImageFormat.ColorMjpg,
                ColorResolution = ColorResolution,
                DepthMode = depthMode,
                WiredSyncMode = WiredSyncMode.Standalone,
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            camera.StartCameras(cameraConfig);
            // mic.AudioEndpointVolume.Mute = false;
            timer1.Enabled = true;
            StartRender();
        }

        private async void StartRender()
        {
            Stopwatch sw = new Stopwatch();
            int frameCountProduced = 0;
            int frameCountDisplay = 0;
            bool isRendererIdle = true;
            sw.Start();
            while (!this.IsDisposed) {
                Capture curCapture = await Task.Run(() =>
                {
                    bool isSucceeded = false;
                    Capture capture = null;
                    try {
                        isSucceeded = camera.TryGetCapture(out capture, Timeout.FromSeconds(0.1)); 
                    } catch {
                        isSucceeded = false;
                        capture?.Dispose();
                    }
                     
                    if (isSucceeded)
                    {
                        if(OnNewCapture != null) { OnNewCapture(capture); }
                        frameCountProduced++;
                        return capture;
                    }
                    else {
                        return null;
                    }
                });
                if (curCapture != null) {
                    if (isRendererIdle)
                    {
                        isRendererIdle = false;
                        Capture curDisplayCapture = curCapture.DuplicateReference();
                        Task.Run(() =>
                        {
                            pictureBoxColor.Image = curDisplayCapture.ColorImage.CreateBitmap();
                            isRendererIdle = true;
                            curDisplayCapture.Dispose();
                            frameCountDisplay++;
                        });
                    }
                    else
                    {
                        curCapture.Dispose();
                    }
                }
                

                if (sw.Elapsed > TimeSpan.FromSeconds(2))
                {
                    double framesProducedPerSecond = (double)frameCountProduced / sw.Elapsed.TotalSeconds;
                    double framesRenderedPerSecond = (double)frameCountDisplay / sw.Elapsed.TotalSeconds;
                    this.fpsProducedLabel.Text = $"{framesProducedPerSecond:F2}";
                    this.fpsRenderedLabel.Text = $"{framesRenderedPerSecond:F2}";
                    frameCountProduced = 0;
                    frameCountDisplay = 0;
                    sw.Restart();
                }

                this.Update();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnNewCapture = null;
            camera.StopCameras();
            camera.Dispose();
            Globals.getInstance().viewerRecorderPairs[this].Dispose();
            Globals.getInstance().viewerRecorderPairs.Remove(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            probarVolume.Value = (int)Math.Round(mic.AudioMeterInformation.MasterPeakValue * 100);
            // Debug.WriteLine(mic.AudioMeterInformation.MasterPeakValue);
        }

        private void Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Globals.getInstance().isRecording)
            {
                MessageBox.Show("Please stop recording first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
    }
}
