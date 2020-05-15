using K4AdotNet.Record;
using K4AdotNet.Sensor;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKinectRecorder
{
    class IntegratedRecorder:IDisposable
    {
        Recorder cameraRecorder;
        Device camera;
        DeviceConfiguration cameraConfig;
        MMDevice microphone;
        String recordDirectory;
        /// <summary>
        /// This will be used to format part of the file name
        /// </summary>
        private Field field;

        String imageFullFileName;
        String audioFullFillName;

        // For audio recording
        IWaveIn audioCaptureDevice;
        private WaveFileWriter audioFileWriter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="cameraConfig"></param>
        /// <param name="deviceLabel">This will be incoporated into the file name recorded</param>
        /// <param name="mic"></param>
        public IntegratedRecorder(Device camera, DeviceConfiguration cameraConfig, Field field, MMDevice mic) {
            this.camera = camera;
            this.cameraConfig = cameraConfig;
            this.microphone = mic;
            this.field = field;
            if (audioCaptureDevice == null)
            {
                audioCaptureDevice = CreateWaveInDevice();
            }
            // Forcibly turn on the microphone (some programs (Skype) turn it off).
            microphone.AudioEndpointVolume.Mute = false;
            // Not really start to record, while just for enabling calculating the volume peak value
            // refer to: https://github.com/naudio/NAudio/blob/master/Docs/RecordingLevelMeter.md
            audioCaptureDevice.StartRecording();
        }

        public void InitializeRecorder(String recordDirectory, String siteID) {
            this.recordDirectory = recordDirectory;
            var fileName = siteID + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + field.ToString();
            imageFullFileName = Path.Combine(recordDirectory, $"{fileName}.mkv");
            audioFullFillName = Path.Combine(recordDirectory, $"{fileName}.wav");
            audioFileWriter = new WaveFileWriter(audioFullFillName, audioCaptureDevice.WaveFormat);
        }

        public void StartRecord() {
            // get the file name
            cameraRecorder = new Recorder(imageFullFileName, camera, cameraConfig);
            cameraRecorder.WriteHeader();
        }

        private IWaveIn CreateWaveInDevice()
        {
            IWaveIn newWaveIn;
            // can't set WaveFormat as WASAPI doesn't support SRC
            newWaveIn = new WasapiCapture(microphone);
            newWaveIn.DataAvailable += OnAudioDataAvailable;
            // newWaveIn.RecordingStopped += OnRecordingStopped;
            return newWaveIn;
        }

        void OnAudioDataAvailable(object sender, WaveInEventArgs e)
        {
            if (Globals.getInstance().isRecording) {
                //Debug.WriteLine("Flushing Data Available");
                audioFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
                audioFileWriter.Flush();
                //int secondsRecorded = (int)(writer.Length / writer.WaveFormat.AverageBytesPerSecond);
            }
        }

        public void StopRecord() {
            // flush data into hard disk
            cameraRecorder.Flush();
            cameraRecorder.Dispose();

            // audio complete
            audioFileWriter?.Flush();
            audioFileWriter?.Dispose();
            audioFileWriter = null;
        }

        public void NewCaptureArrive(Capture capture) {
            if (Globals.getInstance().isRecording) {
                cameraRecorder.WriteCapture(capture);
                cameraRecorder.Flush();
            }
        }

        public void Dispose() {
            try
            {
                // audio complete
                audioFileWriter?.Dispose();
                audioFileWriter = null;
                audioCaptureDevice.StopRecording();
                audioCaptureDevice.DataAvailable -= OnAudioDataAvailable;
                audioCaptureDevice.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"An exception occured when the {field.ToString()}-view Kinect is being closed!");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        ~IntegratedRecorder() {
            this.Dispose();
        }
    }
}
