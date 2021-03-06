﻿using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureKinectRecorder
{
    public partial class Tuner : Form
    {
        int num = 0;

        public Tuner()
        {
            InitializeComponent();
        }

        private void Tuner_Load(object sender, EventArgs e)
        {
            //lblNum.Visible = false;
            timer1.Enabled = true;
            foreach (var tp in Globals.getInstance().tunerProcesses)
            {
                tp.tuningStarted = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaintBackground(e);
            lblNum.Top = (int)((this.Height - lblNum.Height) / 2);
            lblNum.Left = (int)((this.Width - lblNum.Width) / 2);
            //btnStart.Top = (int)((this.Height - btnStart.Height) / 2);
            //btnStart.Left = (int)((this.Width - btnStart.Width) / 2);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(90, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(brush, e.ClipRectangle);
            }
        }

        private void Tuner_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            foreach (var tp in Globals.getInstance().tunerProcesses)
            {
                tp.tuningStarted = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            num++;
            if (num > 9)
            {
                foreach (var tp in Globals.getInstance().tunerProcesses)
                {
                    tp.OnProcessEnd();
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            lblNum.Text = num.ToString();
            foreach (var tp in Globals.getInstance().tunerProcesses)
            {
                tp.Timer_Tick();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //lblNum.Visible = true;
            //btnStart.Visible = false;
            //timer1.Enabled = true;
            //foreach (var tp in Globals.getInstance().tunerProcesses)
            //{
            //    tp.tuningStarted = true;
            //}
            //this.Update();
        }

        
    }

    public class TunerProcess {
        MMDevice mic;
        byte[] buffer;
        int bytesPerSample;
        bool hasData;
        float maxSensitivity = 1.0f;
        float minSensitivity = 0.8f;
        float maxSensitivityTemp = (float)1.0;
        float minSensitivityTemp = (float)0.7;
        float maxAmplitude = (float)0.3; // refer to: https://manual.audacityteam.org/man/tutorial_making_a_test_recording.html
        float sensitivityStep = (float)0.01;
        Mutex mu = new Mutex();
        List<float> sensitivitySeries;
        public bool tuningStarted = false;
        public float originalVolumeLevel;

        // To search the most proper volume level asap, we use two strategies: BinarySearch and StepSearch
        // BinarySearch first
        private bool isBinarySearch = true; 

        public TunerProcess(MMDevice mic, int bytesPerSample) {
            this.mic = mic;
            hasData = false;
            this.bytesPerSample = bytesPerSample;
            sensitivitySeries = new List<float>();
            originalVolumeLevel = GetVolumeLevel();
            SetVolumeLevel((minSensitivityTemp+ maxSensitivityTemp)/2);
        }

        public void RestoreVolumeLevel() {
            SetVolumeLevel(originalVolumeLevel);
        }

        private void SetVolumeLevel(float value)
        {
            mic.AudioSessionManager.AudioSessionControl.SimpleAudioVolume.Volume = value;
        }

        private float GetVolumeLevel()
        {
            return mic.AudioSessionManager.AudioSessionControl.SimpleAudioVolume.Volume;
        }

        public void OnAudioDataAvailable(object sender, WaveInEventArgs e)
        {
            if (tuningStarted) {
                mu.WaitOne();
                if (!hasData)
                {
                    buffer = e.Buffer.Take(e.BytesRecorded).ToArray();
                    hasData = true;
                }
                else
                {
                    buffer = buffer.Concat(e.Buffer.Take(e.BytesRecorded)).ToArray();
                }
                mu.ReleaseMutex();
            }
        }

        public void OnProcessEnd() {
            int numAverage = 3;
            float sum = 0;
            for (int i = sensitivitySeries.Count - 3; i < sensitivitySeries.Count; i++) {
                sum += sensitivitySeries[i];
            }
            var factor = 1.0f;
            float finalSensitivity = sum / numAverage * factor;
            SetVolumeLevel(finalSensitivity);
        }

        public void Timer_Tick() {
            // Prepare the data first
            mu.WaitOne();
            var buff = buffer.ToArray();
            hasData = false;
            mu.ReleaseMutex();

            // Calc the sensitivity to be set
            var targetSensitivity = GetVolumeLevel() + GetSensitivityDelta(buff);

            if (targetSensitivity > maxSensitivity) targetSensitivity = maxSensitivity;
            else if (targetSensitivity < minSensitivity) targetSensitivity = minSensitivity;

            sensitivitySeries.Add(targetSensitivity);

            //Set the target sensitivity
            SetVolumeLevel(targetSensitivity);
        }

        private float GetSensitivityDelta(byte[] from) {
            var max = GetMaxAmplitude(from);
            if (isBinarySearch)
            {
                var curSensitivity = GetVolumeLevel();
                if (max > maxAmplitude)
                {
                    // reduce the sensitivity
                    maxSensitivityTemp = curSensitivity;
                }
                else
                {
                    minSensitivityTemp = curSensitivity;
                }
                var middle = (maxSensitivityTemp + minSensitivityTemp) / 2;

                if (maxSensitivityTemp - minSensitivityTemp < 2 * sensitivityStep)
                {
                    isBinarySearch = false;
                }
                return middle - curSensitivity;
            }
            else {
                if (max > maxAmplitude)
                {
                    return -sensitivityStep;
                }
                else {
                    return sensitivityStep;
                }
            }
            
            
            var sd = GetStandardDeviation(from);
            if (3.5 * sd < maxAmplitude)
            {
                return sensitivityStep;
            }
            else {
                return -sensitivityStep;
            }
        }

        private float GetMaxAmplitude(byte[] values) {
            var buffer = new WaveBuffer(values);
            float max = float.MinValue;
            for (int i = 0; i < values.Length / bytesPerSample; i++)
            {
                var temp = Math.Abs(buffer.FloatBuffer[i]);
                if (temp > max) {
                    max = temp;
                }
            }
            return max;
        }

        private double GetStandardDeviation(byte[] values)
        {
            var buffer = new WaveBuffer(values);
            double sum = 0;
            for (int i = 0; i < values.Length/bytesPerSample; i++)
            {
                sum += buffer.FloatBuffer[i];
            }
            double average = sum / (values.Length / bytesPerSample);

            double variance = 0;
            for (int i = 0; i < values.Length / bytesPerSample; i++)
            {
                variance += Math.Pow(buffer.FloatBuffer[i] - average, 2);
            }
            //double sum = buffer.FloatBuffer.Sum(v => (v - avg) * (v - avg));
            double denominator = values.Length / bytesPerSample - 1;
            return denominator > 0.0 ? Math.Sqrt(variance / denominator) : -1;
        }
    }
}
