using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKinectRecorder
{
    class Globals
    {
        private Globals() { }
        private static Globals instance = null;
        public static Globals getInstance() {
            if (instance == null) {
                instance = new Globals();
                instance.dictIsFieldOpen[Field.Close] = false;
                instance.dictIsFieldOpen[Field.Far] = false;
            }
            return instance;
        }

        public bool isRecording = false;
        public Dictionary<Viewer, IntegratedRecorder> viewerRecorderPairs = new Dictionary<Viewer, IntegratedRecorder>();
        public List<TunerProcess> tunerProcesses = new List<TunerProcess>();
        public Dictionary<Field, bool> dictIsFieldOpen = new Dictionary<Field, bool>();
        public bool hasStoppedRecordingButFlushing = false;
    }

    public enum Field { Close, Far}
}
