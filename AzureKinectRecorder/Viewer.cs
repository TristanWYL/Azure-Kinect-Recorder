using K4AdotNet;
using K4AdotNet.Record;
using K4AdotNet.Sensor;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureKinectRecorder
{
    public delegate void NewCaptureEventHandler(Capture capture);
    public partial class Viewer : Form
    {
        // To visualize images received from Capture
        // private readonly ImageVisualizer colorImageVisualizer;
        public MMDevice mic;
        private Field field;
        private int volumeAmplification = 97; // From 0 to 100. 100 means most amplification
        private double fpsProduced = 0;
        private double fpsRender = 0;
        private int frameCountRenderedTemp = 0;
        Stopwatch stopWatchCountFrameRendered = new Stopwatch();

        public Viewer(MMDevice mic, Field field)
        {
            InitializeComponent();
            ////Add a trackbar in toolstrip
            //ToolStripTraceBarItem tbSensitivity = new ToolStripTraceBarItem();
            ////tbSensitivity.bar.Location = new System.Drawing.Point(78, 317);
            //tbSensitivity.bar.Maximum = 100;
            //tbSensitivity.bar.Name = "trackBar1";
            //tbSensitivity.bar.Size = new System.Drawing.Size(183, 69);
            //tbSensitivity.bar.TabIndex = 5;
            //tbSensitivity.bar.Value = volumeAmplification;
            //tbSensitivity.bar.TickStyle = System.Windows.Forms.TickStyle.None;
            //tbSensitivity.bar.ValueChanged += TractBarSensitivity_ValueChanged;
            //statusStrip1.Items.Add(tbSensitivity);

            this.field = field;
            this.Text = field.ToString();
            this.mic = mic;
            //SetVolumeLevel(Convert.ToSingle(volumeAmplification / 100.0));

            stopWatchCountFrameRendered.Start();
        }

        private void TractBarSensitivity_ValueChanged(object sender, EventArgs e)
        {
            //SetVolumeLevel(Convert.ToSingle((sender as System.Windows.Forms.TrackBar).Value / 100.0));
        }

        private void SetVolumeLevel(float value) {
            this.mic.AudioSessionManager.AudioSessionControl.SimpleAudioVolume.Volume = value;
        }

        public void UpdateSensitivityLabel() {
            lblSensitivity.Text = this.mic.AudioSessionManager.AudioSessionControl.SimpleAudioVolume.Volume.ToString("0.00");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // mic.AudioEndpointVolume.Mute = false;
            timer1.Enabled = true;
        }

        public void OnFrameDataAvailable(object sender, System.Drawing.Image image) {
            frameCountRenderedTemp++;
            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                fpsProduced = ((IntegratedRecorder)sender).fpsProduced;
                pictureBoxColor.Image = image;
                this.Update();
            }));
            if (stopWatchCountFrameRendered.Elapsed > TimeSpan.FromSeconds(2))
            {
                fpsRender = (double)frameCountRenderedTemp / stopWatchCountFrameRendered.Elapsed.TotalSeconds;
                frameCountRenderedTemp = 0;
                stopWatchCountFrameRendered.Restart();
            }
        }

        public void OnAudioDataAvailable(object sender, WaveInEventArgs e) {
            float max = 0;
            var buffer = new WaveBuffer(e.Buffer);
            int bytesPerSample = ((NAudio.CoreAudioApi.WasapiCapture)sender).WaveFormat.BitsPerSample / 8;
            // interpret as 32 bit floating point audio
            for (int index = 0; index < e.BytesRecorded/ bytesPerSample; index++)
            {
                var sample = buffer.FloatBuffer[index];
                //float sample;
                //if (BitConverter.IsLittleEndian)
                //{
                //    sample = BitConverter.ToSingle(e.Buffer, index * bytesPerSample);
                //}
                //else {
                //    byte[] temp = new byte[bytesPerSample];
                //    if (bytesPerSample == 4)
                //    {
                //        // convert the endienness
                //        temp[0] = e.Buffer[index * bytesPerSample + 3];
                //        temp[1] = e.Buffer[index * bytesPerSample + 2];
                //        temp[2] = e.Buffer[index * bytesPerSample + 1];
                //        temp[3] = e.Buffer[index * bytesPerSample];
                //    }
                //    else {
                //        throw new NotImplementedException();
                //    }
                //    sample = BitConverter.ToSingle(temp, 0);
                //}
                // absolute value 
                if (sample < 0) sample = -sample;
                // is this the max value?
                if (sample > max) max = sample;
            }
            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                //lbTest.Text = $"{max}";
                if (max > 1) max = 1;
                probarVolume.Value = (int)(100 * max);
                lblSampleRate.Text = ((IntegratedRecorder)sender).audioSampleRate.ToString();
            }));
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Globals.getInstance().viewerRecorderPairs[this].audioCaptureDevice.DataAvailable -= OnAudioDataAvailable;
            timer1.Enabled = false;
            timer1.Dispose();
            Globals.getInstance().viewerRecorderPairs[this].Dispose();
            Globals.getInstance().viewerRecorderPairs.Remove(this);
            Globals.getInstance().dictIsFieldOpen[this.field] = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // probarVolume.Value = (int)Math.Round(mic.AudioMeterInformation.MasterPeakValue * 100);
            // Debug.WriteLine(mic.AudioMeterInformation.MasterPeakValue);
            fpsProducedLabel.Text = $"{fpsProduced:F2}";
            fpsRenderedLabel.Text = $"{fpsRender:F2}";
            lblNumberOfAudioQueue.Text = Globals.getInstance().viewerRecorderPairs[this].qAudioBufferToRecord?.Count.ToString();
            lblNumberOfVideoQueue.Text = Globals.getInstance().viewerRecorderPairs[this].qVideoBufferToRecord?.Count.ToString();
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
