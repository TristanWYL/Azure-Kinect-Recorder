using K4AdotNet.Record;
using K4AdotNet.Sensor;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        //String audioFullFillName;

        // For audio recording
        public IWaveIn audioCaptureDevice;
        private List<WaveFileWriter> audioFileWriters;
        WaveFileWriter testWriter;
        String siteID;
        private int indOfNextChannelToWrite = 0;
        private int bytesPerSample;
        private Mutex mutAudioFileProcess;
        private Task taskAudioRecording;
        private List<WaveInEventArgs> audioBufferToRecord;

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
            mutAudioFileProcess = new Mutex();
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
            this.siteID = siteID;
            var fileName = siteID + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + field.ToString();
            indOfNextChannelToWrite = 0;
            bytesPerSample = audioCaptureDevice.WaveFormat.BitsPerSample / 8;
            imageFullFileName = Path.Combine(recordDirectory, $"{fileName}.mkv");
            audioFileWriters = new List<WaveFileWriter>();
            audioBufferToRecord = new List<WaveInEventArgs>();
            var wf_original = audioCaptureDevice.WaveFormat;
            var wf = WaveFormat.CreateCustomFormat(
                wf_original.Encoding, 
                wf_original.SampleRate, 
                1,
                wf_original.AverageBytesPerSecond / wf_original.Channels, 
                wf_original.BlockAlign / wf_original.Channels, 
                wf_original.BitsPerSample);
            for (int i = 0; i < wf_original.Channels; i++) {
                String audioFullFillName = Path.Combine(recordDirectory, $"{fileName}_channel_{i}.wav");
                WaveFileWriter writer = new WaveFileWriter(audioFullFillName, wf);
                audioFileWriters.Add(writer);
            }
            testWriter = new WaveFileWriter(Path.Combine(recordDirectory, $"{fileName}.wav"), audioCaptureDevice.WaveFormat);
        }

        //private void ReInitializeAudioWriter() {
        //    var fileName = siteID + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + field.ToString();
        //    audioFullFillName = Path.Combine(recordDirectory, $"{fileName}.wav");
        //    audioFileWriter = new WaveFileWriter(audioFullFillName, audioCaptureDevice.WaveFormat);
        //}

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
            //newWaveIn.RecordingStopped += OnRecordingStopped;
            return newWaveIn;
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task AudioRecordingProcessAsync() {
            while(true) {
                if (audioBufferToRecord.Count > 0)
                {
                    var curBuffer = audioBufferToRecord.First();
                    for (int i = 0; i < curBuffer.BytesRecorded / bytesPerSample; i++)
                    {
                        audioFileWriters[indOfNextChannelToWrite].Write(curBuffer.Buffer, i * bytesPerSample, bytesPerSample);
                        indOfNextChannelToWrite++;
                        indOfNextChannelToWrite = indOfNextChannelToWrite % audioCaptureDevice.WaveFormat.Channels;
                    }
                    testWriter.Write(curBuffer.Buffer, 0, curBuffer.BytesRecorded);
                    audioBufferToRecord.RemoveAt(0);
                }
                else
                {
                    if (Globals.getInstance().isRecording)
                    {
                        await Task.Delay(100);
                    }
                    else {
                        break;
                    }
                }
            }
            foreach (var writer in audioFileWriters)
            {
                writer.Flush();
                writer.Dispose();
            }
            audioFileWriters = null;
            testWriter.Flush();
            testWriter.Dispose();
            testWriter = null;
            audioBufferToRecord.Clear();
            audioBufferToRecord = null;
        }

        void OnAudioDataAvailable(object sender, WaveInEventArgs e)
        {
            if (Globals.getInstance().isRecording)
            {
                if (taskAudioRecording == null) {
                    taskAudioRecording = Task.Run(()=> AudioRecordingProcessAsync());
                }
                audioBufferToRecord.Add(e);
            }
        }

        public async void StopRecord() {
            // flush data into hard disk
            cameraRecorder.Flush();
            cameraRecorder.Dispose();
            await taskAudioRecording;
            taskAudioRecording = null;
        }

        //private void DisposeAudioWriters() {
        //    mutAudioFileProcess.WaitOne();
            
        //    audioFileWriters = null;
        //    mutAudioFileProcess.ReleaseMutex();
        //}

        public void NewCaptureArrive(Capture capture) {
            if (Globals.getInstance().isRecording) {
                cameraRecorder.WriteCapture(capture);
                cameraRecorder.Flush();
            }
        }

        public void Dispose() {
            mutAudioFileProcess.Dispose();
            try
            {
                // audio complete
                audioCaptureDevice.DataAvailable -= OnAudioDataAvailable;
                audioCaptureDevice.StopRecording();
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
